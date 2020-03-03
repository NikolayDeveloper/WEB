using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Icfem.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Icgs.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Icis.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Info.ProtectionDocInfos;
using Iserv.Niis.BusinessLogic.Bibliographics.Ipc.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bulletin;
using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.EarlyReg;
using Iserv.Niis.Model.Models.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class UpdateProtectionDocHandler: BaseHandler
    {
        private readonly IMapper _mapper;
        public UpdateProtectionDocHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Handle(ProtectionDoc protectionDoc, ProtectionDocDetailsDto protectionDocDetailsDto)
        {
            await ApplyIcgsProtectionDoc(protectionDoc, protectionDocDetailsDto.IcgsRequestDtos);
            await ApplyIcisIds(protectionDoc, protectionDocDetailsDto.IcisRequestIds.ToList());
            await ApplyColorIds(protectionDoc, protectionDocDetailsDto.ColorTzIds.ToList());
            await ApplyProtectionDocInfo(protectionDocDetailsDto, protectionDoc);
            await ApplyProtectionDocEarlyRegDtos(protectionDoc, protectionDocDetailsDto.RequestEarlyRegDtos);
            await ApplyIcfemIds(protectionDoc, protectionDocDetailsDto.IcfemIds.ToList());
            await ApplyIpcIds(protectionDoc, protectionDocDetailsDto.IpcIds.ToList(), protectionDocDetailsDto.MainIpcId);
            await ApplyProtectionDocConventionInfo(protectionDoc, protectionDocDetailsDto.RequestConventionInfoDtos);
            await UpdateBulletin(protectionDoc, protectionDocDetailsDto);
        }

        private async Task UpdateBulletin(ProtectionDoc protectionDoc, ProtectionDocDetailsDto details)
        {
            var relation = protectionDoc.Bulletins.FirstOrDefault(b => b.IsPublish);
            if (relation != null)
            {
                relation.BulletinId = details.BulletinId ?? 0;
                await Executor.GetCommand<UpdateProtectionDocBulletinRelationCommand>()
                    .Process(c => c.ExecuteAsync(relation));
            }
        }

        private async Task ApplyProtectionDocConventionInfo(ProtectionDoc protectionDoc,
            ConventionInfoDto[] protectionDocConventionInfoDtos)
        {
            await DeleteProtectionDocConventionInfo(protectionDoc, protectionDocConventionInfoDtos);
            await AddRangeProtectionDocConventionInfo(protectionDoc, protectionDocConventionInfoDtos);
            await UpdateProtectionDocConventionInfo(protectionDoc, protectionDocConventionInfoDtos);
        }


        private async Task AddRangeProtectionDocConventionInfo(ProtectionDoc protectionDoc,
            ConventionInfoDto[] protectionDocConventionInfoDtos)
        {
            if (protectionDoc == null || protectionDocConventionInfoDtos == null || !protectionDocConventionInfoDtos.Any())
            {
                return;
            }

            var newProtectionDocConventionInfos = _mapper.Map<List<ProtectionDocConventionInfo>>(protectionDocConventionInfoDtos.Where(r => !r.Id.HasValue)).ToList();
            await Executor.GetCommand<AddProtectionDocConventionInfosRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, newProtectionDocConventionInfos));
        }

        private async Task UpdateProtectionDocConventionInfo(ProtectionDoc protectionDoc, ConventionInfoDto[] protectionDocConventionInfoDtos)
        {
            if (protectionDoc == null || protectionDocConventionInfoDtos == null || !protectionDocConventionInfoDtos.Any() || protectionDoc.ProtectionDocConventionInfos == null || !protectionDoc.ProtectionDocConventionInfos.Any())
            {
                return;
            }
            protectionDoc.ProtectionDocConventionInfos
                .ToList()
                .ForEach(originRc =>
                {
                    protectionDocConventionInfoDtos = protectionDocConventionInfoDtos.Where(rc =>
                        !rc.Equals(_mapper.Map<ConventionInfoDto>(originRc)) && rc.Id.HasValue).ToArray();
                });
            var updateProtectionDocConventionInfos = _mapper.Map<IEnumerable<ProtectionDocConventionInfo>>(protectionDocConventionInfoDtos).ToList();
            await Executor.GetCommand<UpdateProtectionDocConventionInfosRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, updateProtectionDocConventionInfos));
        }

        private async Task DeleteProtectionDocConventionInfo(ProtectionDoc protectionDoc,
            ConventionInfoDto[] protectionDocConventionInfoDtos)
        {
            if (protectionDoc?.ProtectionDocConventionInfos == null || !protectionDoc.ProtectionDocConventionInfos.Any())
            {
                return;
            }

            var protectionDocConventionInfos = protectionDoc.ProtectionDocConventionInfos.ToList();
            if (!protectionDocConventionInfoDtos.Any())
            {
                await Executor.GetCommand<RemoveProtectionDocConventionInfosRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, protectionDocConventionInfos));
            }
            else
            {
                var deleteProtectionDocConventionInfos = protectionDocConventionInfos
                    .Where(rc => !protectionDocConventionInfoDtos.Select(x => x.Id).Contains(rc.Id))
                    .ToList();
                await Executor.GetCommand<RemoveProtectionDocConventionInfosRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, deleteProtectionDocConventionInfos));
            }

        }

        private async Task ApplyIpcIds(ProtectionDoc protectionDoc, List<int> ipcIds, int? mainIpcId)
        {
            var originIpcIds = protectionDoc.IpcProtectionDocs.Select(ip => ip.IpcId).ToList();
            var removeIpcIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h => h.Execute(ipcIds, originIpcIds));
            if (removeIpcIds.Any())
            {
                await Executor.GetCommand<RemoveIpcProtectionDocRelationsCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, removeIpcIds));
            }

            var addIcfemIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h => h.Execute(ipcIds, originIpcIds));
            if (addIcfemIds.Any())
            {
                await Executor.GetCommand<AddIpcProtectionDocRelationsCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, addIcfemIds));
            }
            if (mainIpcId.HasValue)
            {
                Executor.GetCommand<SetMainIpcOnIpcProtectionDocCommand>().Process(c => c.Execute(protectionDoc.Id, mainIpcId.Value));
            }
        }


        private async Task ApplyIcfemIds(ProtectionDoc protectionDoc, List<int> icfemIds)
        {
            var originIcfemIds = protectionDoc.Icfems.Select(i => i.DicIcfemId).ToList();
            var removeIcfemIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h => h.Execute(icfemIds, originIcfemIds));
            if (removeIcfemIds.Any())
            {
                await Executor.GetCommand<RemoveIcfemProtectionDocRelationsCommand>()
                    .Process(c => c.ExecuteAsync(protectionDoc.Id, removeIcfemIds));
            }

            var addIcfemIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h => h.Execute(icfemIds, originIcfemIds));
            if (addIcfemIds.Any())
            {
                await Executor.GetCommand<AddIcfemProtectionDocRelationsCommand>()
                    .Process(c => c.ExecuteAsync(protectionDoc.Id, addIcfemIds));
            }
        }


        private async Task ApplyProtectionDocEarlyRegDtos(ProtectionDoc protectionDoc, RequestEarlyRegDto[] protectionDocEarlyRegDtos)
        {
            await DeleteProtectionDocEarlyRegDtos(protectionDoc, protectionDocEarlyRegDtos);
            await AddProtectionDocEarlyRegDtos(protectionDoc, protectionDocEarlyRegDtos);
            await UpdateProtectionDocEarlyRegDtos(protectionDoc, protectionDocEarlyRegDtos);
        }

        private async Task UpdateProtectionDocEarlyRegDtos(ProtectionDoc protectionDoc,
            RequestEarlyRegDto[] protectionDocEarlyRegDtos)
        {
            if (protectionDoc == null || protectionDocEarlyRegDtos == null || !protectionDocEarlyRegDtos.Any() || !protectionDoc.EarlyRegs.Any())
            {
                return;
            }
            protectionDoc.EarlyRegs
                .ToList()
                .ForEach(r =>
                {
                    protectionDocEarlyRegDtos = protectionDocEarlyRegDtos
                        .Where(x => !x.Equals(_mapper.Map<RequestEarlyRegDto>(r)) && x.Id.HasValue)
                        .ToArray();
                });
            var updateProtectionDocEarlyRegs = _mapper.Map<List<ProtectionDocEarlyReg>>(protectionDocEarlyRegDtos).ToList();
            await Executor.GetCommand<UpdateProtectionDocEarlyRegsRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, updateProtectionDocEarlyRegs));
        }


        private async Task AddProtectionDocEarlyRegDtos(ProtectionDoc protectionDoc,
            RequestEarlyRegDto[] protectionDocEarlyRegDtos)
        {
            if (protectionDoc == null || protectionDocEarlyRegDtos == null || !protectionDocEarlyRegDtos.Any())
            {
                return;
            }
            var newprotectionDocEarlyRegDtos = protectionDocEarlyRegDtos.Where(x => !x.Id.HasValue);
            var newProtectionDocEarlyRegs = _mapper.Map<List<ProtectionDocEarlyReg>>(newprotectionDocEarlyRegDtos).ToList();
            await Executor.GetCommand<AddProtectionDocEarlyRegsRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, newProtectionDocEarlyRegs));
        }


        private async Task DeleteProtectionDocEarlyRegDtos(ProtectionDoc protectionDoc,
            RequestEarlyRegDto[] protectionDocEarlyRegDtos)
        {
            if (protectionDoc?.EarlyRegs == null || !protectionDoc.EarlyRegs.Any())
            {
                return;
            }

            var earlyRegs = protectionDoc.EarlyRegs.ToList();
            if (!protectionDocEarlyRegDtos.Any())
            {
                await Executor.GetCommand<RemoveProtectionDocEarlyRegsRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, earlyRegs));
            }
            else
            {
                var deleteEarlyRegs = earlyRegs.Where(r => !protectionDocEarlyRegDtos.Select(x => x.Id).Contains(r.Id)).ToList();
                await Executor.GetCommand<RemoveProtectionDocEarlyRegsRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, deleteEarlyRegs));
            }
        }


        private async Task ApplyProtectionDocInfo(ProtectionDocDetailsDto protectionDocDetailsDto, ProtectionDoc protectionDoc)
        {
            var protectionDocInfo = _mapper.Map<ProtectionDocDetailsDto, ProtectionDocInfo>(protectionDocDetailsDto);
            if (protectionDocInfo == null)
            {
                return;
            }
            if (protectionDoc.ProtectionDocInfo == null)
            {
                Executor.GetCommand<CreateProtectionDocInfoCommand>().Process(c => c.Execute(protectionDoc.Id, protectionDocInfo));
            }
            else
            {
                await Executor.GetCommand<UpdateProtectionDocInfoCommand>().Process(c => c.ExecuteAsync(protectionDocInfo));
            }
        }


        private async Task ApplyColorIds(ProtectionDoc protectionDoc, List<int> colorTzIds)
        {
            var originColorTzIds = protectionDoc.ColorTzs.Select(c => c.ColorTzId).ToList();

            var removeColorTzIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h => h.Execute(colorTzIds, originColorTzIds));
            if (removeColorTzIds.Any())
            {
                await Executor.GetCommand<RemoveColorTzProtectionDocRelationsCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, removeColorTzIds));
            }

            var addColorTzIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h => h.Execute(colorTzIds, originColorTzIds));
            if (addColorTzIds.Any())
            {
                await Executor.GetCommand<AddColorTzProtectionDocRelationsCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, addColorTzIds));
            }
        }


        private async Task ApplyIcisIds(ProtectionDoc protectionDoc, List<int> icisIds)
        {
            var originIcisIds = protectionDoc.IcisProtectionDocs.Select(i => i.IcisId).ToList();

            var removeIcisIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>()
                .Process(h => h.Execute(icisIds, originIcisIds));
            if (removeIcisIds.Any())
            {
                await Executor.GetCommand<RemoveIcisProtectionDocRelationsCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, removeIcisIds));
            }

            var addIcisIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>()
                .Process(h => h.Execute(icisIds, originIcisIds));
            if (addIcisIds.Any())
            {
                await Executor.GetCommand<AddIcisProtectionDocRelationsCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, addIcisIds));
            }
        }

        private async Task ApplyIcgsProtectionDoc(ProtectionDoc protectionDoc, IcgsDto[] icgsProtectionDocDtos)
        {
            await DeleteRangeIcgsProtectionDoc(protectionDoc, icgsProtectionDocDtos);
            await AddRangeIcgsProtectionDoc(protectionDoc, icgsProtectionDocDtos);
            await UpdateRangeIcgsProtectionDoc(protectionDoc, icgsProtectionDocDtos);
        }

        private IEnumerable<ICGSProtectionDoc> ConvertToIcgsProtectionDocs(IcgsDto[] icgsProtectionDocDtos, int protectionDocId)
        {
            return icgsProtectionDocDtos
                .Select(icgsProtectionDocDto => new ICGSProtectionDoc
                {
                    Id = icgsProtectionDocDto.Id ?? default(int),
                    ProtectionDocId = protectionDocId,
                    IcgsId = icgsProtectionDocDto.IcgsId,
                    ClaimedDescription = icgsProtectionDocDto.ClaimedDescription,
                    ClaimedDescriptionEn = icgsProtectionDocDto.ClaimedDescriptionEn,
                    Description = icgsProtectionDocDto.Description,
                    DescriptionKz = icgsProtectionDocDto.DescriptionKz,
                    NegativeDescription = icgsProtectionDocDto.NegativeDescription,
                    IsRefused = icgsProtectionDocDto.IsRefused,
                    IsPartialRefused = icgsProtectionDocDto.IsPartialRefused,
                    ReasonForPartialRefused = icgsProtectionDocDto.ReasonForPartialRefused,
                });
        }

        private async Task UpdateRangeIcgsProtectionDoc(ProtectionDoc protectionDoc, IcgsDto[] icgsProtectionDocDtos)
        {
            var isNotNeedUpdateIcgsProtectionDoc = (protectionDoc == null || icgsProtectionDocDtos == null || !icgsProtectionDocDtos.Any() || !protectionDoc.IcgsProtectionDocs.Any());
            if (isNotNeedUpdateIcgsProtectionDoc)
            {
                return;
            }

            protectionDoc.IcgsProtectionDocs
                .ToList()
                .ForEach(ir =>
                {
                    icgsProtectionDocDtos = icgsProtectionDocDtos
                        .Where(x => !x.Equals(_mapper.Map<IcgsDto>(ir)) && x.Id.HasValue)
                        .ToArray();
                });

            var updateIcgsProtectionDocs = ConvertToIcgsProtectionDocs(icgsProtectionDocDtos, protectionDoc.Id).ToList();
            await Executor.GetCommand<UpdateIcgsProtectionDocRangeCommand>()
               .Process(c => c.ExecuteAsync(protectionDoc.Id, updateIcgsProtectionDocs));
        }

        private async Task AddRangeIcgsProtectionDoc(ProtectionDoc protectionDoc, IcgsDto[] icgsProtectionDocDtos)
        {
            if (protectionDoc == null || icgsProtectionDocDtos == null || !icgsProtectionDocDtos.Any())
            {
                return;
            }
            var newIcgsProtectionDocs = ConvertToIcgsProtectionDocs(icgsProtectionDocDtos, protectionDoc.Id).Where(x => x.Id == default(int)).ToList();
            await Executor.GetCommand<AddIcgsProtectionDocRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, newIcgsProtectionDocs));
        }


        private async Task DeleteRangeIcgsProtectionDoc(ProtectionDoc protectionDoc, IcgsDto[] icgsProtectionDocDtos)
        {

            if (protectionDoc?.IcgsProtectionDocs == null || !protectionDoc.IcgsProtectionDocs.Any())
            {
                return;
            }
            if (!icgsProtectionDocDtos.Any())
            {
                await Executor.GetCommand<RemoveIcgsProtectionDocRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, protectionDoc.IcgsProtectionDocs.ToList()));
            }
            else
            {
                var icgsProtectionDocsForDelete = protectionDoc.IcgsProtectionDocs
                    .Where(ir => !icgsProtectionDocDtos.Select(x => x.Id).Contains(ir.Id))
                    .ToList();
                if (icgsProtectionDocsForDelete.Any())
                {
                    await Executor.GetCommand<RemoveIcgsProtectionDocRangeCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, icgsProtectionDocsForDelete));
                }
            }
        }
    }
}
