using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.BusinessLogic.Bibliographics;
using Iserv.Niis.BusinessLogic.Bibliographics.Changes;
using Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Icfem.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Icfem.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Icgs.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Icgs.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Icis.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Icis.Request;
using Iserv.Niis.BusinessLogic.Bibliographics.Ipc.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Ipc.Request;
using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.RequestCustomers;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Workflows.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework.Mappings.Dictionaries;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.EarlyReg;
using Iserv.Niis.Model.Models.Journal;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using Iserv.Niis.WorkflowServices;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Api.Controllers
{
    [Route("api/BibliographicData")]
    public class BibliographicDataController : BaseNiisApiController
    {
        private readonly ILogoUpdater _logoUpdater;

        public BibliographicDataController(ILogoUpdater logoUpdater)
        {
            _logoUpdater = logoUpdater;
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BibliographicDataDto dto)
        {
            switch (dto.OwnerType)
            {
                case Owner.Type.Request:
                    var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(dto.Id));
                    Mapper.Map(dto, request);
                    _logoUpdater.Update(request);
                    await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));
                    break;
            }

            return NoContent();
        }

        [HttpGet("{ownerType}/{ownerId}")]
        public async Task<IActionResult> Get(int ownerId, Owner.Type ownerType)
        {
            if (ownerType == Owner.Type.Request)
            {
                var requestTypeBibliographic = await Executor.GetQuery<GetRequestTypeBibliographicQuery>().Process(q => q.ExecuteAsync(ownerId));
                return Ok(requestTypeBibliographic);
            }

            if (ownerType == Owner.Type.ProtectionDoc)
            {
                var protectionDocTypeBibliographic = await Executor.GetQuery<GetProtectionDocTypeBibliographicQuery>().Process(q => q.ExecuteAsync(ownerId));
                return Ok(protectionDocTypeBibliographic);
            }
            //Биб. данные есть только у заявок и ОД
            throw new NotImplementedException();
        }

        [HttpGet("colorTzs/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetColorTzs(int ownerId, Owner.Type ownerType)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestColorTzs = await Executor.GetQuery<GetColorTzsByRequestIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    return Ok(requestColorTzs);
                case Owner.Type.ProtectionDoc:
                    var protectionDocColorTzs = await Executor.GetQuery<GetColorTzsByProtectionDocIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    return Ok(protectionDocColorTzs);
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost("colorTzs/{ownerType}/{ownerId}")]
        public async Task<IActionResult> AddColorTzs(int ownerId, Owner.Type ownerType,
            [FromBody] List<int> colorTzList)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    await Executor.GetCommand<AddColorTzRequestRelationCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, colorTzList));
                    break;
                case Owner.Type.ProtectionDoc:
                    await Executor.GetCommand<AddColorTzProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, colorTzList));
                    break;
                default:
                    throw new NotImplementedException();
            }


            return NoContent();
        }

        [HttpPut("colorTzs/{ownerType}/{ownerId}")]
        public async Task<IActionResult> ChangeColorTzsCollection(int ownerId, Owner.Type ownerType,
            [FromBody] List<int> colorTzList)
        {
            List<int> addIds;
            List<int> removeIds;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(ownerId));

                    addIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h =>
                        h.Execute(colorTzList, request.ColorTzs.Select(c => c.ColorTzId).ToList()));
                    removeIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h =>
                        h.Execute(colorTzList, request.ColorTzs.Select(c => c.ColorTzId).ToList()));

                    await Executor.GetCommand<AddColorTzRequestRelationCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, addIds));
                    var removeColorTzRequestRelations = await Executor.GetQuery<GetColorTzsByRequestIdAndColorIdsQuery>()
                        .Process(c => c.ExecuteAsync(request.Id, removeIds));
                    await Executor.GetCommand<DeleteRangeColorTzRequestRelationCommand>()
                        .Process(c => c.ExecuteAsync(removeColorTzRequestRelations));
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));

                    addIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h =>
                        h.Execute(colorTzList, protectionDoc.ColorTzs.Select(c => c.ColorTzId).ToList()));
                    removeIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h =>
                        h.Execute(colorTzList, protectionDoc.ColorTzs.Select(c => c.ColorTzId).ToList()));

                    await Executor.GetCommand<AddColorTzProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, addIds));
                    await Executor.GetCommand<RemoveColorTzProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, removeIds));
                    break;
                default:
                    throw new NotImplementedException();
            }
            return NoContent();
        }

        [HttpGet("conventionInfos/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetConventionInfos(int ownerId, Owner.Type ownerType)
        {
            List<ConventionInfoDto> conventionInfoDtos;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestConventionInfos = await Executor.GetQuery<GetConventionInfosByRequestIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    conventionInfoDtos =
                        Mapper.Map<List<RequestConventionInfo>, List<ConventionInfoDto>>(
                            requestConventionInfos);
                    return Ok(conventionInfoDtos);
                case Owner.Type.ProtectionDoc:
                    var protectionDocConventionInfos = await Executor.GetQuery<GetConventionInfosByProtectionDocIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    conventionInfoDtos =
                        Mapper.Map<List<ProtectionDocConventionInfo>, List<ConventionInfoDto>>(
                            protectionDocConventionInfos);
                    return Ok(conventionInfoDtos);
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost("conventionInfos/{ownerType}/{ownerId}")]
        public async Task<IActionResult> AddConventionInfos(int ownerId, Owner.Type ownerType,
            [FromBody] List<ConventionInfoDto> conventionInfoDtos)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestConventionInfos = Mapper.Map<List<ConventionInfoDto>, List<RequestConventionInfo>>(conventionInfoDtos);
                    await Executor.GetCommand<AddRequestConventionInfosRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, requestConventionInfos));
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDocConventionInfos = Mapper.Map<List<ConventionInfoDto>, List<ProtectionDocConventionInfo>>(conventionInfoDtos);
                    await Executor.GetCommand<AddProtectionDocConventionInfosRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, protectionDocConventionInfos));
                    break;
                default:
                    throw new NotImplementedException();
            }


            return NoContent();
        }

        [HttpPut("conventionInfos/{ownerType}/{ownerId}")]
        public async Task<IActionResult> ChangeConventionInfoCollection(int ownerId, Owner.Type ownerType,
            [FromBody] List<ConventionInfoDto> conventionInfoDtos)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    //TODO! реализовать для заявок
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));

                    var addIds = Executor.GetHandler<FilterAddEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(conventionInfoDtos.Select(ci => ci.Id ?? default(int)).ToList(),
                            protectionDoc.ProtectionDocConventionInfos.ToList()));
                    var removeIds = Executor.GetHandler<FilterRemoveEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(conventionInfoDtos.Select(ci => ci.Id ?? default(int)).ToList(),
                            protectionDoc.ProtectionDocConventionInfos.ToList()));
                    var updateIds = Executor.GetHandler<FilterUpdateEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(conventionInfoDtos.Select(ci => ci.Id ?? default(int)).ToList(),
                            protectionDoc.ProtectionDocConventionInfos.ToList()));

                    var protectionDocConventionInfos =
                        Mapper.Map<List<ConventionInfoDto>, List<ProtectionDocConventionInfo>>(conventionInfoDtos);
                    await Executor.GetCommand<AddProtectionDocConventionInfosRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId,
                            protectionDocConventionInfos.Where(pdci => addIds.Contains(pdci.Id)).ToList()));
                    await Executor.GetCommand<RemoveProtectionDocConventionInfosRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId,
                            protectionDocConventionInfos.Where(pdci => removeIds.Contains(pdci.Id)).ToList()));
                    await Executor.GetCommand<UpdateProtectionDocConventionInfosRangeCommand>().Process(c =>
                        c.ExecuteAsync(ownerId,
                            protectionDocConventionInfos.Where(pdci => updateIds.Contains(pdci.Id)).ToList()));
                    break;
                default:
                    throw new NotImplementedException();
            }
            return NoContent();
        }

        [HttpGet("earlyRegs/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetEarlyRegs(int ownerId, Owner.Type ownerType)
        {
            List<RequestEarlyRegDto> earlyRegDtos;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestEarlyRegs = await Executor.GetQuery<GetEarlyRegsByRequestIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    earlyRegDtos =
                        Mapper.Map<List<RequestEarlyReg>, List<RequestEarlyRegDto>>(
                            requestEarlyRegs);
                    return Ok(earlyRegDtos);
                case Owner.Type.ProtectionDoc:
                    var protectionDocEarlyRegs = await Executor.GetQuery<GetEarlyRegsByProtectionDocIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    earlyRegDtos =
                        Mapper.Map<List<ProtectionDocEarlyReg>, List<RequestEarlyRegDto>>(
                            protectionDocEarlyRegs);
                    return Ok(earlyRegDtos);
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost("earlyRegs/{ownerType}/{ownerId}")]
        public async Task<IActionResult> AddEarlyRegs(int ownerId, Owner.Type ownerType,
            [FromBody] List<RequestEarlyRegDto> earlyRegDtos)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestEarlyRegs = Mapper.Map<List<RequestEarlyRegDto>, List<RequestEarlyReg>>(earlyRegDtos);
                    await Executor.GetCommand<AddRequestEarlyRegsRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, requestEarlyRegs));
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDocEarlyRegs = Mapper.Map<List<RequestEarlyRegDto>, List<ProtectionDocEarlyReg>>(earlyRegDtos);
                    await Executor.GetCommand<AddProtectionDocEarlyRegsRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, protectionDocEarlyRegs));
                    break;
                default:
                    throw new NotImplementedException();
            }


            return NoContent();
        }

        [HttpPut("earlyRegs/{ownerType}/{ownerId}")]
        public async Task<IActionResult> ChangeEarlyRegCollection(int ownerId, Owner.Type ownerType,
            [FromBody] List<RequestEarlyRegDto> earlyRegDtos)
        {
            List<int> addIds;
            List<int> removeIds;
            List<int> updateIds;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(ownerId));

                    addIds = Executor.GetHandler<FilterAddEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(earlyRegDtos.Select(ci => ci.Id ?? default(int)).ToList(),
                            request.EarlyRegs.ToList()));
                    removeIds = Executor.GetHandler<FilterRemoveEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(earlyRegDtos.Select(ci => ci.Id ?? default(int)).ToList(),
                            request.EarlyRegs.ToList()));
                    updateIds = Executor.GetHandler<FilterUpdateEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(earlyRegDtos.Select(ci => ci.Id ?? default(int)).ToList(),
                            request.EarlyRegs.ToList()));

                    var requestEarlyRegs = Mapper.Map<List<RequestEarlyRegDto>, List<RequestEarlyReg>>(earlyRegDtos);
                    await Executor.GetCommand<AddRequestEarlyRegsRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId,
                            requestEarlyRegs.Where(pdci => addIds.Contains(pdci.Id)).ToList()));
                    await Executor.GetCommand<DeleteRangeRequestEarlyRegsCommand>()
                        .Process(c =>
                            c.ExecuteAsync(request.EarlyRegs.Where(er => removeIds.Contains(er.Id)).ToList()));
                    await Executor.GetCommand<UpdateRequestEarlyRegsRangeCommand>().Process(c =>
                        c.ExecuteAsync(ownerId,
                            requestEarlyRegs.Where(pdci => updateIds.Contains(pdci.Id)).ToList()));
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));

                    addIds = Executor.GetHandler<FilterAddEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(earlyRegDtos.Select(ci => ci.Id ?? default(int)).ToList(),
                            protectionDoc.EarlyRegs.ToList()));
                    removeIds = Executor.GetHandler<FilterRemoveEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(earlyRegDtos.Select(ci => ci.Id ?? default(int)).ToList(),
                            protectionDoc.EarlyRegs.ToList()));
                    updateIds = Executor.GetHandler<FilterUpdateEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(earlyRegDtos.Select(ci => ci.Id ?? default(int)).ToList(),
                            protectionDoc.EarlyRegs.ToList()));

                    var protectionDocEarlyRegs =
                        Mapper.Map<List<RequestEarlyRegDto>, List<ProtectionDocEarlyReg>>(earlyRegDtos);
                    await Executor.GetCommand<AddProtectionDocEarlyRegsRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId,
                            protectionDocEarlyRegs.Where(pdci => addIds.Contains(pdci.Id)).ToList()));
                    await Executor.GetCommand<RemoveProtectionDocEarlyRegsRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId,
                            protectionDocEarlyRegs.Where(pdci => removeIds.Contains(pdci.Id)).ToList()));
                    await Executor.GetCommand<UpdateProtectionDocEarlyRegsRangeCommand>().Process(c =>
                        c.ExecuteAsync(ownerId,
                            protectionDocEarlyRegs.Where(pdci => updateIds.Contains(pdci.Id)).ToList()));
                    break;
                default:
                    throw new NotImplementedException();
            }
            return NoContent();
        }

        [HttpGet("icfem/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetIcfem(int ownerId, Owner.Type ownerType)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var icfemRequestRelations = await Executor.GetQuery<GetIcfemByRequestIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    return Ok(icfemRequestRelations);
                case Owner.Type.ProtectionDoc:
                    var icfemProtectionDocRelations = await Executor.GetQuery<GetIcfemByProtectionDocIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    return Ok(icfemProtectionDocRelations);
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost("icfem/{ownerType}/{ownerId}")]
        public async Task<IActionResult> AddIcfem(int ownerId, Owner.Type ownerType,
            [FromBody] List<int> icfemList)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    await Executor.GetCommand<AddIcfemRequestRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, icfemList));
                    break;
                case Owner.Type.ProtectionDoc:
                    await Executor.GetCommand<AddIcfemProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, icfemList));
                    break;
                default:
                    throw new NotImplementedException();
            }


            return NoContent();
        }

        [HttpPut("icfem/{ownerType}/{ownerId}")]
        public async Task<IActionResult> ChangeIcfemCollection(int ownerId, Owner.Type ownerType,
            [FromBody] List<int> icfemList)
        {
            List<int> addIds;
            List<int> removeIds;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(ownerId));

                    addIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h =>
                        h.Execute(icfemList, request.Icfems.Select(c => c.DicIcfemId).ToList()));
                    removeIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h =>
                        h.Execute(icfemList, request.Icfems.Select(c => c.DicIcfemId).ToList()));

                    await Executor.GetCommand<AddIcfemRequestRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, addIds));
                    var removeIcfemRequest = await Executor.GetQuery<GetIcfemRequestsByRequestIdAndIcfemIdsQuery>()
                        .Process(q => q.ExeciuteAsync(request.Id, removeIds));
                    await Executor.GetCommand<DeleteRangeIcfemRequestCommand>()
                        .Process(c => c.ExecuteAsync(removeIcfemRequest));
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));

                    addIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h =>
                        h.Execute(icfemList, protectionDoc.Icfems.Select(c => c.DicIcfemId).ToList()));
                    removeIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h =>
                        h.Execute(icfemList, protectionDoc.Icfems.Select(c => c.DicIcfemId).ToList()));

                    await Executor.GetCommand<AddIcfemProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, addIds));
                    await Executor.GetCommand<RemoveIcfemProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, removeIds));
                    break;
                default:
                    throw new NotImplementedException();
            }
            return NoContent();
        }

        [HttpGet("icgs/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetIcgs(int ownerId, Owner.Type ownerType)
        {
            List<ICGSRequestItemDto> icgsItemDtos;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var icgsRequests = await Executor.GetQuery<GetIcgsByRequestIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    icgsItemDtos =
                        Mapper.Map<List<ICGSRequest>, List<ICGSRequestItemDto>>(
                            icgsRequests);
                    return Ok(icgsItemDtos);
                case Owner.Type.ProtectionDoc:
                    var icgsProtectionDocs = await Executor.GetQuery<GetIcgsByProtectionDocIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    icgsItemDtos =
                        Mapper.Map<List<ICGSProtectionDoc>, List<ICGSRequestItemDto>>(
                            icgsProtectionDocs);
                    return Ok(icgsItemDtos);
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost("icgs/{ownerType}/{ownerId}")]
        public async Task<IActionResult> AddIcgs(int ownerId, Owner.Type ownerType,
            [FromBody] List<ICGSRequestItemDto> icgsItemDtos)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var icgsRequests = Mapper.Map<List<ICGSRequestItemDto>, List<ICGSRequest>>(icgsItemDtos);
                    await Executor.GetCommand<AddIcgsRequestRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, icgsRequests));
                    break;
                case Owner.Type.ProtectionDoc:
                    var icgsProtectionDocs = Mapper.Map<List<ICGSRequestItemDto>, List<ICGSProtectionDoc>>(icgsItemDtos);
                    await Executor.GetCommand<AddIcgsProtectionDocRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, icgsProtectionDocs));
                    break;
                default:
                    throw new NotImplementedException();
            }


            return NoContent();
        }

        [HttpPut("icgs/{ownerType}/{ownerId}")]
        public async Task<IActionResult> ChangeIcgsCollection(int ownerId, Owner.Type ownerType,
            [FromBody] List<IcgsDto> icgsDtos)
        {
            List<int> addIds;
            List<int> removeIds;
            List<int> updateIds;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(ownerId));

                    addIds = Executor.GetHandler<FilterAddEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(icgsDtos.Select(ci => ci.Id ?? 0).ToList(),
                            request.ICGSRequests.ToList()));
                    removeIds = Executor.GetHandler<FilterRemoveEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(icgsDtos.Select(ci => ci.Id ?? 0).ToList(),
                            request.ICGSRequests.ToList()));
                    updateIds = Executor.GetHandler<FilterUpdateEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(icgsDtos.Select(ci => ci.Id ?? 0).ToList(),
                            request.ICGSRequests.ToList()));

                    var icgsRequests =
                        Mapper.Map<List<IcgsDto>, List<ICGSRequest>>(icgsDtos);
                    foreach (var icgsRequest in icgsRequests)
                    {
                        icgsRequest.RequestId = ownerId;
                    }
                    await Executor.GetCommand<AddIcgsRequestRangeCommand>().Process(c =>
                        c.ExecuteAsync(ownerId, icgsRequests.Where(pdci => addIds.Contains(pdci.Id)).ToList()));
                    await Executor.GetCommand<DeleteRangeIcgsRequestCommand>().Process(c =>
                        c.ExecuteAsync(request.ICGSRequests.Where(pdci => removeIds.Contains(pdci.Id)).ToList()));
                    await Executor.GetCommand<UpdateIcgsRequestRangeCommand>().Process(c =>
                        c.ExecuteAsync(ownerId, icgsRequests.Where(pdci => updateIds.Contains(pdci.Id)).ToList()));
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));

                    addIds = Executor.GetHandler<FilterAddEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(icgsDtos.Select(ci => ci.Id ?? 0).ToList(),
                            protectionDoc.IcgsProtectionDocs.ToList()));
                    removeIds = Executor.GetHandler<FilterRemoveEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(icgsDtos.Select(ci => ci.Id ?? 0).ToList(),
                            protectionDoc.IcgsProtectionDocs.ToList()));
                    updateIds = Executor.GetHandler<FilterUpdateEntityCollectionIdsHandler>().Process(h =>
                        h.Execute(icgsDtos.Select(ci => ci.Id ?? 0).ToList(),
                            protectionDoc.IcgsProtectionDocs.ToList()));

                    var icgsProtectionDocs =
                        Mapper.Map<List<IcgsDto>, List<ICGSProtectionDoc>>(icgsDtos);
                    await Executor.GetCommand<AddIcgsProtectionDocRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId,
                            icgsProtectionDocs.Where(pdci => addIds.Contains(pdci.Id)).ToList()));
                    await Executor.GetCommand<RemoveIcgsProtectionDocRangeCommand>()
                        .Process(c => c.ExecuteAsync(ownerId,
                            icgsProtectionDocs.Where(pdci => removeIds.Contains(pdci.Id)).ToList()));
                    await Executor.GetCommand<UpdateIcgsProtectionDocRangeCommand>().Process(c =>
                        c.ExecuteAsync(ownerId,
                            icgsProtectionDocs.Where(pdci => updateIds.Contains(pdci.Id)).ToList()));
                    break;
                default:
                    throw new NotImplementedException();
            }
            return NoContent();
        }

        [HttpGet("icis/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetIcis(int ownerId, Owner.Type ownerType)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var icisRequests = await Executor.GetQuery<GetIcisByRequestIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    return Ok(icisRequests);
                case Owner.Type.ProtectionDoc:
                    var icisProtectionDocs = await Executor.GetQuery<GetIcisByProtectionDocIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    return Ok(icisProtectionDocs);
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost("icis/{ownerType}/{ownerId}")]
        public async Task<IActionResult> AddIcis(int ownerId, Owner.Type ownerType,
            [FromBody] List<int> icisList)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    await Executor.GetCommand<AddIcisRequestRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, icisList));
                    break;
                case Owner.Type.ProtectionDoc:
                    await Executor.GetCommand<AddIcisProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, icisList));
                    break;
                default:
                    throw new NotImplementedException();
            }


            return NoContent();
        }

        [HttpPut("icis/{ownerType}/{ownerId}")]
        public async Task<IActionResult> ChangeIcisCollection(int ownerId, Owner.Type ownerType,
            [FromBody] List<int> icisList)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    //TODO! реализовать для заявок
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));

                    var addIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h =>
                        h.Execute(icisList, protectionDoc.ColorTzs.Select(c => c.ColorTzId).ToList()));
                    var removeIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h =>
                        h.Execute(icisList, protectionDoc.ColorTzs.Select(c => c.ColorTzId).ToList()));

                    await Executor.GetCommand<AddIcisProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, addIds));
                    await Executor.GetCommand<RemoveIcisProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, removeIds));
                    break;
                default:
                    throw new NotImplementedException();
            }
            return NoContent();
        }

        [HttpGet("ipc/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetIpc(int ownerId, Owner.Type ownerType)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var ipcRequests = await Executor.GetQuery<GetIpcByRequestIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    return Ok(ipcRequests);
                case Owner.Type.ProtectionDoc:
                    var ipcProtectionDocs = await Executor.GetQuery<GetIpcByProtectionDocIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    return Ok(ipcProtectionDocs);
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost("ipc/{ownerType}/{ownerId}")]
        public async Task<IActionResult> AddIpc(int ownerId, Owner.Type ownerType,
            [FromBody] List<int> ipcList)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    await Executor.GetCommand<AddIpcRequestRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, ipcList));
                    break;
                case Owner.Type.ProtectionDoc:
                    await Executor.GetCommand<AddIpcProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, ipcList));
                    break;
                default:
                    throw new NotImplementedException();
            }


            return NoContent();
        }

        [HttpPut("ipc/{ownerType}/{ownerId}")]
        public async Task<IActionResult> ChangeIpcCollection(int ownerId, Owner.Type ownerType,
            [FromBody] List<int> ipcList)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    //TODO! реализовать для заявок
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));

                    var addIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h =>
                        h.Execute(ipcList, protectionDoc.ColorTzs.Select(c => c.ColorTzId).ToList()));
                    var removeIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h =>
                        h.Execute(ipcList, protectionDoc.ColorTzs.Select(c => c.ColorTzId).ToList()));

                    await Executor.GetCommand<AddIpcProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, addIds));
                    await Executor.GetCommand<RemoveIpcProtectionDocRelationsCommand>()
                        .Process(c => c.ExecuteAsync(ownerId, removeIds));
                    break;
                default:
                    throw new NotImplementedException();
            }
            return NoContent();
        }

        [HttpPut("change")]
        public async Task<IActionResult> ChangeObject([FromBody] ChangesDto[] changes)
        {
            foreach (var change in changes)
            {
                switch (change.OwnerType)
                {
                    case Owner.Type.Request:
                        var request = await Executor.GetQuery<GetRequestByIdQuery>()
                            .Process(q => q.ExecuteAsync(change.Id));
                        RequestCustomer requestCustomer;
                        switch (change.ChangeType)
                        {
                            case ChangeType.AddresseeAddress:
                                requestCustomer = request.RequestCustomers.FirstOrDefault(rc =>
                                    rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);
                                if (requestCustomer != null)
                                {
                                    requestCustomer.Address = change.NewValue;
                                    await Executor.GetCommand<UpdateRequestCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer));
                                    requestCustomer.Customer.Address = change.NewValue;
                                    await Executor.GetCommand<UpdateDicCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer.Customer));
                                }
                                break;
                            case ChangeType.DeclarantAddress:
                                requestCustomer = request.RequestCustomers.FirstOrDefault(rc =>
                                    rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant);
                                if (requestCustomer != null)
                                {
                                    requestCustomer.Address = change.NewValue;
                                    await Executor.GetCommand<UpdateRequestCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer));
                                    requestCustomer.Customer.Address = change.NewValue;
                                    await Executor.GetCommand<UpdateDicCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer.Customer));
                                }
                                break;
                            case ChangeType.DeclarantName:
                                requestCustomer = request.RequestCustomers.FirstOrDefault(rc =>
                                    rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant);
                                if (requestCustomer != null)
                                {
                                    requestCustomer.Customer.NameRu = change.NewValue;
                                    await Executor.GetCommand<UpdateDicCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer.Customer));
                                }
                                break;
                            case ChangeType.AddresseeAddressEn:
                                requestCustomer = request.RequestCustomers.FirstOrDefault(rc =>
                                    rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);
                                if (requestCustomer != null)
                                {
                                    requestCustomer.AddressEn = change.NewValue;
                                    await Executor.GetCommand<UpdateRequestCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer));
                                    requestCustomer.Customer.AddressEn = change.NewValue;
                                    await Executor.GetCommand<UpdateDicCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer.Customer));
                                }
                                break;
                            case ChangeType.DeclarantAddressEn:
                                requestCustomer = request.RequestCustomers.FirstOrDefault(rc =>
                                    rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant);
                                if (requestCustomer != null)
                                {
                                    requestCustomer.AddressEn = change.NewValue;
                                    await Executor.GetCommand<UpdateRequestCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer));
                                    requestCustomer.Customer.AddressEn = change.NewValue;
                                    await Executor.GetCommand<UpdateDicCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer.Customer));
                                }
                                break;
                            case ChangeType.DeclarantNameEn:
                                requestCustomer = request.RequestCustomers.FirstOrDefault(rc =>
                                    rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant);
                                if (requestCustomer != null)
                                {
                                    requestCustomer.Customer.NameEn = change.NewValue;
                                    await Executor.GetCommand<UpdateDicCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer.Customer));
                                }
                                break;
                            case ChangeType.AddresseeAddressKz:
                                requestCustomer = request.RequestCustomers.FirstOrDefault(rc =>
                                    rc.CustomerRole.Code == DicCustomerRoleCodes.Correspondence);
                                if (requestCustomer != null)
                                {
                                    requestCustomer.AddressKz = change.NewValue;
                                    await Executor.GetCommand<UpdateRequestCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer));
                                    requestCustomer.Customer.AddressKz = change.NewValue;
                                    await Executor.GetCommand<UpdateDicCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer.Customer));
                                }
                                break;
                            case ChangeType.DeclarantAddressKz:
                                requestCustomer = request.RequestCustomers.FirstOrDefault(rc =>
                                    rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant);
                                if (requestCustomer != null)
                                {
                                    requestCustomer.AddressKz = change.NewValue;
                                    await Executor.GetCommand<UpdateRequestCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer));
                                    requestCustomer.Customer.AddressKz = change.NewValue;
                                    await Executor.GetCommand<UpdateDicCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer.Customer));
                                }
                                break;
                            case ChangeType.DeclarantNameKz:
                                requestCustomer = request.RequestCustomers.FirstOrDefault(rc =>
                                    rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant);
                                if (requestCustomer != null)
                                {
                                    requestCustomer.Customer.NameKz = change.NewValue;
                                    await Executor.GetCommand<UpdateDicCustomerCommand>()
                                        .Process(c => c.ExecuteAsync(requestCustomer.Customer));
                                }
                                break;
                        }
                        break;
                }
            }

            return NoContent();
        }

        [HttpGet("changeTypeOptions")]
        public async Task<IActionResult> GetChangeTypeOptions()
        {
            Owner.Type ownerType;
            int ownerId;
            if (Request.Query.ContainsKey("ownerType"))
            {
                if (Enum.TryParse(Request.Query["ownerType"], out ownerType) == false)
                {
                    throw new InvalidCastException("В веб-запросе некорректное значение ownerType!!!");
                }
            }
            else
            {
                throw new ArgumentNullException($"В веб-запросе отсутсвует параметр ownerType!!!");
            }
            if (Request.Query.ContainsKey("ownerId"))
            {
                if (int.TryParse(Request.Query["ownerId"], out ownerId) == false)
                {
                    throw new InvalidCastException("В веб-запросе некорректное значение ownerId!!!");
                }
            }
            else
            {
                throw new ArgumentNullException($"В веб-запросе отсутсвует параметр ownerId!!!");
            }
            var result = Executor.GetQuery<GetChangeTypeOptionsQuery>().Process(q => q.Execute());
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestWorkflows = await Executor.GetQuery<GetRequestWorkflowsByOwnerIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    result = result
                        .Where(r => !requestWorkflows.Select(rw => rw.CurrentStage.Code).Contains(r.StageCode)).ToList();
                    break;
            }
            return Ok(result);
        }

        [HttpPost("changeworkflow/{ownerType}")]
        public async Task<IActionResult> ChangeWorkflow(Owner.Type ownerType, [FromBody] int ownerId)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var workflows = await Executor.GetQuery<GetRequestWorkflowsByOwnerIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    var lastStageBeforeScenario = workflows.OrderBy(w => w.DateCreate)
                        .LastOrDefault(w => w.IsChangeScenarioEntry == true);
                    var requestWorkFlowRequest = new RequestWorkFlowRequest
                    {
                        RequestId = ownerId,
                        NextStageUserId = lastStageBeforeScenario?.CurrentUserId ?? NiisAmbientContext.Current.User.Identity.UserId
                    };

                    NiisWorkflowAmbientContext.Current.RequestWorkflowService.Process(requestWorkFlowRequest);
                    NiisWorkflowAmbientContext.Current.RequestWorkflowService.Process(requestWorkFlowRequest);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return NoContent();
        }
    }
}