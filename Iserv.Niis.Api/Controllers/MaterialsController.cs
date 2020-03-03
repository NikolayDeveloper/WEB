using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.BusinessLogic.AutoRouteStages;
using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCommon;
using Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentStatusQuery;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType;
using Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocStatuses;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.BusinessLogic.Dictionaries.DicStageExpirations;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations;
using Iserv.Niis.BusinessLogic.Documents.Numbers;
using Iserv.Niis.BusinessLogic.Documents.UserInput;
using Iserv.Niis.BusinessLogic.DocumentUserSignature;
using Iserv.Niis.BusinessLogic.DocumentWorkflows;
using Iserv.Niis.BusinessLogic.PaymentInvoiceAutoGeneration;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.BusinessLogic.Sing;
using Iserv.Niis.BusinessLogic.Workflows.Documents;
using Iserv.Niis.BusinessLogic.Workflows.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.Material.DocumentRequest;
using Iserv.Niis.Model.Models.Material.Incoming;
using Iserv.Niis.Model.Models.Material.Internal;
using Iserv.Niis.Model.Models.Material.Outgoing;
using Iserv.Niis.Model.Models.Subject;
using Iserv.Niis.Utils.Constans;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Exceptions;
using CreateDocumentCommand = Iserv.Niis.BusinessLogic.Documents.CreateDocumentCommand;
using CreateUserInputCommand = Iserv.Niis.BusinessLogic.Documents.UserInput.CreateUserInputCommand;
using GenerateAutoNotificationHandler = Iserv.Niis.BusinessLogic.Documents.GenerateAutoNotificationHandler;
using GenerateProtectionDocRegisterNumberHandler = Iserv.Niis.BusinessLogic.Documents.Numbers.GenerateProtectionDocRegisterNumberHandler;
using GenerateRegisterDocumentNumberHandler = Iserv.Niis.BusinessLogic.Documents.Numbers.GenerateRegisterDocumentNumberHandler;
using GetDocumentByIdQuery = Iserv.Niis.BusinessLogic.Documents.GetDocumentByIdQuery;
using GetDocumentStatusByCodeQuery = Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentStatusQuery.GetDocumentStatusByCodeQuery;
using GetDocumentUserInputByDocumentIdQuery = Iserv.Niis.BusinessLogic.DocumentUserInput.GetDocumentUserInputByDocumentIdQuery;
using GetInitialDocumentWorkflowQuery = Iserv.Niis.BusinessLogic.Workflows.Documents.GetInitialDocumentWorkflowQuery;
using UpdateDocumentCommand = Iserv.Niis.BusinessLogic.Documents.UpdateDocumentCommand;
using Iserv.Niis.BusinessLogic.Attachments;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Materials")]
    public class MaterialsController : BaseNiisApiController
    {
        private readonly IMapper _mapper;
        private readonly ICustomerUpdater _customerUpdater;
        private readonly IDocumentGeneratorFactory _templateGeneratorFactory;
        private readonly ICalendarProvider _calendarProvider;
        private readonly IAutoRouteStageHelper _autoRouteStageHelper;
        private readonly List<string> _prolongationPetiotionCodes = new List<string>
        {
            DicDocumentTypeCodes.PetitionForExtendTimeRorResponse,
            DicDocumentTypeCodes.Petition_001_004G_3,
            DicDocumentTypeCodes.PetitionForExtendTimeRorObjections
        };

        public MaterialsController(
            IDocumentGeneratorFactory templateGeneratorFactory,
            ICalendarProvider calendarProvider,
            IAutoRouteStageHelper autoRouteStageHelper,
            IMapper mapper,
            ICustomerUpdater customerUpdater)
        {
            _mapper = mapper;
            _customerUpdater = customerUpdater;
            _autoRouteStageHelper = autoRouteStageHelper;
            _calendarProvider = calendarProvider;
            _templateGeneratorFactory = templateGeneratorFactory;
        }

        [HttpGet("printAddressee/{ownerId}/{ownerType}")]
        public IActionResult PrintAddressee(int ownerId, Owner.Type ownerType)
        {
            var documentGenerator = _templateGeneratorFactory.Create("Addressee");
            if (documentGenerator != null)
            {
                var dic = new Dictionary<string, object>
                {
                    {"UserId", NiisAmbientContext.Current.User.Identity.UserId},
                    {"RequestId", ownerId},
                    {"OwnerType", ownerType}
                };

                var generatedFile = documentGenerator.Process(dic);
                var file = generatedFile.File;
                var contentType = ContentType.Pdf;
                return File(file, contentType);
            }
            return NoContent();
        }

        [HttpGet("listByOwner/{ownerType}/{ownerId}")]
        public async Task<IActionResult> ByOwner(int ownerId, Owner.Type ownerType)
        {
            var documents = new List<Document>();
            List<Document> protectionDocDocuments = null;
            var requests = new List<Request>();
            ProtectionDoc protectionDoc = null;

            if (ownerId != 0)
            {
                Request request;
                switch (ownerType)
                {
                    case Owner.Type.Request:
                        documents = await Executor.GetQuery<GetDocumentsByRequestIdQuery>().Process(r => r.ExecuteAsync(ownerId));
                        protectionDoc = Executor.GetQuery<GetProtectionDocByRequestIdQuery>().Process(q => q.Execute(ownerId));
                        var parentRequests = await Executor.GetQuery<GetParentRequestsByChildRequestIdQuery>()
                            .Process(q => q.Execute(ownerId));
                        if (parentRequests.Count > 0)
                        {
                            requests.AddRange(parentRequests);
                        }
                        else
                        {
                            var childRequests = await Executor.GetQuery<GetChildRequestsByParentRequestIdQuery>()
                                .Process(q => q.Execute(ownerId));
                            if (childRequests.Count > 0)
                            {
                                requests.AddRange(childRequests);
                            }
                        }
                        break;
                    case Owner.Type.Contract:
                        documents = await Executor.GetQuery<GetDocumentsByContractIdQuery>().Process(r => r.ExecuteAsync(ownerId));
                        break;
                    case Owner.Type.ProtectionDoc:
                        var owner = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                            .Process(q => q.ExecuteAsync(ownerId));
                        protectionDocDocuments = await Executor.GetQuery<GetDocumentsByProtectionDocIdQuery>().Process(r => r.ExecuteAsync(ownerId));
                        request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(owner.RequestId ?? 0));
                        requests.Add(request);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            var materialDtos = Mapper.Map<List<Document>, List<MaterialItemDto>>(documents);
            if (requests.Count > 0)
            {
                var requestDto = Mapper.Map<List<Request>, List<MaterialItemDto>>(requests);
                materialDtos = materialDtos?.Concat(requestDto).ToList();
                materialDtos = materialDtos?.OrderBy(m => m.DateCreate).ToList();
            }
            if (protectionDoc != null)
            {
                var protectionDocDto = Mapper.Map<ProtectionDoc, MaterialItemDto>(protectionDoc);
                materialDtos?.Add(protectionDocDto);
                materialDtos = materialDtos?.OrderBy(m => m.DateCreate).ToList();
            }
            if (protectionDocDocuments != null)
            {
                var protectionDocMaterialDtos = Mapper.Map<List<Document>, List<MaterialItemDto>>(protectionDocDocuments);
                materialDtos = materialDtos?.Concat(protectionDocMaterialDtos).ToList();
            }
            return Ok(materialDtos);
        }

        [HttpGet("doneMaterial/{id}")]
        public async Task<IActionResult> DoneMaterial(int id)
        {
            var document = Executor.GetQuery<GetDocumentWithIncludesByDocumentIdQuery>().Process(r => r.Execute(id));

            var workStatus = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.Completed));
            document.StatusId = workStatus.Id;
            document.Status = null;

            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));

            var result = GetIncomingDtoByDocument(document);
            result.StatusCode = workStatus.Code;
            result.StatusNameRu = workStatus.NameRu;

            return Ok(result);
        }

        [HttpGet("attachments/{documentId}")]
        public async Task<IActionResult> GetAttachments(int documentId)
        {
            var attachments = await Executor.GetQuery<GetAllAttachmentsQuery>().Process(entry => entry.ExecuteAsync(documentId));

            return Ok(attachments);
        }

        [HttpGet("attachment/{id}")]
        public async Task<IActionResult> GetAttachment(int id)
        {
            var attachment = await Executor.GetQuery<GetAttachmentQuery>().Process(entry => entry.ExecuteAsync(id));

            if (attachment == null)
                throw new DataNotFoundException(nameof(Attachment), DataNotFoundException.OperationType.Read, id);

            var file = await Executor.GetQuery<GetAttachmentFileQuery>().Process(entry => entry.Execute(id));

            return File(file, attachment.ContentType);
        }

        [HttpGet("incoming/{id}")]
        public async Task<IActionResult> GetIncoming(int id)
        {
            var result = GetIncomingDto(id);

            return Ok(result);
        }

        //TODO: Есть аплаеры
        [HttpPost("incoming")]
        public async Task<IActionResult> PostIncoming([FromBody] MaterialIncomingDetailDto detail)
        {
            var userId = NiisAmbientContext.Current.User.Identity.UserId;
            var document = Mapper.Map<Document>(detail);
            document.DocumentType = DocumentType.Incoming;

            if (detail.Owners is null)
            {
                detail.Owners = new MaterialOwnerDto[] { };
            }

            var requestDocumentDtos = Executor.GetHandler<FilterOwnersByOwnerTypeHandler>()
                .Process(h => h.Execute(Owner.Type.Request, detail.Owners.ToList()));
            var contractDocumentDtos = Executor.GetHandler<FilterOwnersByOwnerTypeHandler>()
                .Process(h => h.Execute(Owner.Type.Contract, detail.Owners.ToList()));
            var protectionDocDocumentDtos = Executor.GetHandler<FilterOwnersByOwnerTypeHandler>()
                .Process(h => h.Execute(Owner.Type.ProtectionDoc, detail.Owners.ToList()));

            var workStatus = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.InWork));
            document.StatusId = workStatus.Id;

            var documentId = await Executor.GetCommand<CreateDocumentCommand>()
                .Process(q => q.ExecuteAsync(document));

            var requestDocuments =
                requestDocumentDtos.Select(o => new RequestDocument { DocumentId = documentId, RequestId = o.OwnerId }).ToList();
            var contractDocuments =
                contractDocumentDtos.Select(o => new ContractDocument { DocumentId = documentId, ContractId = o.OwnerId }).ToList();
            var protectionDocDocuments =
                protectionDocDocumentDtos.Select(o => new ProtectionDocDocument { DocumentId = documentId, ProtectionDocId = o.OwnerId }).ToList();
            var initialWorkflow = await Executor.GetQuery<GetInitialDocumentWorkflowQuery>()
                .Process(q => q.ExecuteAsync(documentId, userId));

            Executor.CommandChain()
                .AddCommand<AddRequestDocumentsCommand>(async c => await c.ExecuteAsync(requestDocuments))
                .AddCommand<AddContractDocumentsCommand>(async c => await c.ExecuteAsync(contractDocuments))
                .AddCommand<AddProtectionDocDocumentsCommand>(async c => await c.ExecuteAsync(protectionDocDocuments))
                .AddCommand<ApplyDocumentWorkflowCommand>(async c => await c.ExecuteAsync(initialWorkflow))
                .ExecuteAllWithTransaction();

            if (document.AddresseeId.HasValue && string.IsNullOrWhiteSpace(detail.Apartment) == false)
            {
                await UpdateApartmentInDicCustomer(document.AddresseeId.Value, detail.Apartment, detail.AddresseeShortAddress, detail.ContactInfos);
            }

            //foreach (var requestDocument in requestDocuments)
            //{
            //    await Executor.GetHandler<GenerateAutoNotificationHandler>()
            //        .Process(h => h.Execute(requestDocument.RequestId));
            //    document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));
            //    if (_prolongationPetiotionCodes.Contains(document.Type.Code))
            //    {
            //        await Executor.GetHandler<GenerateAutoTermProlongationPaymentHandler>()
            //            .Process(h => h.ExecuteAsync(requestDocument.RequestId, documentId));
            //    }
            //    await Executor.GetHandler<GenerateAutoPaymentByPetitionHandler>()
            //        .Process(h => h.Execute(documentId, requestDocument.RequestId));
            //}

            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(document.Id));

            await Executor.GetHandler<SetRequestChangeScenarioFlagHandler>().Process(h => h.ExecuteAsync(document.Id));

            await Executor.GetHandler<SaveDocumentCommentHandler>().Process(h => h.ExecuteAsync(documentId, detail.CommentDtos));
            await Executor.GetHandler<SaveDocumentLinkHandler>().Process(h => h.ExecuteAsync(documentId, detail.DocumentLinkDtos));

            return Ok(documentId);
        }

        [HttpPost("numbers")]
        public async Task<IActionResult> GetIncomingNumbers([FromBody] MaterialIncomingDetailDto detail)
        {
            var document = Mapper.Map<Document>(detail);
            await Executor.GetHandler<GenerateDocumentIncomingNumberHandler>().Process(h => h.ExecuteAsync(document));
            Executor.GetHandler<GenerateBarcodeHandler>().Process(h => h.Execute(document));
            document.DateCreate = DateTimeOffset.Now;
            return Ok(new
            {
                Barcode = document.Barcode,
                IncomingNumber = document.IncomingNumber,
                DateCreate = document.DateCreate
            });
        }

        //TODO: Есть аплаеры
        [HttpPut("incoming/{id}")]
        public async Task<IActionResult> PutIncoming(int id, [FromBody] MaterialIncomingDetailDto detail)
        {
            var document = Mapper.Map<Document>(detail);

            if (document.DateOutOfControl.HasValue)
            {
                var workStatus = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.Completed));
                document.StatusId = workStatus.Id;
            }

            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));

            if (document.AddresseeId.HasValue)
            {
                await UpdateApartmentInDicCustomer(document.AddresseeId.Value, detail.Apartment, detail.AddresseeShortAddress, detail.ContactInfos);
            }

            //await Executor.GetHandler<GenerateAutoPaymentByPetitionHandler>()
            //    .Process(h => h.Execute(id,
            //        detail.Owners.FirstOrDefault()?.OwnerId ?? 0));

            await UpdateOwners(id, detail);

            await SaveComments(detail.CommentDtos, document);

            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(document.Id));
            await Executor.GetHandler<SaveDocumentCommentHandler>().Process(h => h.ExecuteAsync(document.Id, detail.CommentDtos));
            await Executor.GetHandler<SaveDocumentLinkHandler>().Process(h => h.ExecuteAsync(document.Id, detail.DocumentLinkDtos));

            var result = GetIncomingDto(id);

            return Ok(result);
        }

        /// <summary>
        /// Сохраняет новые комментарии к документу
        /// </summary>
        /// <param name="commentDtos"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        private async Task SaveComments(DocumentCommentDto[] commentDtos, Document document)
        {
            foreach (var dto in commentDtos.Where(d => d.Id <= 0))
            {
                dto.DocumentId = document.Id;
                dto.WorkflowId = document.Workflows.FirstOrDefault(d => d.IsCurent)?.Id;
                await Executor.GetQuery<SaveDocumentCommentCommand>().Process(q => q.ExecuteAsync(dto));
            }
        }

        [HttpGet("outgoing/{id}")]
        public async Task<IActionResult> GetOutgoing(int id)
        {
            var result = await GetOutgoingDetailDto(id);

            return Ok(result);
        }

        //TODO: Есть аплаеры
        [HttpPost("outgoing")]
        public async Task<IActionResult> PostOutgoing([FromBody] MaterialOutgoingDetailDto detail)
        {
            var userId = NiisAmbientContext.Current.User.Identity.UserId;
            var document = Mapper.Map<Document>(detail);
            document.DocumentType = DocumentType.Outgoing;

            var workStatus = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.InWork));
            document.StatusId = workStatus.Id;

            //var newDocumentType = await Executor.GetQuery<GetDicDocumentTypeByIdQuery>()
            //    .Process(q => q.ExecuteAsync(detail.TypeId ?? 0));
            int documentId;

            // удаленно (старый док)
            //if (newDocumentType?.Code == DicDocumentTypeCodes.Reestr_006_014_3)
            //{
            //    var protectionDocId =
            //        detail.Owners.FirstOrDefault(o => o.OwnerType == Owner.Type.ProtectionDoc)?.OwnerId;
            //    var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
            //        .Process(q => q.ExecuteAsync(protectionDocId ?? 0));
            //    var bulletinId = protectionDoc.Bulletins.FirstOrDefault(b => b.IsPublish)?.BulletinId;
            //    var register = Executor.GetQuery<GetProtectionDocRegisterByBulletinIdAndProtectionDocTypeIdQuery>()
            //        .Process(q => q.Execute(bulletinId ?? 0, protectionDoc.TypeId));
            //    if (register != null)
            //    {
            //        if (register.ProtectionDocs.All(pdd => pdd.ProtectionDocId != protectionDocId))
            //        {
            //            var protectionDocDocument = new ProtectionDocDocument
            //            {
            //                DocumentId = register.Id,
            //                ProtectionDocId = protectionDocId ?? 0
            //            };
            //            Executor.GetCommand<CreateProtectionDocDocumentCommand>()
            //                .Process(c => c.Execute(protectionDocDocument));
            //        }
            //        return Ok(register.Id);
            //    }
            //    documentId = await CreateOutgoing(detail, userId, document);
            //    await Executor.GetHandler<GenerateProtectionDocRegisterNumberHandler>()
            //        .Process(h => h.ExecuteAsync(documentId));
            //    return Ok(documentId);
            //}
            documentId = await CreateOutgoing(detail, userId, document);

            await Executor.GetHandler<SaveDocumentCommentHandler>().Process(h => h.ExecuteAsync(documentId, detail.CommentDtos));
            await Executor.GetHandler<SaveDocumentLinkHandler>().Process(h => h.ExecuteAsync(documentId, detail.DocumentLinkDtos));

            return Ok(documentId);
        }

        private async Task<int> CreateOutgoing(MaterialOutgoingDetailDto detail, int userId, Document document)
        {
            var documentId = await Executor.GetCommand<CreateDocumentCommand>()
                            .Process(q => q.ExecuteAsync(document));

            await Executor.GetHandler<GenerateDocumentBarcodeHandler>().Process(c => c.ExecuteAsync(documentId));
            await Executor.GetHandler<GenerateRegisterDocumentNumberHandler>().Process(c => c.ExecuteAsync(documentId));
            await Executor.GetHandler<GenerateNumberForPaymentHandler>().Process(c => c.ExecuteAsync(documentId));

            await AddOwnersAndInput(detail, documentId, userId, detail.UserInput);

            if (document.AddresseeId.HasValue && string.IsNullOrWhiteSpace(detail.Apartment) == false)
            {
                await UpdateApartmentInDicCustomer(document.AddresseeId.Value, detail.Apartment, detail.AddresseeShortAddress, detail.ContactInfos);
            }

            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(documentId));
            return documentId;
        }

        //TODO: Есть аплаеры
        [HttpPut("outgoing/{id}")]
        public async Task<IActionResult> PutOutgoing(int id, [FromBody] MaterialOutgoingDetailDto detail)
        {
            var document = Mapper.Map<Document>(detail);

            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));

            if (document.AddresseeId.HasValue)
            {
                await UpdateApartmentInDicCustomer(document.AddresseeId.Value, detail.Apartment, detail.AddresseeShortAddress, detail.ContactInfos);
            }

            await UpdateOwners(id, detail);

            var userInput = await Executor.GetQuery<GetUserInputByDocumentIdQuery>().Process(q => q.ExecuteAsync(id));
            if (userInput == null)
            {
                await Executor.GetCommand<CreateUserInputCommand>().Process(c => c.ExecuteAsync(id, detail.UserInput));
            }
            else
            {
                await Executor.GetCommand<UpdateUserInputCommand>().Process(c => c.ExecuteAsync(id, detail.UserInput));
            }

            await SaveComments(detail.CommentDtos, document);

            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(document.Id));
            await Executor.GetHandler<SaveDocumentCommentHandler>().Process(h => h.ExecuteAsync(document.Id, detail.CommentDtos));
            await Executor.GetHandler<SaveDocumentLinkHandler>().Process(h => h.ExecuteAsync(document.Id, detail.DocumentLinkDtos));

            var result = await GetOutgoingDetailDto(id);

            return Ok(result);
        }

        /// <summary>
        /// Получить Документ Заявки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("documentRequest/{id}")]
        public async Task<IActionResult> GetDocumentRequests(int id)
        {
            var result = await GetDocumentRequestDetailDto(id);

            return Ok(result);
        }

        /// <summary>
        /// Созать Документ Заявки
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [HttpPost("documentRequest")]
        public async Task<IActionResult> PostDocumentRequest([FromBody] MaterialDocumentRequestDetailDto detail)
        {
            var userId = NiisAmbientContext.Current.User.Identity.UserId;
            var document = Mapper.Map<Document>(detail);
            document.DocumentType = DocumentType.DocumentRequest;

            var workStatus = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.Completed));
            document.StatusId = workStatus.Id;

            var documentId = await Executor.GetCommand<CreateDocumentCommand>().Process(q => q.ExecuteAsync(document));
            var initialWorkflow = await Executor.GetQuery<GetInitialDocumentWorkflowQuery>()
                .Process(q => q.ExecuteAsync(documentId, userId));

            await Executor.GetCommand<ApplyDocumentWorkflowCommand>().Process(c => c.ExecuteAsync(initialWorkflow));

            await Executor.GetHandler<GenerateDocumentBarcodeHandler>().Process(c => c.ExecuteAsync(documentId));

            await AddOwnersAndInput(detail, documentId, userId, detail.UserInput);

            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(document.Id));

            await Executor.GetHandler<SaveDocumentCommentHandler>().Process(h => h.ExecuteAsync(documentId, detail.CommentDtos));
            await Executor.GetHandler<SaveDocumentLinkHandler>().Process(h => h.ExecuteAsync(documentId, detail.DocumentLinkDtos));

            return Ok(documentId);
        }

        /// <summary>
        /// Сохранить Документ Заявки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        [HttpPut("documentRequest/{id}")]
        public async Task<IActionResult> PutDocumentRequest(int id, [FromBody] MaterialDocumentRequestDetailDto detail)
        {
            var document = Mapper.Map<Document>(detail);

            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));

            await UpdateOwners(id, detail);

            var userInput = await Executor.GetQuery<GetUserInputByDocumentIdQuery>().Process(q => q.ExecuteAsync(id));
            if (userInput == null)
            {
                await Executor.GetCommand<CreateUserInputCommand>().Process(c => c.ExecuteAsync(id, detail.UserInput));
            }
            else
            {
                await Executor.GetCommand<UpdateUserInputCommand>().Process(c => c.ExecuteAsync(id, detail.UserInput));
            }

            await SaveComments(detail.CommentDtos, document);

            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(document.Id));
            await Executor.GetHandler<SaveDocumentCommentHandler>().Process(h => h.ExecuteAsync(document.Id, detail.CommentDtos));
            await Executor.GetHandler<SaveDocumentLinkHandler>().Process(h => h.ExecuteAsync(document.Id, detail.DocumentLinkDtos));

            var result = await GetDocumentRequestDetailDto(id);

            return Ok(result);
        }

        [HttpGet("internal/{id}")]
        public async Task<IActionResult> GetInternal(int id)
        {
            var result = await GetInternalDetailDto(id);

            return Ok(result);
        }

        //TODO: Есть аплаеры
        [HttpPost("internal")]
        public async Task<IActionResult> PostInternal([FromBody] MaterialInternalDetailDto detail)
        {
            var userId = NiisAmbientContext.Current.User.Identity.UserId;
            var document = Mapper.Map<Document>(detail);
            document.DocumentType = DocumentType.Internal;

            var workStatus = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.InWork));
            document.StatusId = workStatus.Id;

            var documentId = await Executor.GetCommand<CreateDocumentCommand>()
                .Process(q => q.ExecuteAsync(document));

            await Executor.GetHandler<GenerateDocumentBarcodeHandler>().Process(c => c.ExecuteAsync(documentId));

            var documentTypesNeedToGenerateRegisterDocumentNumber = new string[] { DicDocumentTypeCodes.ResultDistributionRequests, DicDocumentTypeCodes.DK_Registry_Transfer };

            var documentType = await Executor.GetQuery<GetDictionaryRowByEntityNameAndIdQuery>().Process(q => q.ExecuteAsync(DictionaryType.DicDocumentType, document.TypeId));
            if (documentTypesNeedToGenerateRegisterDocumentNumber.Contains(documentType.Code))
            {
                await Executor.GetHandler<GenerateRegisterDocumentNumberHandler>().Process(c => c.ExecuteAsync(documentId));
            }

            await AddOwnersAndInput(detail, documentId, userId, detail.UserInput);

            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(document.Id));

            await Executor.GetHandler<SaveDocumentCommentHandler>().Process(h => h.ExecuteAsync(documentId, detail.CommentDtos));
            await Executor.GetHandler<SaveDocumentLinkHandler>().Process(h => h.ExecuteAsync(documentId, detail.DocumentLinkDtos));

            return Ok(documentId);
        }

        //TODO: Есть аплаеры
        [HttpPut("internal/{id}")]
        public async Task<IActionResult> PutInternal(int id, [FromBody] MaterialInternalDetailDto detail)
        {
            var document = Mapper.Map<Document>(detail);

            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));

            await UpdateOwners(id, detail);

            var userInput = await Executor.GetQuery<GetUserInputByDocumentIdQuery>().Process(q => q.ExecuteAsync(id));
            if (userInput == null)
            {
                await Executor.GetCommand<CreateUserInputCommand>().Process(c => c.ExecuteAsync(id, detail.UserInput));
            }
            else
            {
                await Executor.GetCommand<UpdateUserInputCommand>().Process(c => c.ExecuteAsync(id, detail.UserInput));
            }

            await SaveComments(detail.CommentDtos, document);

            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(document.Id));
            await Executor.GetHandler<SaveDocumentCommentHandler>().Process(h => h.ExecuteAsync(document.Id, detail.CommentDtos));
            await Executor.GetHandler<SaveDocumentLinkHandler>().Process(h => h.ExecuteAsync(document.Id, detail.DocumentLinkDtos));

            var result = await GetInternalDetailDto(id);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Executor.GetCommand<DocumentMarkAsDeletedCommand>().Process(r => r.Execute(id));

            return NoContent();
        }

        //TODO: Есть аплаеры
        [HttpPost("previousWorkflows")]
        public async Task<IActionResult> PreviousPost([FromBody] MaterialWorkflowDto workflowDto)
        {
            var result = await Executor.GetHandler<CreateMaterialWorkFlowHandler>().Process(d => d.ExecuteAsync(workflowDto, true));
            return Ok(result);
        }

        [HttpPut("updateWorkflows")]
        public async Task<IActionResult> UpdateWorkflows([FromBody] MaterialWorkflowDto workflowDto)
        {
            await Executor.GetCommand<UpdateMaterialWorkFlowCommand>().Process(d => d.ExecuteAsync(workflowDto));

            var workflowWithIncludes = await Executor.GetQuery<GetWorkflowsByDocumentIdQuery>()
                .Process(q => q.ExecuteAsync(workflowDto.Id));
            var result = Mapper.Map<MaterialWorkflowDto>(workflowWithIncludes);

            return Ok(result);
        }

        [HttpPost("workflows")]
        public async Task<IActionResult> Post([FromBody] MaterialWorkflowDto workflowDto)
        {
            var result = await Executor.GetHandler<CreateMaterialWorkFlowHandler>().Process(d => d.ExecuteAsync(workflowDto));
            return Ok(result);
        }

        [HttpGet]
        public IActionResult List()
        {
            var curentUserId = NiisAmbientContext.Current.User.Identity.UserId;
            return GetListByUserId(curentUserId);
        }

        [HttpGet("users/{userId}")]
        public IActionResult ListBy(int userId)
        {
            return GetListByUserId(userId);
        }


        /// <summary>
        /// Загрузка по коду
        /// </summary>
        /// <returns></returns>
        [HttpGet("lastStageIncoming")]
        public async Task<IActionResult> LastStageIncoming()
        {
            //TODO: here IQueryable response
            var documents = Executor.GetQuery<GetRequestDocumentsQuery>().Process(r => r.Execute());

            var curentUserId = NiisAmbientContext.Current.User.Identity.UserId;

            var materialsResult = documents
                //Проверка на наличие текущего пользователя в исполнителях этапа или в списке просматривателей
                .Where(d => d.Workflows.Any(c => c.IsCurent && (c.CurrentStage.Code == DicRouteStageCodes.DocumentProcessing || c.CurrentStage.Code == DicRouteStageCodes.DocumentProcessingPay)))
                .Filter(Request.Query)
                .Sort(Request.Query)
                .ProjectTo<MaterialTaskDto>()
                .ToPagedList(Request.GetPaginationParams());

            return materialsResult.AsOkObjectResult(Response);
        }

        [HttpGet("linkMaterials")]
        public async Task<IActionResult> LinkMaterials()
        {
            var documents = Executor.GetQuery<GetRequestDocumentsQuery>().Process(r => r.Execute());

            var materialsResult = await documents
                    .Filter(Request.Query)
                    .Sort(Request.Query)
                    .ProjectTo<MaterialTaskDto>()
                    .ToPagedListAsync(Request.GetPaginationParams())
                ;

            return materialsResult.AsOkObjectResult(Response);
        }

        [HttpPost("sign")]
        public async Task<IActionResult> SignDocument([FromBody] DocumentUserSignatureDto documentUserSignatureDto)
        {
            documentUserSignatureDto.UserId = NiisAmbientContext.Current.User.Identity.UserId;

            var curentUser = Executor.GetQuery<GetUserByIdQuery>().Process(d => d.Execute(documentUserSignatureDto.UserId));
            curentUser.CertPassword = documentUserSignatureDto.Password;
            curentUser.CertStoragePath = documentUserSignatureDto.CertStoragePath;
            Executor.GetCommand<UpdateUserCommand>().Process(d => d.Execute(curentUser));

            var documentUserSignature = Mapper.Map<DocumentUserSignature>(documentUserSignatureDto);

            var isValidSertificate = Executor.GetCommand<ValidateSingHandler>().Process(r => r.Execute(documentUserSignature));

            documentUserSignature.IsValidCertificate = isValidSertificate;

            var documentWorkflow = Executor.GetQuery<GetDocumentWorkflowByIdQuery>().Process(r => r.Execute(documentUserSignature.WorkflowId));

            var requests = await Executor.GetQuery<GetRequestsByDocumentIdQuery>()
                .Process(q => q.ExecuteAsync(documentWorkflow.OwnerId));

            foreach (var request in requests)
            {
                if (new[] { RouteStageCodes.UM_03_4 }.Contains(request.CurrentWorkflow.CurrentStage.Code))
                {
                    request.CurrentWorkflow.CurrentUserId = documentWorkflow.CurrentUserId;
                    await Executor.GetCommand<UpdateRequestWorkflowCommand>()
                        .Process(c => c.ExecuteAsync(request.CurrentWorkflow));
                }
            }

            await Executor.GetCommand<CreateDocumentUserSignatureCommand>().Process(r => r.Execute(documentUserSignature));
            await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(documentWorkflow.OwnerId));

            return Ok(Mapper.Map<DocumentUserSignatureDto>(documentUserSignature));
        }

		[HttpGet("generateOutgoingNumber/{documentId}")]
		public async Task<IActionResult> GenerateOutgoingNumber(int documentId)
		{
			var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(r => r.ExecuteAsync(documentId));

			if (document.SendingDate == null)
			{
				document.SendingDate = DateTimeOffset.Now;
				await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
			}

			await Executor.GetHandler<GenerateDocumentOutgoingNumberHandler>().Process(r => r.Execute(document));

			new Notifications.Implementations.NotificationTaskQueueRegister(_templateGeneratorFactory).AddNotificationsByOwnerType(document.Id, Owner.Type.Material);

			//var patentCodes = new List<string>
			//{
			//	DicDocumentTypeCodes.IndustrialDesignsPatent,
			//	DicDocumentTypeCodes.InventionPatent,
			//	DicDocumentTypeCodes.SelectiveAchievementsAgriculturalPatent,
			//	DicDocumentTypeCodes.SelectiveAchievementsAnimalHusbandryPatent,
			//	DicDocumentTypeCodes.UsefulModelPatent,
			//	DicDocumentTypeCodes.NmptCertificate,
			//	DicDocumentTypeCodes.TrademarkCertificate,
			//};
			//if (patentCodes.Contains(document.Type.Code))
			//{
			//	foreach (var protectionDocDocument in document.ProtectionDocs)
			//	{
			//		//var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
			//		//	.Process(q => q.ExecuteAsync(protectionDocDocument.ProtectionDocId));
			//		//if (protectionDoc.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD01_5_1)
			//		//{
			//		//    protectionDoc.OutgoingNumber = document.OutgoingNumber;
			//		//    protectionDoc.OutgoingDate = document.SendingDate;
			//		//    await Executor.GetCommand<UpdateProtectionDocCommand>()
			//		//        .Process(c => c.ExecuteAsync(protectionDoc.Id, protectionDoc));
			//		//}
			//	}
			//}
			if (document.Type.Code == DicDocumentTypeCodes.ProtectionDocAnullmentNotification)
			{
				foreach (var protectionDocDocument in document.ProtectionDocs)
				{
					var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
						.Process(q => q.ExecuteAsync(protectionDocDocument.ProtectionDocId));
					if (protectionDoc.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD03_2)
					{
						var annulledStatus = Executor.GetQuery<GetDicProtectionDocStatusByCodeQuery>()
							.Process(q => q.ExecuteAsync(DicProtectionDocStatusCodes._03_47));
						protectionDoc.StatusId = annulledStatus?.Id;
						await Executor.GetCommand<UpdateProtectionDocCommand>()
							.Process(c => c.ExecuteAsync(protectionDoc.Id, protectionDoc));
					}
				}
			}

			await Executor.GetHandler<ProcessWorkflowByDocumentIdHandler>().Process(r => r.ExecuteAsync(document.Id));

			foreach (var requestDocument in document.Requests)
			{
				await Executor.GetHandler<ChargePaymentsByDocumentsOutgoingDateHandler>().Process(h =>
					h.Execute(Owner.Type.Request, requestDocument.RequestId, document.Type.Code));

				await Executor.GetHandler<ChargeSplitPaymentHandler>()
					.Process(h => h.ExecuteAsync(requestDocument.RequestId, document.Type.Code));

				await Executor.GetHandler<ChargeRequestPublicDateHandler>().Process(h =>
					h.Execute(requestDocument.RequestId));
			}

			await Executor.GetHandler<UnsetRequestChangeScenarioFlagHandler>().Process(h => h.ExecuteAsync(document.Id));

			if (document.IncomingAnswerId != null)
			{
				var incomingAnswerDocument = await Executor.GetQuery<GetDocumentByIdQuery>().Process(r => r.ExecuteAsync(document.IncomingAnswerId.Value));
				if (incomingAnswerDocument != null && incomingAnswerDocument.Type.Code != "006.05" && incomingAnswerDocument.Type.Code != "006.03")
				{
					var workStatus = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.Completed));
					incomingAnswerDocument.StatusId = workStatus.Id;

					await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(incomingAnswerDocument));
				}
				else
				{
					//await _autoRouteStageHelper.StartAutuRouteStage(document.IncomingAnswerId.GetValueOrDefault(0), true);
				}
			}


            document.StatusId = Executor
                                        .GetQuery<GetDocumentStatusByCodeQuery>()
                                        .Process(query => query.Execute(DicDocumentStatusCodes.Completed)).Id;
            await Executor
                        .GetCommand<UpdateDocumentCommand>()
                        .Process(command => command.Execute(document));

            return Ok(new { document.OutgoingNumber, document.SendingDate });
		}

        /// <summary>
        /// Возвращает статус документа по идентификатору документа.
        /// </summary>
        /// <param name="documentId">Идентификатор документа.</param>
        /// <returns>Статус документа.</returns>
        [HttpGet("getDocumentStatus/{documentId}")]
        public async Task<IActionResult> GetDocumentStatus(int documentId)
        {
            DicDocumentStatus status = await Executor
                                    .GetQuery<GetDocumentStatusByDocumentIdQuery>()
                                    .Process(query => query.ExecuteAsync(documentId));

            return Ok(_mapper.Map<SelectOptionDto>(status));
        }

        [HttpGet("internalRegister/{ownerId}/{ownerType}")]
        public IActionResult GetInternalRegister(int ownerId, Owner.Type ownerType)
        {
            var documentGenerator = _templateGeneratorFactory.Create(DicDocumentTypeCodes.IN01_vn_opis_V1_19);
            if (documentGenerator != null)
            {
                var dic = new Dictionary<string, object>
                {
                    {"UserId", NiisAmbientContext.Current.User.Identity.UserId},
                    {"RequestId", ownerId},
                    {"OwnerType", ownerType}
                };

                var generatedFile = documentGenerator.Process(dic);
                var file = generatedFile.File;
                var contentType = ContentType.Pdf;
                return File(file, contentType);
            }
            return NoContent();
        }

        private MaterialIncomingDetailDto GetIncomingDto(int documentId)
        {
            var document = Executor.GetQuery<GetDocumentWithIncludesByDocumentIdQuery>().Process(r => r.Execute(documentId));

            if (document == null)
                throw new DataNotFoundException(nameof(Document), DataNotFoundException.OperationType.Read, documentId);

            return GetIncomingDtoByDocument(document);
        }

        private MaterialIncomingDetailDto GetIncomingDtoByDocument(Document document)
        {
            var result = Mapper.Map<Document, MaterialIncomingDetailDto>(document);
            var requestsOwnerDtos = Mapper.Map<RequestDocument[], MaterialOwnerDto[]>(document.Requests.ToArray());
            var contractOwnerDtos = Mapper.Map<ContractDocument[], MaterialOwnerDto[]>(document.Contracts.ToArray());
            var protectionDocOwnerDtos =
                Mapper.Map<ProtectionDocDocument[], MaterialOwnerDto[]>(document.ProtectionDocs.ToArray());

            result.Owners = requestsOwnerDtos.Concat(contractOwnerDtos.Concat(protectionDocOwnerDtos)).ToArray();
            result.PageCount = document.PageCount ?? document.MainAttachment?.PageCount;

            var curentUserId = NiisAmbientContext.Current.User.Identity.UserId;
            result.IsReadOnly = document.CurrentWorkflows.All(cwf => cwf.CurrentUserId != curentUserId);
            return result;
        }

        private async Task<MaterialOutgoingDetailDto> GetOutgoingDetailDto(int documentId)
        {
            var document = Executor.GetQuery<GetDocumentWithIncludesByDocumentIdQuery>().Process(r => r.Execute(documentId));

            if (document == null)
                throw new DataNotFoundException(nameof(Document),
                    DataNotFoundException.OperationType.Read, documentId);
            var result = Mapper.Map<Document, MaterialOutgoingDetailDto>(document);
            var requestsOwnerDtos = Mapper.Map<RequestDocument[], MaterialOwnerDto[]>(document.Requests.ToArray());
            var contractOwnerDtos = Mapper.Map<ContractDocument[], MaterialOwnerDto[]>(document.Contracts.ToArray());
            var protectionDocOwnerDtos =
                Mapper.Map<ProtectionDocDocument[], MaterialOwnerDto[]>(document.ProtectionDocs.ToArray());
            result.Owners = requestsOwnerDtos.Concat(contractOwnerDtos.Concat(protectionDocOwnerDtos)).ToArray();

            var curentUserId = NiisAmbientContext.Current.User.Identity.UserId;
            result.IsReadOnly = document.CurrentWorkflows.All(cwf => cwf.CurrentUserId != curentUserId);

            var input = await Executor.GetQuery<GetDocumentUserInputByDocumentIdQuery>()
                .Process(q => q.ExecuteAsync(document.Id));

            if (input != null)
            {
                result.UserInput = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);
                //if (document.MainAttachmentId == null)
                //{
                //    var documentGenerator = _templateGeneratorFactory.Create(result.UserInput.Code);
                //    if (documentGenerator != null)
                //    {
                //        var dic = new Dictionary<string, object>
                //        {
                //            {"UserId", NiisAmbientContext.Current.User.Identity.UserId},
                //            {"RequestId", result.UserInput.OwnerId},
                //            {"DocumentId", documentId},
                //            {"UserInputFields", result.UserInput.Fields},
                //            {"SelectedRequestIds", result.UserInput.SelectedRequestIds},
                //            {"PageCount", result.PageCount},
                //            {"OwnerType", result.UserInput.OwnerType},
                //            {"Index", result.UserInput.Index }
                //        };

                //        var generatedFile = documentGenerator.Process(dic);
                //        result.PageCount = document.PageCount ?? generatedFile.PageCount;
                //    }
                //}
                //else
                //{
                //    result.PageCount = document.PageCount ?? document.MainAttachment.PageCount;
                //}
                result.PageCount = document.PageCount ?? document.MainAttachment?.PageCount;
            }
            else if (document.MainAttachmentId.HasValue)
            {
                result.PageCount = document.PageCount ?? document.MainAttachment?.PageCount;
            }
            return result;
        }

        /// <summary>
        /// Получить DTO для типа "Документ Заявки"
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        private async Task<MaterialDocumentRequestDetailDto> GetDocumentRequestDetailDto(int documentId)
        {
            var document = Executor.GetQuery<GetDocumentWithIncludesByDocumentIdQuery>().Process(r => r.Execute(documentId));

            if (document == null)
                throw new DataNotFoundException(nameof(Document), DataNotFoundException.OperationType.Read, documentId);

            var result = Mapper.Map<Document, MaterialDocumentRequestDetailDto>(document);

            var input = await Executor.GetQuery<GetDocumentUserInputByDocumentIdQuery>()
                .Process(q => q.ExecuteAsync(document.Id));

            if (input != null)
            {
                result.UserInput = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);
                //if (document.MainAttachmentId == null)
                //{
                //    var documentGenerator = _templateGeneratorFactory.Create(result.UserInput.Code);
                //    if (documentGenerator != null)
                //    {
                //        var generatedFile = documentGenerator?.Process(new Dictionary<string, object>
                //        {
                //            {"UserId", NiisAmbientContext.Current.User.Identity.UserId},
                //            {"RequestId", result.UserInput.OwnerId},
                //            {"DocumentId", documentId},
                //            {"UserInputFields", result.UserInput.Fields},
                //            {"SelectedRequestIds", result.UserInput.SelectedRequestIds},
                //            {"PageCount", result.PageCount},
                //            {"OwnerType", result.UserInput.OwnerType},
                //            {"Index", result.UserInput.Index }
                //        });
                //        result.PageCount = document.PageCount ?? generatedFile?.PageCount;
                //    }
                //}
                //else
                //{
                //    result.PageCount = document.PageCount ?? document.MainAttachment.PageCount;
                //}
                //result.PageCount = document.PageCount ?? document.MainAttachment.PageCount;
            }

            result.PageCount = document.PageCount ?? document.MainAttachment?.PageCount;

            var requestsOwnerDtos = Mapper.Map<RequestDocument[], MaterialOwnerDto[]>(document.Requests.ToArray());
            var contractOwnerDtos = Mapper.Map<ContractDocument[], MaterialOwnerDto[]>(document.Contracts.ToArray());
            var protectionDocOwnerDtos = Mapper.Map<ProtectionDocDocument[], MaterialOwnerDto[]>(document.ProtectionDocs.ToArray());
            result.Owners = requestsOwnerDtos.Concat(contractOwnerDtos.Concat(protectionDocOwnerDtos)).ToArray();

            var curentUserId = NiisAmbientContext.Current.User.Identity.UserId;
            result.IsReadOnly = document.CurrentWorkflows.All(cwf => cwf.CurrentUserId != curentUserId);

            return result;
        }

        private async Task<MaterialInternalDetailDto> GetInternalDetailDto(int documentId)
        {
            var document = Executor.GetQuery<GetDocumentWithIncludesByDocumentIdQuery>().Process(r => r.Execute(documentId));

            if (document == null)
                throw new DataNotFoundException(nameof(Document), DataNotFoundException.OperationType.Read, documentId);

            var result = Mapper.Map<Document, MaterialInternalDetailDto>(document);

            var input = await Executor.GetQuery<GetDocumentUserInputByDocumentIdQuery>()
                .Process(q => q.ExecuteAsync(document.Id));

            if (input != null)
            {
                result.UserInput = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);
                //if (document.MainAttachmentId == null)
                //{
                //    var documentGenerator = _templateGeneratorFactory.Create(result.UserInput.Code);
                //    if (documentGenerator != null)
                //    {
                //        var generatedFile = documentGenerator?.Process(new Dictionary<string, object>
                //        {
                //            {"UserId", NiisAmbientContext.Current.User.Identity.UserId},
                //            {"RequestId", result.UserInput.OwnerId},
                //            {"DocumentId", documentId},
                //            {"UserInputFields", result.UserInput.Fields},
                //            {"SelectedRequestIds", result.UserInput.SelectedRequestIds},
                //            {"PageCount", result.PageCount},
                //            {"OwnerType", result.UserInput.OwnerType},
                //            {"Index", result.UserInput.Index }
                //        });
                //        result.PageCount = document.PageCount ?? generatedFile?.PageCount;
                //    }
                //}
                //else
                //{
                //    result.PageCount = document.PageCount ?? document.MainAttachment?.PageCount;
                //}
            }
            result.PageCount = document.PageCount ?? document.MainAttachment?.PageCount;

            var requestsOwnerDtos = Mapper.Map<RequestDocument[], MaterialOwnerDto[]>(document.Requests.ToArray());
            var contractOwnerDtos = Mapper.Map<ContractDocument[], MaterialOwnerDto[]>(document.Contracts.ToArray());
            var protectionDocOwnerDtos =
                Mapper.Map<ProtectionDocDocument[], MaterialOwnerDto[]>(document.ProtectionDocs.ToArray());
            result.Owners = requestsOwnerDtos.Concat(contractOwnerDtos.Concat(protectionDocOwnerDtos)).ToArray();

            var curentUserId = NiisAmbientContext.Current.User.Identity.UserId;
            result.IsReadOnly = document.CurrentWorkflows.All(cwf => cwf.CurrentUserId != curentUserId);

            return result;
        }

        private async Task AddOwnersAndInput(MaterialDetailDto detail, int documentId, int userId, UserInputDto userInputDto)
        {
            var requestDocumentDtos = Executor.GetHandler<FilterOwnersByOwnerTypeHandler>()
                .Process(h => h.Execute(Owner.Type.Request, detail.Owners.ToList()));
            var contractDocumentDtos = Executor.GetHandler<FilterOwnersByOwnerTypeHandler>()
                .Process(h => h.Execute(Owner.Type.Contract, detail.Owners.ToList()));
            var protectionDocDocumentDtos = Executor.GetHandler<FilterOwnersByOwnerTypeHandler>()
                .Process(h => h.Execute(Owner.Type.ProtectionDoc, detail.Owners.ToList()));

            var requestDocuments =
                requestDocumentDtos.Select(o => new RequestDocument { DocumentId = documentId, RequestId = o.OwnerId }).ToList();
            var contractDocuments =
                contractDocumentDtos.Select(o => new ContractDocument { DocumentId = documentId, ContractId = o.OwnerId })
                    .ToList();
            var protectionDocDocuments =
                protectionDocDocumentDtos
                    .Select(o => new ProtectionDocDocument { DocumentId = documentId, ProtectionDocId = o.OwnerId }).ToList();

            var initialWorkflow = await Executor.GetQuery<GetInitialDocumentWorkflowQuery>()
                .Process(q => q.ExecuteAsync(documentId, userId));

            Executor.CommandChain()
                .AddCommand<AddRequestDocumentsCommand>(async c => await c.ExecuteAsync(requestDocuments))
                .AddCommand<AddContractDocumentsCommand>(async c => await c.ExecuteAsync(contractDocuments))
                .AddCommand<AddProtectionDocDocumentsCommand>(async c => await c.ExecuteAsync(protectionDocDocuments))
                .AddCommand<ApplyDocumentWorkflowCommand>(async c => await c.ExecuteAsync(initialWorkflow))
                .AddCommand<CreateUserInputCommand>(async c => await c.ExecuteAsync(documentId, userInputDto))
                .ExecuteAllWithTransaction();
        }

        private async Task UpdateOwners(int id, MaterialDetailDto detail)
        {
            var requestDocumentDtos = Executor.GetHandler<FilterOwnersByOwnerTypeHandler>()
                .Process(h => h.Execute(Owner.Type.Request, detail.Owners.ToList()));
            var contractDocumentDtos = Executor.GetHandler<FilterOwnersByOwnerTypeHandler>()
                .Process(h => h.Execute(Owner.Type.Contract, detail.Owners.ToList()));
            var protectionDocDocumentDtos = Executor.GetHandler<FilterOwnersByOwnerTypeHandler>()
                .Process(h => h.Execute(Owner.Type.ProtectionDoc, detail.Owners.ToList()));

            var updatedDocument = Executor.GetQuery<GetDocumentWithIncludesByDocumentIdQuery>()
                .Process(q => q.Execute(id));

            var requestDocumentsToAdd = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h =>
                    h.Execute(requestDocumentDtos.Select(r => r.OwnerId).ToList(),
                        updatedDocument.Requests.Select(r => r.RequestId).ToList()))
                .Select(requestId => new RequestDocument { DocumentId = id, RequestId = requestId }).ToList();
            var contractDocumentsToAdd = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h =>
                    h.Execute(contractDocumentDtos.Select(r => r.OwnerId).ToList(),
                        updatedDocument.Contracts.Select(r => r.ContractId).ToList()))
                .Select(contractId => new ContractDocument { DocumentId = id, ContractId = contractId })
                .ToList();
            var protectionDocDocumentsToAdd = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h =>
                    h.Execute(protectionDocDocumentDtos.Select(r => r.OwnerId).ToList(),
                        updatedDocument.ProtectionDocs.Select(r => r.ProtectionDocId).ToList()))
                .Select(protectionDocId =>
                    new ProtectionDocDocument { DocumentId = id, ProtectionDocId = protectionDocId }).ToList();

            var requestDocumentIdsToRemove = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h =>
                h.Execute(requestDocumentDtos.Select(r => r.OwnerId).ToList(),
                    updatedDocument.Requests.Select(r => r.RequestId).ToList()));
            var requestDocumentsToRemove = updatedDocument.Requests
                .Where(rd => requestDocumentIdsToRemove.Contains(rd.RequestId))
                .Select(rd => new RequestDocument { Id = rd.Id, DocumentId = id, RequestId = rd.RequestId }).ToList();

            var contractDocumentIdsToRemove = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h =>
                h.Execute(contractDocumentDtos.Select(r => r.OwnerId).ToList(),
                    updatedDocument.Contracts.Select(r => r.ContractId).ToList()));
            var contractDocumentsToRemove = updatedDocument.Contracts
                .Where(cd => contractDocumentIdsToRemove.Contains(cd.ContractId))
                .Select(cd => new ContractDocument { Id = cd.Id, DocumentId = id, ContractId = cd.ContractId })
                .ToList();

            var protectionDocDocumentIdsToRemove = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>().Process(h =>
                h.Execute(protectionDocDocumentDtos.Select(r => r.OwnerId).ToList(),
                    updatedDocument.ProtectionDocs.Select(r => r.ProtectionDocId).ToList()));
            var protectionDocDocumentsToRemove = updatedDocument.ProtectionDocs
                .Where(cd => protectionDocDocumentIdsToRemove.Contains(cd.ProtectionDocId))
                .Select(pdd =>
                    new ProtectionDocDocument { Id = pdd.Id, DocumentId = id, ProtectionDocId = pdd.ProtectionDocId })
                .ToList();

            Executor.CommandChain()
                .AddCommand<AddRequestDocumentsCommand>(async c => await c.ExecuteAsync(requestDocumentsToAdd))
                .AddCommand<AddContractDocumentsCommand>(async c => await c.ExecuteAsync(contractDocumentsToAdd))
                .AddCommand<AddProtectionDocDocumentsCommand>(async c => await c.ExecuteAsync(protectionDocDocumentsToAdd))
                .AddCommand<RemoveRequestDocumentsCommand>(async c => await c.ExecuteAsync(requestDocumentsToRemove))
                .AddCommand<RemoveContractDocumentsCommand>(async c => await c.ExecuteAsync(contractDocumentsToRemove))
                .AddCommand<RemoveProtectionDocDocumentsCommand>(async c => await c.ExecuteAsync(protectionDocDocumentsToRemove))
                //TODO: ExecuteAllWithTransaction with Async
                .ExecuteAllWithTransaction();
        }

        private async Task UpdateApartmentInDicCustomer(int customerId, string newApartment, string newShortAddress, ContactInfoDto[] contactInfos)
        {
            var addressee = Executor.GetQuery<GetDicCustomerByIdQuery>().Process(q => q.Execute(customerId));
            addressee.Apartment = newApartment;
            addressee.ShortAddress = newShortAddress;
            addressee.ContactInfos = Mapper.Map<List<ContactInfo>>(contactInfos);
            _customerUpdater.Update(addressee);
            //await Executor.GetCommand<UpdateDicCustomerCommand>().Process(c => c.ExecuteAsync(addressee));
        }


        /// <summary>
        /// Метод, который возвращает пагинированный список документов
        /// по идентификатору пользователя.
        /// <para></para>
        /// Внимание, документы мапятся на <see cref="MaterialTaskDto"/>.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Список документов.</returns>
        private IActionResult GetListByUserId(int userId)
        {
            IQueryable<Document> query = Executor.GetQuery<GetRequestDocumentsQuery>().Process(r => r.Execute());
            IQueryCollection queryCollection = Request.Query;

            query = FilterByIncomingNumber(query, queryCollection);
            query = FilterByOutgoingNumber(query, queryCollection);
            query = FilterByBarcode(query, queryCollection);
            query = FilterByCreateDateFrom(query, queryCollection);
            query = FilterByCreateDateTo(query, queryCollection);
            query = FilterByDocumentType(query, queryCollection);
            query = FilterByTypeId(query, queryCollection);

            var curentUserId = userId;

            var excludeTypes = new[]
            {
                DicDocumentTypeCodes.RequestForInvention,
                DicDocumentTypeCodes._001_001B_PCT,
                DicDocumentTypeCodes._001_001B_EAPO,
                DicDocumentTypeCodes._001_001C,
                DicDocumentTypeCodes.RequestForInternationalTrademark,
                DicDocumentTypeCodes.RequestForNmpt,
                DicDocumentTypeCodes.RequestForIndustrialSample,
                DicDocumentTypeCodes.RequestForUsefulModel,
                DicDocumentTypeCodes.A,
                DicDocumentTypeCodes._001_001E,
                DicDocumentTypeCodes._001_001A_1,
                DicDocumentTypeCodes.RequestForSelectiveAchievement,
                DicDocumentTypeCodes.RequestForTrademark,
                DicDocumentTypeCodes.Statement,
                DicDocumentTypeCodes._001_001_1B_IN,
                DicDocumentTypeCodes.AP_ZAYAVLENIE_SCAN,
                DicDocumentTypeCodes.StatementInventions,
                DicDocumentTypeCodes.StatementNamePlaces,
                DicDocumentTypeCodes.StatementUsefulModels,
                DicDocumentTypeCodes.StatementIndustrialDesigns,
                DicDocumentTypeCodes.StatementSelectiveAchievs,
                DicDocumentTypeCodes.StatementTrademark,
                DicDocumentTypeCodes.TIM_ZAYAVLENIE_SCAN
            };

            var materialsResult = (from q in query
                                   from w in q.Workflows
                                        where w.IsCurent 
                                              && w.CurrentUserId == curentUserId 
                                              && q.Status.Code != DicDocumentStatusCodes.Completed 
                                              && !excludeTypes.Contains(q.Type.Code) 
                                   select q)
                .Sort(Request.Query)
                .ProjectTo<MaterialTaskDto>()
                .ToPagedList(Request.GetPaginationParams());
            


            foreach (var materials in materialsResult.Items)
            {
                var document = query.FirstOrDefault(d => d.Id == materials.Id);

                if (document == null)
                {
                    materials.Priority = TaskPriority.Normal;
                    continue;
                }

                var commentDtos = _mapper.Map<ICollection<DocumentComment>, List<DocumentCommentDto>>(document.Comments);
                materials.CommentDtos = commentDtos;

                materials.CurrentStageUser = document.Workflows.FirstOrDefault(d => d.IsCurent)?.CurrentUser?.NameRu;
                materials.CurrentStageUserId = document.Workflows.FirstOrDefault(d => d.IsCurent)?.CurrentUser?.Id;

                materials.IsReadOnly = document.CurrentWorkflows.All(cwf => cwf.CurrentUserId != curentUserId);

                if (!document.CurrentWorkflows.Any()) continue;
                var wf = document.CurrentWorkflows.FirstOrDefault(d => d.CurrentUserId == curentUserId);

                if (wf?.CurrentStageId == null) continue;

                //Логика из MaterialProfile для вывора именно текущего пользователя и его WF
                materials.CurrentStageUser = wf.CurrentUserId == curentUserId ? wf.CurrentUser.NameRu : string.Empty;
                materials.CurrentStageUserId = wf.CurrentUserId == curentUserId ? wf.CurrentUser.Id : 0;

                var stage = Executor.GetQuery<GetDicRouteStageByIdQuery>().Process(q => q.Execute(wf.CurrentStageId.Value));
                if (stage.ExpirationValue == null) continue;

                var stageExpiration = Executor.GetQuery<GetDicStageExpiration>().Process(q => q.Execute(stage.Id, document.TypeId));

                var deadLineTime = _calendarProvider.GetExecutionDate(wf.DateCreate, stage.ExpirationType, stage.ExpirationValue.Value);
                var deadLine = new DateTimeOffset(deadLineTime.Year, deadLineTime.Month, deadLineTime.Day, 0, 0, 0, new TimeSpan());
                if (stageExpiration != null)
                {
                    deadLine = _calendarProvider.GetExecutionDate(wf.DateCreate, stageExpiration.ExpirationType, stageExpiration.ExpirationValue);
                }

                var differenceInDays = (deadLine - new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 0, 0, 0, new TimeSpan())).Days;
                if (differenceInDays <= 3)
                {
                    materials.Priority = TaskPriority.Yellow;
                }
                if (differenceInDays <= 0)
                {
                    materials.Priority = TaskPriority.Red;
                }
            }

            return materialsResult.AsOkObjectResult(Response);
        }

        /// <summary>
        /// Фильтрует документы по входящему номеру.
        /// </summary>
        /// <param name="query">Запрос к <see cref="Document"/>.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<Document> FilterByIncomingNumber(IQueryable<Document> query,
            IQueryCollection queryCollection)
        {
            const string IncomingNumberKey = "incomingNumber_eq";

            if (queryCollection.ContainsKey(IncomingNumberKey)
                && queryCollection.TryGetValue(IncomingNumberKey, out StringValues incomingNumberValue))
            {
                string incomingNumber = incomingNumberValue.ToString();

                if (!string.IsNullOrWhiteSpace(incomingNumber))
                {
                    query = query.Where(document => document.IncomingNumber.Contains(incomingNumber));
                }
            }

            return query;
        }

        /// <summary>
        /// Фильтрует документы по исходящему номеру.
        /// </summary>
        /// <param name="query">Запрос к <see cref="Document"/>.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<Document> FilterByOutgoingNumber(IQueryable<Document> query,
            IQueryCollection queryCollection)
        {
            const string OutgoingNumberKey = "outgoingNumber_eq";

            if (queryCollection.ContainsKey(OutgoingNumberKey)
                && queryCollection.TryGetValue(OutgoingNumberKey, out StringValues outgoingNumberValue))
            {
                string outgoingNumber = outgoingNumberValue.ToString();

                if (!string.IsNullOrWhiteSpace(outgoingNumber))
                {
                    query = query.Where(document => document.OutgoingNumber.Contains(outgoingNumber));
                }
            }

            return query;
        }

        /// <summary>
        /// Фильтрует документы по штрихкоду.
        /// </summary>
        /// <param name="query">Запрос к <see cref="Document"/>.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<Document> FilterByBarcode(IQueryable<Document> query,
            IQueryCollection queryCollection)
        {
            const string BarcodeKey = "barcode_eq";

            if (queryCollection.ContainsKey(BarcodeKey)
                && queryCollection.TryGetValue(BarcodeKey, out StringValues barcodeValue))
            {
                string barcode = barcodeValue.FirstOrDefault();

                query = query.Where(document => document.Barcode.ToString().Contains(barcode));
            }

            return query;
        }

        /// <summary>
        /// Фильтрует документы по дате создания, задавая дату, с которой идет выборка.
        /// </summary>
        /// <param name="query">Запрос к <see cref="Document"/>.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<Document> FilterByCreateDateFrom(IQueryable<Document> query,
            IQueryCollection queryCollection)
        {
            const string CreateDateFromKey = "createDate_from";

            if (queryCollection.ContainsKey(CreateDateFromKey)
                && queryCollection.TryGetValue(CreateDateFromKey, out StringValues createDateFromValue))
            {
                DateTimeOffset.TryParse(createDateFromValue.FirstOrDefault(),
                    out DateTimeOffset createDateFrom);

                query = query.Where(document => document.DateCreate >= createDateFrom);
            }

            return query;
        }

        /// <summary>
        /// Фильтрует документы по дате создания, задавая дату, до которой идет выборка.
        /// </summary>
        /// <param name="query">Запрос к <see cref="Document"/>.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<Document> FilterByCreateDateTo(IQueryable<Document> query,
            IQueryCollection queryCollection)
        {
            const string CreateDateToKey = "createDate_to";

            if (queryCollection.ContainsKey(CreateDateToKey)
                && queryCollection.TryGetValue(CreateDateToKey, out StringValues createDateToValue))
            {
                DateTimeOffset.TryParse(createDateToValue.FirstOrDefault(),
                    out DateTimeOffset createDateTo);

                query = query.Where(document => document.DateCreate <= createDateTo);
            }

            return query;
        }

        /// <summary>
        /// Фильтрует документы по типу.
        /// </summary>
        /// <param name="query">Запрос к <see cref="Document"/>.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<Document> FilterByDocumentType(IQueryable<Document> query,
            IQueryCollection queryCollection)
        {
            const string DocumentTypeKey = "documentType_eq";

            if (queryCollection.ContainsKey(DocumentTypeKey))
            {
                if (queryCollection.TryGetValue(DocumentTypeKey, out StringValues documentTypeValue))
                {
                    int.TryParse(documentTypeValue.FirstOrDefault(),
                        out int documentTypeInt);

                    DocumentType documentType
                        = (DocumentType)Enum.ToObject(typeof(DocumentType), documentTypeInt);

                    query = query.Where(document => document.DocumentType == documentType);
                }
            }

            return query;
        }

        /// <summary>
        /// Фильтрует документы по идентификатору типа.
        /// </summary>
        /// <param name="query">Запрос к <see cref="Document"/>.</param>
        /// <param name="queryCollection">Коллекция значений для фильтрации.</param>
        /// <returns>Отфильтрованный запрос.</returns>
        private IQueryable<Document> FilterByTypeId(IQueryable<Document> query,
            IQueryCollection queryCollection)
        {
            const string TypeIdKey = "typeId_eq";

            if (queryCollection.ContainsKey(TypeIdKey)
                && queryCollection.TryGetValue(TypeIdKey, out StringValues typeIdValue))
            {
                int.TryParse(typeIdValue.FirstOrDefault(), out int typeId);

                query = query.Where(document => document.TypeId == typeId);
            }

            return query;
        }
    }
}