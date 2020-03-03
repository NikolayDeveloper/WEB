using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.EarlyReg;
using Iserv.Niis.Model.Models.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Request
{
    public class Update
    {
        public class Command : IRequest<RequestDetailDto>
        {
            public Command(int requestId, RequestDetailDto requestDetailDto)
            {
                RequestId = requestId;
                RequestDetailDto = requestDetailDto;
            }

            public int RequestId { get; }
            public RequestDetailDto RequestDetailDto { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, RequestDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly ICustomerUpdater _customerUpdater;
            private readonly ILogoUpdater _logoUpdater;
            private readonly IMapper _mapper;

            private const string EarlyRegTypePriorityDataCode = "30 - 300";
            public CommandHandler(NiisWebContext context, IMapper mapper, ICustomerUpdater customerUpdater,
                ILogoUpdater logoUpdater)
            {
                _context = context;
                _mapper = mapper;
                _customerUpdater = customerUpdater;
                _logoUpdater = logoUpdater;
            }

            public async Task<RequestDetailDto> Handle(Command message)
            {
                var requestId = message.RequestDetailDto.Id = message.RequestId;
                var requestDto = message.RequestDetailDto;

                var request = await _context.Requests
                    .Include(r => r.RequestInfo)
                    .Include(r => r.ICGSRequests)
                    .Include(r => r.ICISRequests)
                    .Include(r => r.ColorTzs)
                    .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                    .Include(r => r.Icfems)
                    .Include(r => r.RequestType)
                    .Include(r => r.RequestConventionInfos)
                    .SingleOrDefaultAsync(r => r.Id == requestId);
                if (request == null)
                {
                    throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                        DataNotFoundException.OperationType.Update, requestId);
                }

                _mapper.Map(requestDto, request);
                ApplyIcgsRequest(request, requestDto.IcgsRequestDtos);
                ApplyIcisIds(request, requestDto.IcisRequestIds);
                ApplyColorIds(request, requestDto.ColorTzIds);
                ApplyRequestInfo(requestDto, request);
                ApplyRequestEarlyRegDtos(request, requestDto.RequestEarlyRegDtos);
                ApplyIcfemIds(request, requestDto.IcfemIds);
                ApplyIpcIds(request, requestDto.IpcIds);
                ApplyRequestConventionInfo(request, requestDto.RequestConventionInfoDtos);
                _logoUpdater.Update(request);

                try
                {
                    await _context.SaveChangesAsync();

                    var requestWithIncludes = await _context.Requests
                        .Include(r => r.RequestInfo).ThenInclude(i => i.BreedCountry)
                        .Include(r => r.ProtectionDocType)
                        .Include(r => r.RequestCustomers).ThenInclude(r => r.CustomerRole)
                        .Include(r => r.RequestCustomers).ThenInclude(r => r.Customer).ThenInclude(c => c.Type)
                        .Include(r => r.RequestCustomers).ThenInclude(r => r.Customer).ThenInclude(c => c.Country)
                        .Include(r => r.Addressee)
                        .Include(r => r.Workflows).ThenInclude(r => r.FromStage)
                        .Include(r => r.Workflows).ThenInclude(r => r.CurrentStage)
                        .Include(r => r.Workflows).ThenInclude(r => r.FromUser)
                        .Include(r => r.Workflows).ThenInclude(r => r.CurrentUser)
                        .Include(r => r.Workflows).ThenInclude(r => r.Route)
                        .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                        .Include(r => r.Department)
                        .SingleAsync(r => r.Id == requestId);
                    return _mapper.Map<Domain.Entities.Request.Request, RequestDetailDto>(requestWithIncludes);
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
            }

            private void ApplyColorIds(Domain.Entities.Request.Request request, int[] colorTzIds)
            {
                request.ColorTzs
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
                            _context.DicColorTzRequestRelations.Remove(ir);
                        }
                    });

                //Add new rows
                var colorTzRequestRelations = colorTzIds.Select(id =>
                    new DicColorTZRequestRelation {RequestId = request.Id, ColorTzId = id});
                foreach (var colorTzRequestRelation in colorTzRequestRelations)
                {
                    request.ColorTzs.Add(colorTzRequestRelation);
                }
            }

            private void ApplyRequestInfo(RequestDetailDto requestDto, Domain.Entities.Request.Request request)
            {
                var requestInfo = _mapper.Map<RequestDetailDto, RequestInfo>(requestDto);
                if (requestInfo == null)
                {
                    return;
                }

                request.RequestInfo = requestInfo;
            }

            private void ApplyIcgsRequest(Domain.Entities.Request.Request request, IcgsDto[] icgsRequestDtos)
            {
                DeleteRangeIcgsRequest(request, icgsRequestDtos);
                AddRangeIcgsRequest(request, icgsRequestDtos);
                UpdateRangeIcgsRequest(request, icgsRequestDtos);
            }

            private void AddRangeIcgsRequest(Domain.Entities.Request.Request request, IcgsDto[] icgsRequestDtos)
            {
                if (request == null || icgsRequestDtos==null || !icgsRequestDtos.Any())
                {
                    return;
                }
                var newIcgsRequests = ConvertToIcgsRequests(icgsRequestDtos, request.Id).Where(x => x.Id == default(int));
                foreach (var icgsRequest in newIcgsRequests)
                {
                    request.ICGSRequests.Add(icgsRequest);
                }
            }

            private void UpdateRangeIcgsRequest(Domain.Entities.Request.Request request, IcgsDto[] icgsRequestDtos)
            {
                if (request == null || icgsRequestDtos == null || !icgsRequestDtos.Any())
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

                var updateIcgsRequests = ConvertToIcgsRequests(icgsRequestDtos, request.Id);
                foreach (var icgsRequest in updateIcgsRequests)
                {
                    var originIcgs =  request.ICGSRequests.First(x => x.Id == icgsRequest.Id);
                    originIcgs.ClaimedDescription = icgsRequest.ClaimedDescription;
                    originIcgs.Description = icgsRequest.Description;
                    originIcgs.DescriptionKz = icgsRequest.DescriptionKz;
                    originIcgs.NegativeDescription = icgsRequest.NegativeDescription;
                    originIcgs.IcgsId = icgsRequest.IcgsId;
                }
            }
            private void DeleteRangeIcgsRequest(Domain.Entities.Request.Request request, IcgsDto[] icgsRequestDtos)
            {
                if (request?.ICGSRequests == null || !request.ICGSRequests.Any())
                {
                    return;
                }
                if (!icgsRequestDtos.Any())
                {
                    _context.ICGSRequests.RemoveRange(request.ICGSRequests);

                }
                else
                {
                    request.ICGSRequests
                        .ToList()
                        .ForEach(ir =>
                        {
                            if (!icgsRequestDtos.Select(x => x.Id).Contains(ir.Id))
                            {
                                _context.ICGSRequests.Remove(ir);
                            }
                        });
                }
             
            }

            private void ApplyRequestEarlyRegDtos(Domain.Entities.Request.Request request,
                RequestEarlyRegDto[] requestEarlyRegDtos)
            {
                DeleteRequestEarlyRegDtos(request, requestEarlyRegDtos);
                AddRequestEarlyRegDtos(request, requestEarlyRegDtos);
                UpdateRequestEarlyRegDtos(request, requestEarlyRegDtos);
            }

            private void DeleteRequestEarlyRegDtos(Domain.Entities.Request.Request request,
                RequestEarlyRegDto[] requestEarlyRegDtos)
            {
                if (request?.EarlyRegs == null || !request.EarlyRegs.Any())
                {
                    return;
                }

                if (!requestEarlyRegDtos.Any())
                {
                    _context.RequestEarlyRegs.RemoveRange(request.EarlyRegs);
                }
                else
                {
                    request.EarlyRegs
                        .ToList()
                        .ForEach(r =>
                        {
                            if (!requestEarlyRegDtos.Select(x => x.Id).Contains(r.Id))
                            {
                                _context.RequestEarlyRegs.Remove(r);
                            }
                        });
                }
            }
            private void AddRequestEarlyRegDtos(Domain.Entities.Request.Request request,
                RequestEarlyRegDto[] requestEarlyRegDtos)
            {
                if (request == null || requestEarlyRegDtos == null || !requestEarlyRegDtos.Any())
                {
                    return;
                }
                var earlyRegTypePriorityDataId = _context.DicEarlyRegTypes
                    .First(x=>x.Code.Equals(EarlyRegTypePriorityDataCode)).Id; // TODO временно выбираем только Приоритетные данные
                var newrequestEarlyRegDtos = requestEarlyRegDtos.Where(x => !x.Id.HasValue);
                var newRequestEarlyRegs =
                    _mapper.Map<IEnumerable<RequestEarlyReg>>(newrequestEarlyRegDtos);
                foreach (var requestEarlyReg in newRequestEarlyRegs)
                {
                    requestEarlyReg.RequestId = request.Id;
                    requestEarlyReg.EarlyRegTypeId = earlyRegTypePriorityDataId;
                    request.EarlyRegs.Add(requestEarlyReg);
                }
            }

            private void UpdateRequestEarlyRegDtos(Domain.Entities.Request.Request request,
                RequestEarlyRegDto[] requestEarlyRegDtos)
            {
                if (request == null || requestEarlyRegDtos == null || !requestEarlyRegDtos.Any())
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
                var updateRequestEarlyRegs = _mapper.Map<IEnumerable<RequestEarlyReg>>(requestEarlyRegDtos);
                foreach (var requestEarlyReg in updateRequestEarlyRegs)
                {
                    var originRequestEarlyReg = request.EarlyRegs.First(x => x.Id == requestEarlyReg.Id);
                    originRequestEarlyReg.RegCountryId = requestEarlyReg.RegCountryId;
                    originRequestEarlyReg.RegNumber = requestEarlyReg.RegNumber;
                    originRequestEarlyReg.RegDate = requestEarlyReg.RegDate;
                    originRequestEarlyReg.EarlyRegTypeId = requestEarlyReg.EarlyRegTypeId;
                }
            }


            private void ApplyIcisIds(Domain.Entities.Request.Request request, int[] icisIds)
            {
                request.ICISRequests
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
                            _context.ICISRequests.Remove(ir);
                        }
                    });

                //Add new rows
                var newIcisRequests = icisIds.Select(id => new ICISRequest {RequestId = request.Id, IcisId = id});
                foreach (var icisRequest in newIcisRequests)
                {
                    request.ICISRequests.Add(icisRequest);
                }
            }

            private void ApplyIcfemIds(Domain.Entities.Request.Request request, int[] icfemIds)
            {
                request.Icfems
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
                            _context.DicIcfemRequestRelations.Remove(ir);
                        }
                    });

                //Add new rows
                var newIcfems = icfemIds.Select(id => new DicIcfemRequestRelation { RequestId = request.Id, DicIcfemId = id });
                foreach (var newIcfem in newIcfems)
                {
                    request.Icfems.Add(newIcfem);
                }
            }

            private void ApplyIpcIds(Domain.Entities.Request.Request request, int[] ipcIds)
            {
                request.IPCRequests
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
                            _context.IPCRequests.Remove(ip);
                        }
                    });
                //Add new rows
                var newIpcs = ipcIds.Select(id => new IPCRequest {RequestId = request.Id, IpcId = id});
                foreach (var newIpc in newIpcs)
                {
                    request.IPCRequests.Add(newIpc);
                }
            }

            private void ApplyRequestConventionInfo(Domain.Entities.Request.Request request,
                ConventionInfoDto[] requestConventionInfoDtos)
            {
                DeleteRequestConventionInfo(request, requestConventionInfoDtos);
                AddRangeRequestConventionInfo(request, requestConventionInfoDtos);
                UpdateRequestConventionInfo(request, requestConventionInfoDtos);
            }

            private void AddRangeRequestConventionInfo(Domain.Entities.Request.Request request,
                ConventionInfoDto[] requestConventionInfoDtos)
            {
                if (request == null || requestConventionInfoDtos == null || !requestConventionInfoDtos.Any())
                {
                    return;
                }

                var newRequestConventionInfoDtos =
                    _mapper.Map<IEnumerable<RequestConventionInfo>>(
                        requestConventionInfoDtos.Where(r => !r.Id.HasValue));
                foreach (var item in newRequestConventionInfoDtos)
                {
                    request.RequestConventionInfos.Add(item);
                }
            }

            private void UpdateRequestConventionInfo(Domain.Entities.Request.Request request,
                ConventionInfoDto[] requestConventionInfoDtos)
            {
                if (request == null || requestConventionInfoDtos == null || !requestConventionInfoDtos.Any())
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
                var updateRequestConventionInfos =
                    _mapper.Map<IEnumerable<RequestConventionInfo>>(requestConventionInfoDtos);
                foreach (var itemConventionInfo in updateRequestConventionInfos)
                {
                    var originConventionInfo =
                        request.RequestConventionInfos.First(rc => rc.Id == itemConventionInfo.Id);
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

            private void DeleteRequestConventionInfo(Domain.Entities.Request.Request request,
                ConventionInfoDto[] requestConventionInfoDtos)
            {
                if (request?.RequestConventionInfos == null || !request.RequestConventionInfos.Any())
                {
                    return;
                }

                if (!requestConventionInfoDtos.Any())
                {
                    _context.RequestConventionInfos.RemoveRange(request.RequestConventionInfos);
                }
                else
                {
                    request.RequestConventionInfos
                        .ToList()
                        .ForEach(rc =>
                        {
                            if (!requestConventionInfoDtos.Select(x => x.Id).Contains(rc.Id))
                            {
                                _context.RequestConventionInfos.Remove(rc);
                            }
                        });
                }
            }
            private IEnumerable<Domain.Entities.Request.ICGSRequest> ConvertToIcgsRequests(IcgsDto[] icgsRequestDtos, int requestId)
            {
                return icgsRequestDtos
                    .Select(icgsRequestDto => new Domain.Entities.Request.ICGSRequest
                    {
                        Id = icgsRequestDto.Id ?? default(int),
                        RequestId = requestId,
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