using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Icfem.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Icgs.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Icis.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Info.RequestInfos;
using Iserv.Niis.BusinessLogic.Bibliographics.Ipc.Request;
using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.EarlyReg;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class UpdateRequestHandler : BaseHandler
    {
        private readonly IMapper _mapper;
        public UpdateRequestHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Handle(Request request, RequestDetailDto requestDto)
        {
            await ApplyIcgsRequest(request, requestDto.IcgsRequestDtos);
            await ApplyIcisIds(request, requestDto.IcisRequestIds.ToList());
            await ApplyColorIds(request, requestDto.ColorTzIds.ToList());
            await ApplyRequestInfo(requestDto, request);
            await ApplyRequestEarlyRegDtos(request, requestDto.RequestEarlyRegDtos);
            await ApplyIcfemIds(request, requestDto.IcfemIds.ToList());
            await ApplyIpcIds(request, requestDto.IpcIds.ToList(), requestDto.MainIpcId);
            await ApplyRequestConventionInfo(request, requestDto.RequestConventionInfoDtos);
        }

        private async Task ApplyRequestConventionInfo(Request request,
            ConventionInfoDto[] requestConventionInfoDtos)
        {
            await DeleteRequestConventionInfo(request, requestConventionInfoDtos);
            await AddRangeRequestConventionInfo(request, requestConventionInfoDtos);
            await UpdateRequestConventionInfo(request, requestConventionInfoDtos);
        }


        private async Task AddRangeRequestConventionInfo(Request request,
            ConventionInfoDto[] requestConventionInfoDtos)
        {
            if (request == null || requestConventionInfoDtos == null || !requestConventionInfoDtos.Any())
            {
                return;
            }

            var newRequestConventionInfos = _mapper.Map<IEnumerable<RequestConventionInfo>>(requestConventionInfoDtos.Where(r => !r.Id.HasValue)).ToList();
            await Executor.GetCommand<AddRequestConventionInfosRangeCommand>().Process(c => c.ExecuteAsync(request.Id, newRequestConventionInfos));
        }

        private async Task UpdateRequestConventionInfo(Request request, ConventionInfoDto[] requestConventionInfoDtos)
        {
            if (request == null || requestConventionInfoDtos == null || !requestConventionInfoDtos.Any() || request.RequestConventionInfos == null || !request.RequestConventionInfos.Any())
            {
                return;
            }
            request.RequestConventionInfos
                .ToList()
                .ForEach(originRc =>
                {
                    requestConventionInfoDtos = requestConventionInfoDtos.Where(rc =>
                        !rc.Equals(_mapper.Map<ConventionInfoDto>(originRc)) && rc.Id.HasValue).ToArray();
                });
            var updateRequestConventionInfos = _mapper.Map<IEnumerable<RequestConventionInfo>>(requestConventionInfoDtos).ToList();
            await Executor.GetCommand<UpdateRequestConventionInfosRangeCommand>().Process(c => c.ExecuteAsync(request.Id, updateRequestConventionInfos));
        }

        private async Task DeleteRequestConventionInfo(Request request,
            ConventionInfoDto[] requestConventionInfoDtos)
        {
            if (request?.RequestConventionInfos == null || !request.RequestConventionInfos.Any())
            {
                return;
            }

            var requestConventionInfos = request.RequestConventionInfos.ToList();
            if (!requestConventionInfoDtos.Any())
            {
                await Executor.GetCommand<DeleteRangeRequestConventionInfoCommand>().Process(c => c.ExecuteAsync(requestConventionInfos));
            }
            else
            {
                var deleteRequestConventionInfos = requestConventionInfos
                    .Where(rc => !requestConventionInfoDtos.Select(x => x.Id).Contains(rc.Id))
                    .ToList();
                await Executor.GetCommand<DeleteRangeRequestConventionInfoCommand>().Process(c => c.ExecuteAsync(deleteRequestConventionInfos));
            }

        }

        private async Task ApplyIpcIds(Request request, List<int> ipcIds, int? mainIpcId)
        {
            var originIpcIds = request.IPCRequests.Select(ip => ip.IpcId).ToList();
            var removeIpcIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h => h.Execute(ipcIds, originIpcIds));
            if (removeIpcIds.Any())
            {
                var removeIpcRequest = await Executor.GetQuery<GetIpcByRequestIdAndIpcIdsQuery>().Process(q => q.ExecuteAsync(request.Id, removeIpcIds));
                await Executor.GetCommand<DeleteRangeIpcRequestCommand>().Process(c => c.ExecuteAsync(removeIpcRequest));
            }

            var addIcfemIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h => h.Execute(ipcIds, originIpcIds));
            if (addIcfemIds.Any())
            {
                await Executor.GetCommand<AddIpcRequestRelationsCommand>().Process(c => c.ExecuteAsync(request.Id, addIcfemIds));
            }
            if (mainIpcId.HasValue)
            {
                Executor.GetCommand<SetMainIpcOnIpcRequestCommand>().Process(c => c.Execute(request.Id, mainIpcId.Value));
            }
        }


        private async Task ApplyIcfemIds(Request request, List<int> icfemIds)
        {
            var originIcfemIds = request.Icfems.Select(i => i.DicIcfemId).ToList();
            var removeIcfemIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h => h.Execute(icfemIds, originIcfemIds));
            if (removeIcfemIds.Any())
            {
                var removeIcfemRequest = await Executor.GetQuery<GetIcfemRequestsByRequestIdAndIcfemIdsQuery>()
                    .Process(q => q.ExeciuteAsync(request.Id, removeIcfemIds));
                await Executor.GetCommand<DeleteRangeIcfemRequestCommand>()
                    .Process(c => c.ExecuteAsync(removeIcfemRequest));
            }

            var addIcfemIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h => h.Execute(icfemIds, originIcfemIds));
            if (addIcfemIds.Any())
            {
                await Executor.GetCommand<AddIcfemRequestRelationsCommand>()
                    .Process(c => c.ExecuteAsync(request.Id, addIcfemIds));
            }
        }


        private async Task ApplyRequestEarlyRegDtos(Request request, RequestEarlyRegDto[] requestEarlyRegDtos)
        {
            await DeleteRequestEarlyRegDtos(request, requestEarlyRegDtos);
            await AddRequestEarlyRegDtos(request, requestEarlyRegDtos);
            await UpdateRequestEarlyRegDtos(request, requestEarlyRegDtos);
        }

        private async Task UpdateRequestEarlyRegDtos(Request request,
            RequestEarlyRegDto[] requestEarlyRegDtos)
        {
            if (request == null || requestEarlyRegDtos == null || !requestEarlyRegDtos.Any() || !request.EarlyRegs.Any())
            {
                return;
            }
            request.EarlyRegs
                .ToList()
                .ForEach(r =>
                {
                    requestEarlyRegDtos = requestEarlyRegDtos
                        .Where(x => !x.Equals(_mapper.Map<RequestEarlyRegDto>(r)) && x.Id.HasValue)
                        .ToArray();
                });
            var updateRequestEarlyRegs = _mapper.Map<IEnumerable<RequestEarlyReg>>(requestEarlyRegDtos).ToList();
            await Executor.GetCommand<UpdateRequestEarlyRegsRangeCommand>().Process(c => c.ExecuteAsync(request.Id, updateRequestEarlyRegs));
        }


        private async Task AddRequestEarlyRegDtos(Request request,
            RequestEarlyRegDto[] requestEarlyRegDtos)
        {
            if (request == null || requestEarlyRegDtos == null || !requestEarlyRegDtos.Any())
            {
                return;
            }
            var newrequestEarlyRegDtos = requestEarlyRegDtos.Where(x => !x.Id.HasValue);
            var newRequestEarlyRegs = _mapper.Map<IEnumerable<RequestEarlyReg>>(newrequestEarlyRegDtos).ToList();
            await Executor.GetCommand<AddRequestEarlyRegsRangeCommand>().Process(c => c.ExecuteAsync(request.Id, newRequestEarlyRegs));
        }


        private async Task DeleteRequestEarlyRegDtos(Request request,
            RequestEarlyRegDto[] requestEarlyRegDtos)
        {
            if (request?.EarlyRegs == null || !request.EarlyRegs.Any())
            {
                return;
            }

            var earlyRegs = request.EarlyRegs.ToList();
            if (!requestEarlyRegDtos.Any())
            {
                await Executor.GetCommand<DeleteRangeRequestEarlyRegsCommand>().Process(c => c.ExecuteAsync(earlyRegs));
            }
            else
            {
                var deleteEarlyRegs = earlyRegs.Where(r => !requestEarlyRegDtos.Select(x => x.Id).Contains(r.Id)).ToList();
                await Executor.GetCommand<DeleteRangeRequestEarlyRegsCommand>().Process(c => c.ExecuteAsync(deleteEarlyRegs));
            }
        }


        private async Task ApplyRequestInfo(RequestDetailDto requestDto, Request request)
        {
            var requestInfo = _mapper.Map<RequestDetailDto, RequestInfo>(requestDto);
            if (requestInfo == null)
            {
                return;
            }
            if (request.RequestInfo == null)
            {
                await Executor.GetCommand<CreateRequestInfoCommand>().Process(c => c.ExecuteAsync(requestInfo));
            }
            else
            {
                await Executor.GetCommand<UpdateRequestInfoCommand>().Process(c => c.ExecuteAsync(requestInfo));
            }
        }


        private async Task ApplyColorIds(Request request, List<int> colorTzIds)
        {
            var originColorTzIds = request.ColorTzs.Select(c => c.ColorTzId).ToList();

            var removeColorTzIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h => h.Execute(colorTzIds, originColorTzIds));
            if (removeColorTzIds.Any())
            {
                var removeColorTzRequestRelations = await Executor.GetQuery<GetColorTzsByRequestIdAndColorIdsQuery>()
                    .Process(c => c.ExecuteAsync(request.Id, removeColorTzIds));
                await Executor.GetCommand<DeleteRangeColorTzRequestRelationCommand>().Process(c => c.ExecuteAsync(removeColorTzRequestRelations));
            }

            var addColorTzIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h => h.Execute(colorTzIds, originColorTzIds));
            if (addColorTzIds.Any())
            {
                await Executor.GetCommand<AddColorTzRequestRelationCommand>().Process(c => c.ExecuteAsync(request.Id, addColorTzIds));
            }
        }


        private async Task ApplyIcisIds(Request request, List<int> icisIds)
        {
            var originIcisIds = request.ICISRequests.Select(i => i.IcisId).ToList();

            var removeIcisIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>()
                .Process(h => h.Execute(icisIds, originIcisIds));
            if (removeIcisIds.Any())
            {
                var removeIcisRequests = await Executor.GetQuery<GetICISRequestsByRequestIdAndIcisIdsQuery>().Process(q => q.ExecuteAsync(request.Id, removeIcisIds));
                await Executor.GetCommand<DeleteRangeICISRequestCommand>().Process(c => c.ExecuteAsync(removeIcisRequests));
            }

            var addIcisIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>()
                .Process(h => h.Execute(icisIds, originIcisIds));
            if (addIcisIds.Any())
            {
                await Executor.GetCommand<AddIcisRequestRelationsCommand>().Process(c => c.ExecuteAsync(request.Id, addIcisIds));
            }
        }

        private async Task ApplyIcgsRequest(Request request, IcgsDto[] icgsRequestDtos)
        {
            await DeleteRangeIcgsRequest(request, icgsRequestDtos);
            await AddRangeIcgsRequest(request, icgsRequestDtos);
            await UpdateRangeIcgsRequest(request, icgsRequestDtos);
        }

        private IEnumerable<ICGSRequest> ConvertToIcgsRequests(IcgsDto[] icgsRequestDtos, int requestId)
        {
            return icgsRequestDtos
                .Select(icgsRequestDto => new ICGSRequest
                {
                    Id = icgsRequestDto.Id ?? default(int),
                    RequestId = requestId,
                    IcgsId = icgsRequestDto.IcgsId,
                    ClaimedDescription = icgsRequestDto.ClaimedDescription,
                    ClaimedDescriptionEn = icgsRequestDto.ClaimedDescriptionEn,
                    Description = icgsRequestDto.Description,
                    DescriptionKz = icgsRequestDto.DescriptionKz,
                    NegativeDescription = icgsRequestDto.NegativeDescription,
                    IsRefused = icgsRequestDto.IsRefused,
                    IsPartialRefused = icgsRequestDto.IsPartialRefused,
                    ReasonForPartialRefused = icgsRequestDto.ReasonForPartialRefused,
                });
        }

        private async Task UpdateRangeIcgsRequest(Request request, IcgsDto[] icgsRequestDtos)
        {
            var isNotNeedUpdateIcgsRequest = (request == null || icgsRequestDtos == null || !icgsRequestDtos.Any() || !request.ICGSRequests.Any());
            if (isNotNeedUpdateIcgsRequest)
            {
                return;
            }

            request.ICGSRequests
                .ToList()
                .ForEach(ir =>
                {
                    icgsRequestDtos = icgsRequestDtos
                        .Where(x => !x.Equals(_mapper.Map<IcgsDto>(ir)) && x.Id.HasValue)
                        .ToArray();
                });

            var updateIcgsRequests = ConvertToIcgsRequests(icgsRequestDtos, request.Id).ToList();
            await Executor.GetCommand<UpdateIcgsRequestRangeCommand>()
               .Process(c => c.ExecuteAsync(request.Id, updateIcgsRequests));
        }

        private async Task AddRangeIcgsRequest(Request request, IcgsDto[] icgsRequestDtos)
        {
            if (request == null || icgsRequestDtos == null || !icgsRequestDtos.Any())
            {
                return;
            }
            var newIcgsRequests = ConvertToIcgsRequests(icgsRequestDtos, request.Id).Where(x => x.Id == default(int)).ToList();
            await Executor.GetCommand<AddIcgsRequestRangeCommand>().Process(c => c.ExecuteAsync(0, newIcgsRequests));
        }


        private async Task DeleteRangeIcgsRequest(Request request, IcgsDto[] icgsRequestDtos)
        {

            if (request?.ICGSRequests == null || !request.ICGSRequests.Any())
            {
                return;
            }
            if (!icgsRequestDtos.Any())
            {
                await Executor.GetCommand<DeleteRangeIcgsRequestCommand>().Process(c => c.ExecuteAsync(request.ICGSRequests.ToList()));
            }
            else
            {
                var icgsRequestsForDelete = request.ICGSRequests
                    .Where(ir => !icgsRequestDtos.Select(x => x.Id).Contains(ir.Id))
                    .ToList();
                if (icgsRequestsForDelete.Any())
                {
                    await Executor.GetCommand<DeleteRangeIcgsRequestCommand>().Process(c => c.ExecuteAsync(icgsRequestsForDelete));
                }
            }
        }
    }
}