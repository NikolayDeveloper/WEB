using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Icfem.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Icgs.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Icis.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bibliographics.Ipc.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Bulletin;
using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.ProtectionDoc;
using Iserv.Niis.Model.Models.Subject;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Iserv.Niis.WorkflowServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ProtectionDocs")]
    public class ProtectionDocsController : BaseNiisApiController
    {
        private readonly IDocumentGeneratorFactory _templateGeneratorFactory;
        private readonly IProtectionDocService _protectionDocService;

        public ProtectionDocsController(IDocumentGeneratorFactory templateGeneratorFactory, IProtectionDocService protectionDocService)
        {
            _templateGeneratorFactory = templateGeneratorFactory;
            _protectionDocService = protectionDocService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(id));

            if (protectionDoc.Workflows != null
                 && protectionDoc.Workflows.Any(x => x.CurrentStage != null && x.CurrentStage.Code == RouteStageCodes.ODParallel)
            )
            {
                ProtectionDocWorkflow userSpecificWorkflow = null;
                var userId = NiisAmbientContext.Current.User.Identity.UserId;
                if (Executor
                    .GetQuery<TryGetProtectionDocWorkflowFromParalleByOwnerIdCommand>()
                    .Process(q => q.Execute(protectionDoc.Id, userId, out userSpecificWorkflow))
                    && userSpecificWorkflow != null
                )
                {
                    protectionDoc.CurrentWorkflow = userSpecificWorkflow;
                    protectionDoc.CurrentWorkflowId = userSpecificWorkflow.Id;
                }

            }

            var protectionDocDetailDto = Mapper.Map<ProtectionDoc, ProtectionDocDetailsDto>(protectionDoc);

            if (protectionDoc.Addressee != null)
            {
                protectionDocDetailDto.Addressee = Mapper.Map<SubjectDto>(protectionDoc.Addressee);
            }

            return Ok(protectionDocDetailDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProtectionDocDetailsDto protectionDocDetailsDto)
        {
            var protectionDoc = Mapper.Map<ProtectionDoc>(protectionDocDetailsDto);
            await Executor.GetCommand<UpdateProtectionDocCommand>().Process(c => c.ExecuteAsync(id, protectionDoc));

            protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(id));
            await Executor.GetHandler<UpdateProtectionDocHandler>()
                .Process(h => h.Handle(protectionDoc, protectionDocDetailsDto));

            var updatedProtectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(id));
            var result = Mapper.Map<ProtectionDoc, ProtectionDocDetailsDto>(updatedProtectionDoc);

            return Ok(result);
        }

        [HttpPut("specialUpdate/{id}")]
        public async Task<IActionResult> SpecialUpdate(int id, [FromBody] KeyValuePair<string,object>[] specialValues)
        {
            await Executor.GetCommand<UpdateProtectionDocSpecialCommand>().Process(c => c.ExecuteAsync(id, specialValues));

            var updatedProtectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(id));

            if (updatedProtectionDoc.Workflows != null
                 && updatedProtectionDoc.Workflows.Any(x => x.CurrentStage != null && x.CurrentStage.Code == RouteStageCodes.ODParallel)
            )
            {
                ProtectionDocWorkflow userSpecificWorkflow = null;
                var userId = NiisAmbientContext.Current.User.Identity.UserId;
                if (Executor
                    .GetQuery<TryGetProtectionDocWorkflowFromParalleByOwnerIdCommand>()
                    .Process(q => q.Execute(updatedProtectionDoc.Id, userId, out userSpecificWorkflow))
                    && userSpecificWorkflow != null
                )
                {
                    updatedProtectionDoc.CurrentWorkflow = userSpecificWorkflow;
                    updatedProtectionDoc.CurrentWorkflowId = userSpecificWorkflow.Id;
                }

            }
            var result = Mapper.Map<ProtectionDoc, ProtectionDocDetailsDto>(updatedProtectionDoc);

            return Ok(result);
        }

        [HttpPost("workflow/{userId}/{bulletinUserId}/{supportUserId}/{bulletinId}/{nextUserForPrintId}/{nextUserForDescriptionsId}/{nextUserForMaintenanceId}")]
        public async Task<IActionResult> WorkflowCreateMultiple
            ([FromBody] int[] ids, int userId, int bulletinUserId, int supportUserId, int bulletinId, int nextUserForPrintId, int nextUserForDescriptionsId, int nextUserForMaintenanceId)
        {
            var isAllSelected = Convert.ToBoolean(Request.Query["isAllSelected"].ToString());
            var hasIpc = Convert.ToBoolean(Request.Query["hasIpc"].ToString());
            SelectionMode selectionMode;

            switch (Request.Query["selectionMode"].ToString())
            {
                case "0":
                    selectionMode = SelectionMode.Including;
                    break;
                case "1":
                    selectionMode = SelectionMode.Except;
                    break;
                default:
                    throw new NotImplementedException();
            }

            ids = await _protectionDocService.GenerateGosNumbers(ids, selectionMode, hasIpc, isAllSelected);

            foreach (var id in ids)
            {
                var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                .Process(q => q.ExecuteAsync(id));

                if (protectionDoc is null)
                {
                    throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, id);
                }              

                if (bulletinUserId != 0)
                {
                    protectionDoc.BulletinUserId = bulletinUserId;
                }
                if (supportUserId != 0)
                {
                    protectionDoc.SupportUserId = supportUserId;
                }

                await Executor.GetCommand<UpdateProtectionDocCommand>().Process(c => c.ExecuteAsync(id, protectionDoc));

                string stageCode = GetNextStageCode(protectionDoc);                         

                var relation = new ProtectionDocBulletinRelation
                {
                    BulletinId = bulletinId,
                    ProtectionDocId = id,
                    IsPublish = true
                };
                await Executor.GetCommand<CreateProtectionDocBulletinRelationCommand>()
                    .Process(c => c.ExecuteAsync(relation));

                if(stageCode != RouteStageCodes.ODParallel)
                    await CreatePatentOrCertificate(id, userId != 0 ? userId : nextUserForMaintenanceId);

                var protectionDocumentWorkFlowRequest = new ProtectionDocumentWorkFlowRequest
                {
                    ProtectionDocId = id,
                    NextStageCode = stageCode,
                    NextStageUserId = userId != 0 ? userId : nextUserForMaintenanceId,
                };
                if (nextUserForPrintId > 0)
                    protectionDocumentWorkFlowRequest.SpecificNextStageUserIds[RouteStageCodes.OD01_3] = nextUserForPrintId;
                if (nextUserForDescriptionsId > 0)
                    protectionDocumentWorkFlowRequest.SpecificNextStageUserIds[RouteStageCodes.OD01_2_2] = nextUserForDescriptionsId;
                if (nextUserForMaintenanceId > 0 && bulletinUserId == 0)
                    protectionDocumentWorkFlowRequest.SpecificNextStageUserIds[RouteStageCodes.OD03_1] = nextUserForMaintenanceId;
                if (bulletinUserId > 0 && nextUserForMaintenanceId == 0)
                    protectionDocumentWorkFlowRequest.SpecificNextStageUserIds[RouteStageCodes.OD01_6] = bulletinUserId;



                NiisWorkflowAmbientContext.Current.ProtectionDocumentWorkflowService.Process(protectionDocumentWorkFlowRequest);
            }

            return NoContent();
        }

        [HttpGet("canSplit/{id}")]
        public async Task<IActionResult> CanSplitProtectionDoc(int id)
        {
            var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(id));

            var canSplit = protectionDoc.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD04_4;

            return Ok(canSplit);
        }

        [HttpPut("authorCertificate/{id}")]
        public async Task<IActionResult> CreateAuthorsCertificate(int id, [FromBody] SubjectDto[] authors)
        {
            var userId = NiisAmbientContext.Current.User.Identity.UserId;

            await _protectionDocService.CreateAuthorsCertificate(id, authors.Select(a => a.Id).ToArray(), userId);
            return Ok(true);
        }

        [HttpPost("split/{id}")]
        public async Task<IActionResult> SplitProtectionDoc(int id, [FromBody] IcgsDto[] icgsDtos)
        {
            var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(id));
            var newProtectionDoc = Mapper.Map<ProtectionDoc>(protectionDoc);

            #region Создание охранного документа

            newProtectionDoc.TypeId = protectionDoc.TypeId;
            newProtectionDoc.SubTypeId = protectionDoc.SubTypeId;
            newProtectionDoc.DateCreate = DateTimeOffset.Now;
            newProtectionDoc.RequestId = protectionDoc.RequestId;
            newProtectionDoc.Transliteration = protectionDoc.Transliteration;
            await Executor.GetHandler<GenerateNumberForSplitProtectionDocsHandler>().Process(h => h.ExecuteAsync(protectionDoc, newProtectionDoc));
            Executor.GetHandler<GenerateBarcodeHandler>().Process(h => h.Execute(newProtectionDoc));
            var newProtectionDocId = await Executor.GetCommand<CreateProtectionDocCommand>().Process(c => c.ExecuteAsync(newProtectionDoc));

            if (newProtectionDoc.RequestId != null)
            {
                //Создаем связь между заявкой и охранным документом.
                var requestProtectionDocSimilar = new RequestProtectionDocSimilar
                {
                    RequestId = (int) newProtectionDoc.RequestId,
                    ProtectionDocId = newProtectionDoc.Id,
                    DateCreate = DateTimeOffset.Now
                };

                Executor.GetCommand<CreateRequestProtectionDocSimilarCommand>().Process(r => r.Execute(requestProtectionDocSimilar));
            }

            var newWorkflows = Mapper.Map<ProtectionDocWorkflow[]>(protectionDoc.Workflows).OrderBy(w => w.DateCreate);
            foreach (var newWorkflow in newWorkflows)
            {
                newWorkflow.OwnerId = newProtectionDocId;
                Executor.GetCommand<CreateProtectionDocWorkflowCommand>().Process(c => c.Execute(newWorkflow));
            }

            var initialStage = await Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(q => q.ExecuteAsync(protectionDoc.CurrentWorkflow.FromStage.Code));

            var initialWorkflow = new ProtectionDocWorkflow()
            {
                CurrentUserId = NiisAmbientContext.Current.User.Identity.UserId,
                OwnerId = newProtectionDocId,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
                IsMain = initialStage.IsMain,
            };
            var initialWorkflowId = Executor.GetCommand<CreateProtectionDocWorkflowCommand>().Process(c => c.Execute(initialWorkflow));

            newProtectionDoc.CurrentWorkflowId = initialWorkflowId;
            await Executor.GetCommand<UpdateProtectionDocCommand>().Process(c => c.ExecuteAsync(newProtectionDocId, newProtectionDoc));

            #endregion

            await CreatePatentOrCertificate(newProtectionDocId, null);

            #region МКТУ

            var newIcgsProtectionDocs = Mapper.Map<ICGSProtectionDoc[]>(icgsDtos);
            foreach (var newIcgsRequest in newIcgsProtectionDocs)
            {
                newIcgsRequest.ProtectionDocId = newProtectionDocId;
            }
            await Executor.GetCommand<UpdateIcgsProtectionDocRangeCommand>()
                .Process(c => c.ExecuteAsync(id, newIcgsProtectionDocs.ToList()));

            #endregion

            #region Биб. данные

            var newConventionInfos = Mapper.Map<ProtectionDocConventionInfo[]>(protectionDoc.ProtectionDocConventionInfos);
            await Executor.GetCommand<AddProtectionDocConventionInfosRangeCommand>()
                .Process(c => c.ExecuteAsync(newProtectionDocId, newConventionInfos.ToList()));

            var newProtectionDocEarlyRegs = Mapper.Map<ProtectionDocEarlyReg[]>(protectionDoc.EarlyRegs);
            await Executor.GetCommand<AddProtectionDocEarlyRegsRangeCommand>()
                .Process(c => c.ExecuteAsync(newProtectionDocId, newProtectionDocEarlyRegs.ToList()));

            var newIpcProtectionDocs = Mapper.Map<IPCProtectionDoc[]>(protectionDoc.IpcProtectionDocs);
            await Executor.GetCommand<AddIpcProtectionDocRelationsCommand>().Process(c =>
                c.ExecuteAsync(newProtectionDocId, newIpcProtectionDocs.Select(i => i.IpcId).ToList()));

            var newIcisProtectionDocs = Mapper.Map<ICISProtectionDoc[]>(protectionDoc.IcisProtectionDocs);
            await Executor.GetCommand<AddIcisProtectionDocRelationsCommand>().Process(c =>
                c.ExecuteAsync(newProtectionDocId, newIcisProtectionDocs.Select(i => i.IcisId).ToList()));

            await Executor.GetCommand<AddColorTzProtectionDocRelationsCommand>().Process(c => c.ExecuteAsync(newProtectionDocId, protectionDoc.ColorTzs.Select(color => color.ColorTzId).ToList()));

            await Executor.GetCommand<AddIcfemProtectionDocRelationsCommand>().Process(c =>
                c.ExecuteAsync(newProtectionDocId, protectionDoc.Icfems.Select(i => i.DicIcfemId).ToList()));

            #endregion

            #region Контрагенты

            var newCustomers = Mapper.Map<ProtectionDocCustomer[]>(protectionDoc.ProtectionDocCustomers);
            await Executor.GetCommand<AddProtectionDocCustomersCommand>().Process(c => c.ExecuteAsync(newProtectionDocId, newCustomers.ToList()));

            #endregion

            var protectionDocumentWorkFlowRequest = new ProtectionDocumentWorkFlowRequest()
            {
                ProtectionDocId = id,
                NextStageUserId = protectionDoc.CurrentWorkflow?.FromUserId ?? 0,
                NextStageCode = protectionDoc.CurrentWorkflow?.FromStage.Code ?? "",
            };

            NiisWorkflowAmbientContext.Current.ProtectionDocumentWorkflowService.Process(protectionDocumentWorkFlowRequest);

            return Ok(newProtectionDocId);
        }

        [AllowAnonymous]
        [HttpGet("{id}/image/{isPreview?}")]
        public async Task<IActionResult> Image(int id, bool isPreview = false)
        {
            var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(id));
            if (protectionDoc == null)
                throw new DataNotFoundException(nameof(ProtectionDoc),
                    DataNotFoundException.OperationType.Update, id);

            var imageArray = isPreview ? protectionDoc.PreviewImage : protectionDoc.Image;

            return File(imageArray, "image/png");
        }

        #region Private methods


        private string GetNextStageCode(ProtectionDoc protectionDoc)
        {
            switch (protectionDoc.CurrentWorkflow.CurrentStage.Code)
            {
                case RouteStageCodes.OD01_1:
                    switch (protectionDoc.Type.Code)
                    {
                        case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                        case DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode:
                        case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                        case DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode:
                            return RouteStageCodes.OD01_3;
                        case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                        case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                        case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                        case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                            return RouteStageCodes.ODParallel;
                        default:
                            return RouteStageCodes.OD01_2_2;
                    }

                case RouteStageCodes.PD_TM_AssignmentRegistrationNumber:
                    return RouteStageCodes.PD_TM_PrintingCertificate;
            }

            return string.Empty;
        }

        /// <summary>
        /// Создает документ (патент или сертификат) и прикрепляет его к охранному документу.
        /// </summary>
        /// <param name="id">Идентификатор охранного документа.</param>
        /// <param name="userId">Идентификатор исполнителя.</param>
        /// <returns>Асинхронная операция.</returns>
        private async Task CreatePatentOrCertificate(int id, int? userId)
        {
            var protectionDoc =  await Executor
                .GetQuery<GetProtectionDocByIdQuery>()
                .Process(q => q.ExecuteAsync(id));

            if (protectionDoc is null)
            {
                throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, 0);
            }
               
            string patentCode = GetPatentCode(protectionDoc);

            var userInputDto = new UserInputDto
            {
                Code = patentCode,
                Fields = new List<KeyValuePair<string, string>>(),
                OwnerId = id,
                OwnerType = Owner.Type.ProtectionDoc
            };

            var documentId = await CreateDocument(id, Owner.Type.ProtectionDoc, patentCode, DocumentType.Outgoing, userInputDto, userId);

            var documentGenerator = _templateGeneratorFactory.Create(patentCode);

            if (documentGenerator != null)
            {
                protectionDoc.PageCount = GenerateDocumentAndGetPagesCount(documentGenerator, documentId, userInputDto);
            }

            await Executor.GetCommand<UpdateProtectionDocCommand>().Process(c => c.ExecuteAsync(id, protectionDoc));
        }

        /// <summary>
        /// Генерирует документ и возвращает количество его страниц.
        /// </summary>
        /// <param name="documentGenerator">Генератор документов.</param>
        /// <param name="documentId">Идентификатор документа.</param>
        /// <param name="userInput">Пользовательский ввод.</param>
        /// <returns>Количество страниц документа.</returns>
        private int GenerateDocumentAndGetPagesCount(IDocumentGenerator documentGenerator, int documentId, UserInputDto userInput)
        {
            return documentGenerator.Process(new Dictionary<string, object>
                {
                    { "UserId", NiisAmbientContext.Current.User.Identity.UserId },
                    { "RequestId", userInput.OwnerId },
                    { "DocumentId", documentId },
                    { "UserInputFields", userInput.Fields },
                    { "SelectedRequestIds", userInput.SelectedRequestIds },
                    { "PageCount", userInput.PageCount },
                    { "OwnerType", userInput.OwnerType },
                    { "Index", userInput.Index }
                }).PageCount;
        }

        /// <summary>
        /// Создает документ и прикрепляет его к родительской сущности, указывая исполнителя по идентификатору.
        /// <para></para>
        /// Если в параметр <paramref name="executorId"/> передан <see langword="null"/>, то указывает исполнителем текущего пользователя.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <param name="ownerType">Тип родительской сущности.</param>
        /// <param name="patentCode">Код типа документа.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="userInput">Пользовательский ввод.</param>
        /// <param name="executorId">Идентификатор исполнителя.</param>
        /// <returns>Идентификатор созданного документа.</returns>
        private async Task<int> CreateDocument(int ownerId, Owner.Type ownerType, string patentCode, DocumentType documentType, UserInputDto userInput, int? executorId)
        {
            if (executorId.HasValue)
            {
                return await CreateDocumentWithExecutor(ownerId, ownerType, patentCode, documentType, userInput, executorId.Value);
            }

            return await CreateDocumentWithCurrentUserAsExecutor(ownerId, ownerType, patentCode, documentType, userInput);
        }

        /// <summary>
        /// Создает документ и прикрепляет его к родительской сущности, указывая исполнителем текущего пользователя.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <param name="ownerType">Тип родительской сущности.</param>
        /// <param name="patentCode">Код типа документа.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="userInput">Пользовательский ввод.</param>
        /// <returns></returns>
        private async Task<int> CreateDocumentWithCurrentUserAsExecutor(int ownerId, Owner.Type ownerType, string patentCode, DocumentType documentType, UserInputDto userInput)
        {
            return await Executor
                .GetHandler<CreateDocumentHandler>()
                .Process(handler => handler.ExecuteAsync(ownerId, ownerType, patentCode, documentType, userInput));
        }

        /// <summary>
        /// Создает документ и прикрепляет его к родительской сущности с указанным исполнителем.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <param name="ownerType">Тип родителской сущности.</param>
        /// <param name="patentCode">Код типа документа.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="userInput">Пользовательский ввод.</param>
        /// <param name="executorId">ИДентификатор исполнителя.</param>
        /// <returns>Идентификатор созданного документа.</returns>
        private async Task<int> CreateDocumentWithExecutor(int ownerId, Owner.Type ownerType, string patentCode, DocumentType documentType, UserInputDto userInput, int executorId)
        {
            return await Executor
                .GetHandler<CreateDocumentWithExplicitExecutorHandler>()
                .Process(handler => handler.ExecuteAsync(ownerId, ownerType, patentCode, documentType, userInput, executorId));
        }

        /// <summary>
        /// Возвращает код типа документа исходя из охранного документа.
        /// </summary>
        /// <param name="protectionDoc">Охранный документ.</param>
        /// <returns>Код типа документа.</returns>
        private string GetPatentCode(ProtectionDoc protectionDoc)
        {
            switch (protectionDoc?.Type?.Code)
            {
                case DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode:
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    return DicDocumentTypeCodes.TrademarkCertificate;
                case DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode:
                case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                    return DicDocumentTypeCodes.NmptCertificate;
                case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    return DicDocumentTypeCodes.InventionPatent;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    switch (protectionDoc?.SelectionAchieveType?.Code)
                    {
                        case DicSelectionAchieveTypeCodes.AnimalHusbandry:
                            return DicDocumentTypeCodes.SelectiveAchievementsAnimalHusbandryPatent;
                        case DicSelectionAchieveTypeCodes.Agricultural:
                        case DicSelectionAchieveTypeCodes.VarietiesPlant:
                            return DicDocumentTypeCodes.SelectiveAchievementsAgriculturalPatent;
                    }
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    return DicDocumentTypeCodes.UsefulModelPatent;
                case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    return DicDocumentTypeCodes.IndustrialDesignsPatent;
            }

            return null;
        }
        #endregion
    }
}