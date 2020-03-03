using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.AutoRouteStages;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentStatusQuery;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType;
using Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.Documents.Numbers;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Workflows.Contracts;
using Iserv.Niis.BusinessLogic.Workflows.Documents;
using Iserv.Niis.BusinessLogic.Workflows.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Journal;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Utils.Constans;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Api.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : BaseNiisApiController
    {
        private readonly IAutoRouteStageHelper _autoRouteStageHelper;
        private readonly IUploadService _uploadService;

        public UploadController(IAutoRouteStageHelper autoRouteStageHelper, IUploadService uploadService)
        {
            _autoRouteStageHelper = autoRouteStageHelper;
            _uploadService = uploadService;
        }

        /// <summary>
        /// Загрузка временных файлов в системную папку Temp
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var response = new List<TempFileItemDto>();
            var files = Request.Form?.Files;

            if (files == null || !files.All(x =>
                    ContentType.Pdf.Equals(x.ContentType)
                    || ContentType.Docx.Equals(x.ContentType)
                    || ContentType.Doc.Equals(x.ContentType)
                    || ContentType.Jpeg.Equals(x.ContentType)
                    || ContentType.Png.Equals(x.ContentType)
                ))
            {
                return BadRequest();
            }

            try
            {
                foreach (var file in files)
                {
                    var tempFilePath = Path.GetTempFileName();
                    if (file.Length <= 0)
                    {
                        continue;
                    }
                    using (var stream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    response.Add(new TempFileItemDto(file.FileName, Path.GetFileName(tempFilePath)));
                }
            }
            catch
            {
                //todo: log exception
                foreach (var tempFileItemDto in response)
                {
                    System.IO.File.Delete(Path.Combine(Path.GetTempPath(), tempFileItemDto.TempName));
                }
                throw;
            }

            return Ok(response);
        }

        /// <summary>
        /// Создает прикрепляет дочерние документы к родительскому документу.
        /// <para></para>
        /// Документы, связанные с родительским документом, будут находится в материалах заявок родительского документа.
        /// </summary>
        /// <param name="attachDto">Модель для связывания родительского документа с дочерними.</param>
        /// <returns>Ответ на запрос.</returns>
        [HttpPost("attachToParent")]
        public async Task<IActionResult> AttachToParent([FromBody]AttachMaterialDto attachDto)
        {
            await _uploadService.CreateMaterialsAndAttachThemToParent(attachDto);
            return Ok();
        }


        [Route("completeRequest")]
        [HttpPost]
        public async Task<IActionResult> CompleteRequest([FromBody] IntellectualPropertyScannerDto dto)
        {
            if (!dto.Id.HasValue)
            {
                var newRequest = Executor.GetHandler<CreateRequestFromUploadedFileHandler>()
                    .Process(h => h.Execute(dto));

                await Executor.GetCommand<CreateRequestCommand>()
                    .Process(c => c.ExecuteAsync(newRequest));
                var initialWorkflow = await Executor.GetQuery<GetInitialRequestWorkflowQuery>().Process(q =>
                    q.ExecuteAsync(newRequest, NiisAmbientContext.Current.User.Identity.UserId));
                if (initialWorkflow != null)
                    await Executor.GetHandler<ProcessRequestWorkflowHandler>()
                        .Process(h => h.Handle(initialWorkflow, newRequest));

                Executor.GetHandler<GenerateRequestIncomingNumberHandler>().Process(c => c.Execute(newRequest.Id));
                await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(newRequest));

                dto.Id = newRequest.Id;
            }

            //TODO! Перенести функционал в проект сканера, тот проект большой, долго разбираться, пока реализовал здесь
            var protectionDocType = Executor.GetQuery<GetDicProtectionDocTypeByIdQuery>()
                .Process(q => q.Execute(dto.ProtectionDocTypeId));
            string documentTypeCode;
            switch (protectionDocType.Code)
            {
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    documentTypeCode = DicDocumentTypeCodes.RequestForTrademark;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeInternationalTrademarkCode:
                    documentTypeCode = DicDocumentTypeCodes.RequestForInternationalTrademark;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    documentTypeCode = DicDocumentTypeCodes.RequestForIndustrialSample;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    documentTypeCode = DicDocumentTypeCodes.RequestForInvention;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                    documentTypeCode = DicDocumentTypeCodes.RequestForNmpt;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    documentTypeCode = DicDocumentTypeCodes.RequestForSelectiveAchievement;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    documentTypeCode = DicDocumentTypeCodes.RequestForUsefulModel;
                    break;
                default:
                    throw new NotImplementedException();
            }
            var dicDocumentType = Executor.GetQuery<GetDicDocumentTypeByCodeQuery>()
                .Process(q => q.Execute(documentTypeCode));
            var requestDocuments =
                await Executor.GetQuery<GetDocumentsByRequestIdQuery>().Process(q => q.ExecuteAsync(dto.Id.Value));
            var newDocument = requestDocuments.FirstOrDefault(d => d.TypeId == dicDocumentType.Id);
            if (newDocument == null)
            {
                var materialDto = new MaterialDetailDto
                {
                    TypeId = dicDocumentType.Id,
                    DocumentType = DocumentType.Incoming,
                    Owners = new[]
                    {
                        new MaterialOwnerDto
                        {
                            OwnerType = Owner.Type.Request,
                            OwnerId = dto.Id.Value,
                            ProtectionDocTypeId = dto.ProtectionDocTypeId
                        }
                    },
                    WasScanned = true
                };

                if (materialDto.DocumentType == DocumentType.DocumentRequest)
                {
                    materialDto.StatusId = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.Completed)).Id;
                }

                newDocument = Executor.GetHandler<CreateDocumentFromUploadedFileHandler>()
                    .Process(h => h.Execute(materialDto));

                var docId = await Executor.GetCommand<CreateDocumentCommand>()
                    .Process(c => c.ExecuteAsync(newDocument));
                var initialWorkflow = await Executor.GetQuery<GetInitialDocumentWorkflowQuery>()
                    .Process(q => q.ExecuteAsync(docId, NiisAmbientContext.Current.User.Identity.UserId));

                await Executor.GetCommand<ApplyDocumentWorkflowCommand>().Process(c => c.ExecuteAsync(initialWorkflow));

                if (newDocument.DocumentType == DocumentType.Incoming)
                    await Executor.GetHandler<GenerateDocumentIncomingNumberHandler>().Process(c => c.ExecuteAsync(docId));
            }
            if (newDocument.MainAttachmentId.HasValue)
            {
                newDocument.MainAttachment = null;
                newDocument.MainAttachmentId = null;
                await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(newDocument));
            }
            var request = await Executor.GetHandler<AddMainAttachToRequestHandler>().Process(r => r.Execute(dto));

            newDocument.MainAttachment = request.MainAttachment;
            newDocument.MainAttachmentId = request.MainAttachmentId;
            newDocument.AddresseeId = request.AddresseeId;
            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(newDocument));

            var requestDto = Mapper.Map<IntellectualPropertyDto>(request);
            return Ok(requestDto);
        }

        [Route("completeContract")]
        [HttpPost]
        public async Task<IActionResult> CompleteContract([FromBody] IntellectualPropertyScannerDto dto)
        {
            if (!dto.Id.HasValue)
            {
                var newContract = Executor.GetHandler<CreateContractFromUploadedFileHandler>()
                    .Process(h => h.Execute(dto));

                await Executor.GetCommand<CreateContractCommand>().Process(c => c.ExecuteAsync(newContract));
                // Executor.GetCommand<UpdateContractCommand>().Process(c => c.ExecuteAsync(newContract));
                await Executor.GetHandler<GenerateContractNumberHandler>().Process(c => c.ExecuteAsync(newContract));

                var initialWorkflow = await Executor.GetQuery<GetInitialContractWorkflowQuery>().Process(q =>
                    q.ExecuteAsync(newContract, NiisAmbientContext.Current.User.Identity.UserId));

                if (initialWorkflow != null)
                    await Executor.GetHandler<ProcessContractWorkflowHandler>().Process(h => h.Handle(initialWorkflow, newContract.Id));

                dto.Id = newContract.Id;
            }

            var contract = await Executor.GetHandler<AddMainAttachToContractHandler>().Process(r => r.Execute(dto));
            var contractDto = Mapper.Map<IntellectualPropertyDto>(contract);
            return Ok(contractDto);
        }

        [Route("completeMaterial")]
        [HttpPost]
        public async Task<IActionResult> CompleteMaterial([FromBody] List<MaterialDetailDto> dtos)
        {
            var materialDetailDtos = new List<MaterialDetailDto>();
            foreach (var dto in dtos)
            {
                if (!dto.Id.HasValue)
                {
                    var requestId = dto.Owners.FirstOrDefault(o => o.OwnerType == Owner.Type.Request)?.OwnerId;
                    var contractId = dto.Owners.FirstOrDefault(o => o.OwnerType == Owner.Type.Contract)?.OwnerId;
                    var protectionDocId = dto.Owners.FirstOrDefault(o => o.OwnerType == Owner.Type.ProtectionDoc)?.OwnerId;
                    var requestDocuments = await Executor.GetQuery<GetDocumentsByRequestIdQuery>()
                        .Process(q => q.ExecuteAsync(requestId ?? 0));
                    var contractDocuments = await Executor.GetQuery<GetDocumentsByContractIdQuery>()
                        .Process(q => q.ExecuteAsync(contractId ?? 0));
                    var protectionDocDocuments = await Executor.GetQuery<GetDocumentsByProtectionDocIdQuery>()
                        .Process(q => q.ExecuteAsync(protectionDocId ?? 0));
                    var existingDocument = requestDocuments.FirstOrDefault(d => d.TypeId == dto.TypeId) ??
                                           contractDocuments.FirstOrDefault(d => d.TypeId == dto.TypeId) ??
                                           protectionDocDocuments.FirstOrDefault(d => d.TypeId == dto.TypeId);


                    dto.Id = existingDocument?.Id;
                }
                if (!dto.Id.HasValue)
                {
                    if (dto.DocumentType == DocumentType.DocumentRequest)
                    {
                        dto.StatusId = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.Completed)).Id;
                    }

                    var newDocument = Executor.GetHandler<CreateDocumentFromUploadedFileHandler>()
                        .Process(h => h.Execute(dto));

                    var workStatus = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.InWork));
                    newDocument.StatusId = workStatus.Id;

                    var docId = await Executor.GetCommand<CreateDocumentCommand>()
                        .Process(c => c.ExecuteAsync(newDocument));
                    var initialWorkflow = await Executor.GetQuery<GetInitialDocumentWorkflowQuery>()
                        .Process(q => q.ExecuteAsync(docId, NiisAmbientContext.Current.User.Identity.UserId));

                    await Executor.GetCommand<ApplyDocumentWorkflowCommand>().Process(c => c.ExecuteAsync(initialWorkflow));

                    if (newDocument.DocumentType == DocumentType.Incoming)
                        await Executor.GetHandler<GenerateDocumentIncomingNumberHandler>().Process(c => c.ExecuteAsync(docId));

                    dto.Id = docId;
                }
                var document = await Executor.GetHandler<AddMainAttachToDocumentHandler>().Process(r => r.Execute(dto));

                if (document.DocumentType == DocumentType.DocumentRequest)
                {
                    document.StatusId = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.Completed)).Id;
                }
                var materialDetailDto = Mapper.Map<MaterialDetailDto>(document);
                //var result = await _autoRouteStageHelper.StartAutuRouteStage(document.Id);

                //if (result != null)
                //{
                //    materialDetailDto.WorkflowDtos = result.ToArray();
                //}
                materialDetailDtos.Add(materialDetailDto);
            }
            
            return Ok(materialDetailDtos);
        }

        [HttpDelete("{documentId}/{isMain}")]
        public async Task<IActionResult> DeleteAttachment(int documentId, bool isMain)
        {
            await Executor.GetHandler<DeleteAttachmentHandler>().Process(h => h.ExecuteAsync(documentId, isMain));

            return NoContent();
        }
    }
}