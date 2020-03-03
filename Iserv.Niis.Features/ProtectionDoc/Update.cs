using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.EarlyReg;
using Iserv.Niis.Model.Models.ProtectionDoc;
using Iserv.Niis.Model.Models.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ProtectionDoc
{
    public class Update
    {
        public class Command : IRequest<ProtectionDocDetailsDto>
        {
            public Command(int protectionDocId, ProtectionDocDetailsDto protectionDocDetailsDto)
            {
                ProtectionDocId = protectionDocId;
                ProtectionDocDetailsDto = protectionDocDetailsDto;
            }

            public int ProtectionDocId { get; }
            public ProtectionDocDetailsDto ProtectionDocDetailsDto { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, ProtectionDocDetailsDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            private const string EarlyRegTypePriorityDataCode = "30 - 300";
            public CommandHandler(NiisWebContext context,
                IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ProtectionDocDetailsDto> Handle(Command message)
            {
                var protectionDocId = message.ProtectionDocDetailsDto.Id = message.ProtectionDocId;
                var protectionDocDetailsDto = message.ProtectionDocDetailsDto;

                var protectionDoc = await _context.ProtectionDocs
                    .Include(r => r.ProtectionDocInfo)
                    .Include(r => r.IcgsProtectionDocs)
                    .Include(r => r.IcisProtectionDocs)
                    .Include(r => r.ColorTzs)
                    .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                    .Include(r => r.Icfems)
                    .Include(r => r.SubType)
                    .Include(r => r.ProtectionDocConventionInfos)
                    .SingleOrDefaultAsync(r => r.Id == protectionDocId);
                if (protectionDoc == null)
                {
                    throw new DataNotFoundException(nameof(Domain.Entities.ProtectionDoc.ProtectionDoc),
                        DataNotFoundException.OperationType.Update, protectionDocId);
                }

                _mapper.Map(protectionDocDetailsDto, protectionDoc);
                ApplyIcgsRequest(protectionDoc, protectionDocDetailsDto.IcgsRequestDtos);
                ApplyIcisIds(protectionDoc, protectionDocDetailsDto.IcisRequestIds);
                ApplyColorIds(protectionDoc, protectionDocDetailsDto.ColorTzIds);
                ApplyRequestInfo(protectionDocDetailsDto, protectionDoc);
                ApplyRequestEarlyRegDtos(protectionDoc, protectionDocDetailsDto.RequestEarlyRegDtos);
                ApplyIcfemIds(protectionDoc, protectionDocDetailsDto.IcfemIds);
                ApplyIpcIds(protectionDoc, protectionDocDetailsDto.IpcIds);
                ApplyRequestConventionInfo(protectionDoc, protectionDocDetailsDto.RequestConventionInfoDtos);

                try
                {
                    await _context.SaveChangesAsync();

                    var protectionDocWithIncludes = await _context.ProtectionDocs
                        .Include(r => r.ProtectionDocInfo).ThenInclude(i => i.BreedCountry)
                        .Include(r => r.Type)
                        .Include(r => r.ProtectionDocCustomers).ThenInclude(r => r.CustomerRole)
                        .Include(r => r.ProtectionDocCustomers).ThenInclude(r => r.Customer).ThenInclude(c => c.Type)
                        .Include(r => r.ProtectionDocCustomers).ThenInclude(r => r.Customer).ThenInclude(c => c.Country)
                        .Include(r => r.Workflows).ThenInclude(r => r.FromStage)
                        .Include(r => r.Workflows).ThenInclude(r => r.CurrentStage)
                        .Include(r => r.Workflows).ThenInclude(r => r.FromUser)
                        .Include(r => r.Workflows).ThenInclude(r => r.CurrentUser)
                        .Include(r => r.Workflows).ThenInclude(r => r.Route)
                        .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                        .SingleAsync(r => r.Id == protectionDocId);
                    return _mapper.Map<Domain.Entities.ProtectionDoc.ProtectionDoc, ProtectionDocDetailsDto>(protectionDocWithIncludes);
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
            }

            private void ApplyColorIds(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc, int[] colorTzIds)
            {
                protectionDoc.ColorTzs
                    .ToList()
                    .ForEach(ir =>
                    {
                        if (colorTzIds.Contains(ir.ColorTzId))
                        {
                            //Exclude existing row
                            colorTzIds = colorTzIds.Where(val => val != ir.ColorTzId).ToArray();
                        }
                        else
                        {
                            //Clear deleted row
                            _context.DicColorTzProtectionDocRelations.Remove(ir);
                        }
                    });

                //Add new rows
                var colorTzRequestRelations = colorTzIds.Select(id =>
                    new DicColorTZProtectionDocRelation { ProtectionDocId = protectionDoc.Id, ColorTzId = id });
                foreach (var colorTzRequestRelation in colorTzRequestRelations)
                {
                    protectionDoc.ColorTzs.Add(colorTzRequestRelation);
                }
            }

            private void ApplyRequestInfo(ProtectionDocDetailsDto protectionDocDetailsDto, Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc)
            {
                var protectionDocInfo = _mapper.Map<ProtectionDocDetailsDto, ProtectionDocInfo>(protectionDocDetailsDto);
                if (protectionDocInfo == null)
                {
                    return;
                }

                protectionDoc.ProtectionDocInfo = protectionDocInfo;
            }

            private void ApplyIcgsRequest(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc, IcgsDto[] icgsRequestDtos)
            {
                DeleteRangeIcgsRequest(protectionDoc, icgsRequestDtos);
                AddRangeIcgsRequest(protectionDoc, icgsRequestDtos);
                UpdateRangeIcgsRequest(protectionDoc, icgsRequestDtos);
            }

            private void AddRangeIcgsRequest(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc, IcgsDto[] icgsDtos)
            {
                if (protectionDoc == null || icgsDtos == null || !icgsDtos.Any())
                {
                    return;
                }
                var newIcgs = ConvertToIcgs(icgsDtos, protectionDoc.Id).Where(x => x.Id == default(int));
                foreach (var icgs in newIcgs)
                {
                    protectionDoc.IcgsProtectionDocs.Add(icgs);
                }
            }

            private void UpdateRangeIcgsRequest(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc, IcgsDto[] icgsDtos)
            {
                if (protectionDoc == null || icgsDtos == null || !icgsDtos.Any())
                {
                    return;
                }
                protectionDoc.IcgsProtectionDocs
                    .ToList()
                    .ForEach(ir =>
                    {
                        icgsDtos = icgsDtos
                            .Where(x => !x.Equals(_mapper.Map<IcgsDto>(ir)) && x.Id.HasValue)
                            .ToArray();
                    });

                var updateIcgs = ConvertToIcgs(icgsDtos, protectionDoc.Id);
                foreach (var icgs in updateIcgs)
                {
                    var originIcgs = protectionDoc.IcgsProtectionDocs.First(x => x.Id == icgs.Id);
                    originIcgs.ClaimedDescription = icgs.ClaimedDescription;
                    originIcgs.Description = icgs.Description;
                    originIcgs.DescriptionKz = icgs.DescriptionKz;
                    originIcgs.NegativeDescription = icgs.NegativeDescription;
                    originIcgs.IcgsId = icgs.IcgsId;
                }
            }
            private void DeleteRangeIcgsRequest(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc, IcgsDto[] icgsDtos)
            {
                if (protectionDoc?.IcgsProtectionDocs == null || !protectionDoc.IcgsProtectionDocs.Any())
                {
                    return;
                }
                if (!icgsDtos.Any())
                {
                    _context.ICGSProtectionDocs.RemoveRange(protectionDoc.IcgsProtectionDocs);
                }
                else
                {
                    protectionDoc.IcgsProtectionDocs
                        .ToList()
                        .ForEach(ir =>
                        {
                            if (!icgsDtos.Select(x => x.Id).Contains(ir.Id))
                            {
                                _context.ICGSProtectionDocs.Remove(ir);
                            }
                        });
                }

            }

            private void ApplyRequestEarlyRegDtos(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc,
                RequestEarlyRegDto[] requestEarlyRegDtos)
            {
                DeleteRequestEarlyRegDtos(protectionDoc, requestEarlyRegDtos);
                AddRequestEarlyRegDtos(protectionDoc, requestEarlyRegDtos);
                UpdateRequestEarlyRegDtos(protectionDoc, requestEarlyRegDtos);
            }

            private void DeleteRequestEarlyRegDtos(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc,
                RequestEarlyRegDto[] earlyRegDtos)
            {
                if (protectionDoc?.EarlyRegs == null || !protectionDoc.EarlyRegs.Any())
                {
                    return;
                }

                if (!earlyRegDtos.Any())
                {
                    _context.ProtectionDocEarlyRegs.RemoveRange(protectionDoc.EarlyRegs);
                }
                else
                {
                    protectionDoc.EarlyRegs
                        .ToList()
                        .ForEach(r =>
                        {
                            if (!earlyRegDtos.Select(x => x.Id).Contains(r.Id))
                            {
                                _context.ProtectionDocEarlyRegs.Remove(r);
                            }
                        });
                }
            }
            private void AddRequestEarlyRegDtos(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc,
                RequestEarlyRegDto[] earlyRegDtos)
            {
                if (protectionDoc == null || earlyRegDtos == null || !earlyRegDtos.Any())
                {
                    return;
                }
                var earlyRegTypePriorityDataId = _context.DicEarlyRegTypes
                    .First(x => x.Code.Equals(EarlyRegTypePriorityDataCode)).Id; // TODO временно выбираем только Приоритетные данные
                var newrequestEarlyRegDtos = earlyRegDtos.Where(x => !x.Id.HasValue);
                var newRequestEarlyRegs =
                    _mapper.Map<IEnumerable<ProtectionDocEarlyReg>>(newrequestEarlyRegDtos);
                foreach (var requestEarlyReg in newRequestEarlyRegs)
                {
                    requestEarlyReg.ProtectionDocId = protectionDoc.Id;
                    requestEarlyReg.EarlyRegTypeId = earlyRegTypePriorityDataId;
                    protectionDoc.EarlyRegs.Add(requestEarlyReg);
                }
            }

            private void UpdateRequestEarlyRegDtos(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc,
                RequestEarlyRegDto[] earlyRegDtos)
            {
                if (protectionDoc == null || earlyRegDtos == null || !earlyRegDtos.Any())
                {
                    return;
                }
                protectionDoc.EarlyRegs
                    .ToList()
                    .ForEach(r =>
                    {
                        earlyRegDtos = earlyRegDtos
                            .Where(x => !x.Equals(_mapper.Map<ProtectionDocEarlyReg>(r)) && x.Id.HasValue)
                            .ToArray();
                    });
                var updateEarlyRegs = _mapper.Map<IEnumerable<ProtectionDocEarlyReg>>(earlyRegDtos);
                foreach (var earlyReg in updateEarlyRegs)
                {
                    var originEarlyReg = protectionDoc.EarlyRegs.First(x => x.Id == earlyReg.Id);
                    originEarlyReg.RegCountryId = earlyReg.RegCountryId;
                    originEarlyReg.RegNumber = earlyReg.RegNumber;
                    originEarlyReg.RegDate = earlyReg.RegDate;
                    originEarlyReg.EarlyRegTypeId = earlyReg.EarlyRegTypeId;
                }
            }


            private void ApplyIcisIds(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc, int[] icisIds)
            {
                protectionDoc.IcisProtectionDocs
                    .ToList()
                    .ForEach(ir =>
                    {
                        if (icisIds.Contains(ir.IcisId))
                        {
                            //Exclude existing row
                            icisIds = icisIds.Where(val => val != ir.IcisId).ToArray();
                        }
                        else
                        {
                            //Clear deleted row
                            _context.ICISProtectionDocs.Remove(ir);
                        }
                    });

                //Add new rows
                var newIcisProtectionDocs = icisIds.Select(id => new ICISProtectionDoc { ProtectionDocId = protectionDoc.Id, IcisId = id });
                foreach (var icis in newIcisProtectionDocs)
                {
                    protectionDoc.IcisProtectionDocs.Add(icis);
                }
            }

            private void ApplyIcfemIds(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc, int[] icfemIds)
            {
                protectionDoc.Icfems
                    .ToList()
                    .ForEach(ir =>
                    {
                        if (icfemIds.Contains(ir.DicIcfemId))
                        {
                            //Exclude existing row
                            icfemIds = icfemIds.Where(val => val != ir.DicIcfemId).ToArray();
                        }
                        else
                        {
                            //Clear deleted row
                            _context.DicIcfemProtectionDocRelations.Remove(ir);
                        }
                    });

                //Add new rows
                var newIcfems = icfemIds.Select(id => new DicIcfemProtectionDocRelation() { ProtectionDocId = protectionDoc.Id, DicIcfemId = id });
                foreach (var newIcfem in newIcfems)
                {
                    protectionDoc.Icfems.Add(newIcfem);
                }
            }

            private void ApplyIpcIds(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc, int[] ipcIds)
            {
                protectionDoc.IpcProtectionDocs
                    .ToList()
                    .ForEach(ip =>
                    {
                        if (ipcIds.Contains(ip.IpcId))
                        {
                            //Exclude existing row
                            ipcIds = ipcIds.Where(val => val != ip.IpcId).ToArray();
                        }
                        else
                        {
                            //Clear deleted row
                            _context.IpcProtectionDocs.Remove(ip);
                        }
                    });
                //Add new rows
                var newIpcs = ipcIds.Select(id => new IPCProtectionDoc { ProtectionDocId = protectionDoc.Id, IpcId = id });
                foreach (var newIpc in newIpcs)
                {
                    protectionDoc.IpcProtectionDocs.Add(newIpc);
                }
            }

            private void ApplyRequestConventionInfo(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc,
                ConventionInfoDto[] requestConventionInfoDtos)
            {
                DeleteRequestConventionInfo(protectionDoc, requestConventionInfoDtos);
                AddRangeRequestConventionInfo(protectionDoc, requestConventionInfoDtos);
                UpdateRequestConventionInfo(protectionDoc, requestConventionInfoDtos);
            }

            private void AddRangeRequestConventionInfo(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc,
                ConventionInfoDto[] conventionInfoDtos)
            {
                if (protectionDoc == null || conventionInfoDtos == null || !conventionInfoDtos.Any())
                {
                    return;
                }

                var newRequestConventionInfoDtos =
                    _mapper.Map<IEnumerable<ProtectionDocConventionInfo>>(
                        conventionInfoDtos.Where(r => !r.Id.HasValue));
                foreach (var item in newRequestConventionInfoDtos)
                {
                    protectionDoc.ProtectionDocConventionInfos.Add(item);
                }
            }

            private void UpdateRequestConventionInfo(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc,
                ConventionInfoDto[] conventionInfoDtos)
            {
                if (protectionDoc == null || conventionInfoDtos == null || !conventionInfoDtos.Any())
                {
                    return;
                }
                protectionDoc.ProtectionDocConventionInfos
                    .ToList()
                    .ForEach(originRc =>
                    {
                        conventionInfoDtos = conventionInfoDtos.Where(rc =>
                            !rc.Equals(_mapper.Map<ConventionInfoDto>(originRc)) && rc.Id.HasValue).ToArray();
                    });
                var updateConventionInfos =
                    _mapper.Map<IEnumerable<ProtectionDocConventionInfo>>(conventionInfoDtos);
                foreach (var itemConventionInfo in updateConventionInfos)
                {
                    var originConventionInfo =
                        protectionDoc.ProtectionDocConventionInfos.First(rc => rc.Id == itemConventionInfo.Id);
                    originConventionInfo.CountryId = itemConventionInfo.CountryId;
                    originConventionInfo.EarlyRegTypeId = itemConventionInfo.EarlyRegTypeId;
                    originConventionInfo.DateInternationalApp = itemConventionInfo.DateInternationalApp;
                    originConventionInfo.HeadIps = itemConventionInfo.HeadIps;
                    originConventionInfo.RegNumberInternationalApp = itemConventionInfo.RegNumberInternationalApp;
                    originConventionInfo.TermNationalPhaseFirsChapter = itemConventionInfo.TermNationalPhaseFirsChapter;
                    originConventionInfo.TermNationalPhaseSecondChapter =
                        itemConventionInfo.TermNationalPhaseSecondChapter;
                }

            }

            private void DeleteRequestConventionInfo(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc,
                ConventionInfoDto[] requestConventionInfoDtos)
            {
                if (protectionDoc?.ProtectionDocConventionInfos == null || !protectionDoc.ProtectionDocConventionInfos.Any())
                {
                    return;
                }

                if (!requestConventionInfoDtos.Any())
                {
                    _context.ProtectionDocConventionInfos.RemoveRange(protectionDoc.ProtectionDocConventionInfos);
                }
                else
                {
                    protectionDoc.ProtectionDocConventionInfos
                        .ToList()
                        .ForEach(rc =>
                        {
                            if (!requestConventionInfoDtos.Select(x => x.Id).Contains(rc.Id))
                            {
                                _context.ProtectionDocConventionInfos.Remove(rc);
                            }
                        });
                }
            }
            private IEnumerable<ICGSProtectionDoc> ConvertToIcgs(IcgsDto[] icgsRequestDtos, int protectionDocId)
            {
                return icgsRequestDtos
                    .Select(icgsRequestDto => new ICGSProtectionDoc()
                    {
                        Id = icgsRequestDto.Id ?? default(int),
                        ProtectionDocId = protectionDocId,
                        IcgsId = icgsRequestDto.IcgsId,
                        ClaimedDescription = icgsRequestDto.ClaimedDescription,
                        Description = icgsRequestDto.Description,
                        DescriptionKz = icgsRequestDto.DescriptionKz,
                        NegativeDescription = icgsRequestDto.NegativeDescription
                    });
            }
        }
    }
}
