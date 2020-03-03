using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.DI;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Model.Models.Material;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Newtonsoft.Json;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.BusinessLogic.DocumentUserInput;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Infrastructure.Helpers;
using Iserv.Niis.Utils.Constans;
using Iserv.Niis.Common.Codes;
using System.IO;
using Aspose.Pdf;
using Iserv.Niis.BusinessLogic.Attachments;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Intergrations;
using Iserv.Niis.Exceptions;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.Utils.Helpers;
using Document = Aspose.Pdf.Document;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Documents")]
    public class DocumentsController : Controller
    {
        private readonly ILkIntergarionHelper _lkIntergarionHelper;
        private readonly IDocumentGeneratorFactory _templateGeneratorFactory;
        private readonly ITemplateUserInputChecker _templateUserInputChecker;
        private readonly IDocumentsCompare _documentsCompare;
        private readonly IFileStorage _fileStorage;
        private readonly IFileConverter _fileConverter;
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;

        public DocumentsController(
             IExecutor executor,
             IMapper mapper,
             IDocumentGeneratorFactory templateGeneratorFactory,
             ITemplateUserInputChecker templateUserInputChecker,
             IFileStorage fileStorage,
             IAttachmentHelper attachmentHelper,
             IDocumentsCompare documentsCompare, 
             IFileConverter fileConverter,
             ILkIntergarionHelper lkIntergarionHelper)
        {
            _executor = executor;
            _mapper = mapper;
            _templateGeneratorFactory = templateGeneratorFactory;
            _templateUserInputChecker = templateUserInputChecker;
            _documentsCompare = documentsCompare;
            _fileConverter = fileConverter;
            _fileStorage = fileStorage;
            _lkIntergarionHelper = lkIntergarionHelper;
        }

        //TODO: check access
        [HttpGet("{id}/{wasScanned}/{isMain}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id, bool wasScanned, bool isMain)
        {
            byte[] file;
            var contentType = string.Empty;
            var validName = string.Empty;
            //todo: Почему не работает ProcessAsync?
            var document = await _executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(id));

            if (document == null)
                throw new DataNotFoundException(nameof(Domain.Entities.Document.Document), DataNotFoundException.OperationType.Read, id);

            var input = await _executor.GetQuery<GetDocumentUserInputByDocumentIdQuery>().Process(q => q.ExecuteAsync(document.Id));

            if (input != null && !wasScanned)
            {
                var dto = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);

                var documentGenerator = _templateGeneratorFactory.Create(dto.Code);
                var generatedFile = documentGenerator.Process(new Dictionary<string, object>
                    {
                        {"UserId",NiisAmbientContext.Current.User.Identity.UserId},
                        {"RequestId", dto.OwnerId},
                        {"DocumentId", id},
                        {"UserInputFields", dto.Fields},
                        {"SelectedRequestIds", dto.SelectedRequestIds },
                        {"PageCount", dto.PageCount},
                        {"OwnerType", dto.OwnerType},
                        {"Index", dto.Index }
                    });
                file = generatedFile.File;
                contentType = ContentType.Pdf;
                var typeNameRu = document?.Type?.NameRu;

                if (document.Type.Code == DicDocumentTypeCodes.NotificationOfRegistrationDecision && !string.IsNullOrEmpty(document.OutgoingNumber))
                {
                    var documentLinksIds = new List<int>();
                    documentLinksIds = document.DocumentLinks.Where(d => d.ChildDocument.Type.Code == DicDocumentTypeCodes.ExpertTmRegisterOpinion).Select(d => d.ChildDocumentId).ToList();
                    documentLinksIds.AddRange(document.DocumentParentLinks.Where(d => d.ParentDocument.Type.Code == DicDocumentTypeCodes.ExpertTmRegisterOpinion).Select(d => d.ParentDocumentId).ToList().Where(d => !documentLinksIds.Contains(d)));

                    var linkFiles = new List<byte[]>();
                    linkFiles.Add(file);

                    foreach (var documentLinksId in documentLinksIds)
                    {
                        var linkFile = await GetFileByte(documentLinksId);
                        if (linkFile == null) continue;
                        else linkFiles.Add(linkFile);
                    }

                    if (linkFiles.Count > 0)
                    {
                        var result = MergeFile(linkFiles);

                        return string.IsNullOrWhiteSpace(typeNameRu)
                           ? File(result, contentType)
                           : File(result, contentType, typeNameRu);
                    }
                }

                return string.IsNullOrWhiteSpace(typeNameRu)
                    ? File(file, contentType)
                    : File(file, contentType, typeNameRu);
            }
            
            Attachment attachment = null;
            if (isMain && document.MainAttachment != null)
            {
                attachment = document.MainAttachment;
                contentType = document.MainAttachment.ContentType;
                validName = document.MainAttachment.ValidName;
            }
            else if (document.AdditionalAttachments.Any(d => d.IsMain == false))
            {
                attachment = document.AdditionalAttachments.FirstOrDefault(d => d.IsMain == false);
                contentType = attachment?.ContentType;
                validName = attachment?.ValidName;
            }

            if (attachment != null)
            {
                var fileContent = await _fileStorage.GetAsync(attachment.BucketName, attachment.OriginalName);

                var extention = Path.GetExtension(attachment.OriginalName);

                if (extention != null && extention.ToLower().Contains("odt"))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        memoryStream.Write(fileContent, 0, fileContent.Length);
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var newName = validName.Replace(".odt", ".pdf");
                        var pdf = _fileConverter.DocxToPdf(memoryStream, newName);
                        return File(pdf.File, contentType, newName);
                    }
                }
                
                file = fileContent;
                return File(file, contentType, validName);
            }

            return null;
        }
        //TODO: Много контроллеров пу сути не являются асинхронными, не используйте где попало асинхронность, это вводит в заблуждение!!!

        private async Task<byte[]> GetFileByte(int documentLinksId)
        {
            byte[] file;
            var document = await _executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentLinksId));

            if (document == null)
                return null;

            var input = await _executor.GetQuery<GetDocumentUserInputByDocumentIdQuery>().Process(q => q.ExecuteAsync(document.Id));
            if (input != null)
            {
                var dto = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);

                var documentGenerator = _templateGeneratorFactory.Create(dto.Code);
                var generatedFile = documentGenerator.Process(new Dictionary<string, object>
                    {
                        {"UserId",NiisAmbientContext.Current.User.Identity.UserId},
                        {"RequestId", dto.OwnerId},
                        {"DocumentId", documentLinksId},
                        {"UserInputFields", dto.Fields},
                        {"SelectedRequestIds", dto.SelectedRequestIds },
                        {"PageCount", dto.PageCount},
                        {"OwnerType", dto.OwnerType},
                        {"Index", dto.Index }
                    });
                file = generatedFile.File;
                return file;
            }
            return null;
        }

        private byte[] MergeFile(IList<byte[]> files)
        {
            var mergedDocument = new Document();
            var streams = new List<MemoryStream>();

            foreach (var fileInfo in files)
            {
                var stream = new MemoryStream(fileInfo);
                var document = new Document(stream);

                for (var page = 1; page <= document.Pages.Count; page++)
                {
                    mergedDocument.Pages.Add(document.Pages[page]);
                }
                streams.Add(stream);
            }

            //// Save merged document.
            using (MemoryStream output = new MemoryStream())
            {
                mergedDocument.Save(output, SaveFormat.Pdf);


                foreach (var stream in streams)
                {
                    stream.Close();
                    stream.Dispose();
                }

                return output.ToArray();
            }
        }

        [HttpGet("availableTypes/{id}")]
        public async Task<IActionResult> AvailableTypes(int id)
        {
            //TODO: не используется ID 
            var documentTypes = _executor.GetQuery<GetDicDocumentTypeWithFileTemplate>().Process(q => q.Execute());
            var result = documentTypes.ProjectTo<SelectOptionDto>();

            return Ok(result);
        }

        [HttpGet("getUserInputFields/{code}")]
        public async Task<IActionResult> GetUserInputFields(string code)
        {
            _templateUserInputChecker.GetConfig(code, out var config);

            return Ok(config);
        }

        [HttpGet("getDocumetsInfoForCompare/{requestId}")]
        public async Task<IActionResult> GetDocumetsInfo(int requestId)
        {
            var compareDocument = await _documentsCompare.GetDocumentsInfoForCompare(requestId);

            return Ok(compareDocument);
        }

        [HttpGet("makeDocumentFinished/{documentId}")]
        public async Task<IActionResult> MakeDocumentFinished(int documentId)
        {
            await _executor.GetCommand<UpdateDocumentSetFinishedFlagCommand>().Process(r=>r.ExecuteAsync(documentId));
            
            return NoContent();
        }

        [HttpGet("types/bycode/{code}")]
        public async Task<IActionResult> GetDictionariesByClassificationCode(string code)
        {
            var documentTypes = await _executor.GetQuery<GetDicDocumentTypeByClassificationCodeQuery>().Process(r => r.ExecuteAsync(code));
            //var typesDto = _mapper.Map<List<SelectOptionDto>>(documentTypes);

            return Ok(documentTypes.Where(d => d.IsDeleted != true));
        }

        [HttpGet("checkIcgs/{requestId}")]
        public async Task<IActionResult> GetAreRequestIcgsPaidFor(int requestId)
        {
            var result = await _executor.GetHandler<IsIcgspaidForHandler>().Process(h => h.Execute(requestId));

            return Ok(result);
        }

        /// <summary>
        /// Отправка переписки
        /// </summary>
        /// <param name="barcode">Штрихкод документа</param>
        /// <param name="isFromRequest">Привязка к заявке</param>
        /// <returns>Статус запроса</returns>
        [HttpPost("sendMessage/{barcode}/{isFromRequest}")]
        [AllowAnonymous]
        public async Task<IActionResult> SendMessage(int barcode, bool isFromRequest)
        {
            int? protectionDocTypeId = null;
            int? docBarcode = null;
            string file = string.Empty;
            string typeNameRu = string.Empty;
            Attachment attachment = new Attachment();

            var document = await _executor.GetQuery<GetDocumentByBarcodeQuery>()
                .Process(d => d.ExecuteAsync(barcode));

            int? answerDocId = null;

            if (document.IncomingAnswerId.HasValue)
            {
                var documentAnswer = await _executor.GetQuery<GetDocumentByIdQuery>().Process(d => d.ExecuteAsync(document.IncomingAnswerId.Value));
                answerDocId = documentAnswer.Barcode;
            }

            var docType = document.Type.ExternalId ?? document.TypeId;


            if (document.Requests.Count != 0)
            { 
                var request = await _executor.GetQuery<GetRequestByDocumentIdQuery>().Process(d => d.ExecuteAsync(document.Id));

                if (isFromRequest && request == null)
                {
                    throw new NotSupportedException("Прикрепленная заявка не найдена");
                }

                protectionDocTypeId = request?.ProtectionDocType.ExternalId ?? request?.ProtectionDocType.Id;
                docBarcode = request?.Barcode;
            }

            if (document.Contracts.Count != 0)
            {
                var contract = await _executor.GetQuery<GetContractByDocumentIdQuery>().Process(d => d.ExecuteAsync(document.Id));

                if (isFromRequest && contract == null)
                {
                    throw new NotSupportedException("Прикрепленный договор не найден");
                }

                protectionDocTypeId = contract?.ProtectionDocType.ExternalId ?? contract?.ProtectionDocType.Id;
                docBarcode = contract?.Barcode;
            }

            if (document.ProtectionDocs.Count != 0)
            {
                var protectionDoc = await _executor.GetQuery<GetProtectionDocByDocumentIdQuery>().Process(d => d.ExecuteAsync(document.Id));

                if (isFromRequest && protectionDoc == null)
                {
                    throw new NotSupportedException("Прикрепленный договор не найден");
                }

                protectionDocTypeId = protectionDoc?.Request.ProtectionDocType.ExternalId ?? protectionDoc?.Request.ProtectionDocType.Id;
                docBarcode = protectionDoc?.Request?.Barcode;
            }

            if (!document.MainAttachmentId.HasValue && document.WasScanned)
            {
                throw new NotSupportedException("Файл у документа не найден");
            }

            if (document.MainAttachmentId.HasValue)
            {
                attachment = await _executor.GetQuery<GetAttachmentQuery>().Process(d => d.ExecuteAsync(document.MainAttachmentId.Value));
                var bytes = await _executor.GetQuery<GetAttachmentFileQuery>().Process(d => d.Execute(attachment.Id));
                file = Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
            }

            var input = await _executor.GetQuery<GetDocumentUserInputByDocumentIdQuery>().Process(q => q.ExecuteAsync(document.Id));
            if (input != null && !document.WasScanned && !document.MainAttachmentId.HasValue)
            {
                var dto = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);

                var documentGenerator = _templateGeneratorFactory.Create(dto.Code);
                var generatedFile = documentGenerator.Process(new Dictionary<string, object>
                {
                    {"UserId", NiisAmbientContext.Current.User.Identity.UserId},
                    {"RequestId", dto.OwnerId},
                    {"DocumentId", document.Id},
                    {"UserInputFields", dto.Fields},
                    {"SelectedRequestIds", dto.SelectedRequestIds},
                    {"PageCount", dto.PageCount},
                    {"OwnerType", dto.OwnerType},
                    {"Index", dto.Index}
                });
                var bytes = generatedFile.File;
                file = Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
                typeNameRu = document?.Type?.NameRu + ".pdf";
            }

            

            var sendMessageBody = new SendMessageBody
            {
                Input = new SendMessage
                {
                    DocumentId = docBarcode,
                    PatentTypeId = protectionDocTypeId,
                    AnswerDocId = answerDocId,
                    MessageInfo = new MessageInfo
                    {
                        Id = docType,
                        DocumentId = document.Barcode,
                        DocNumber = document.OutgoingNumber,
                        DocDate = document.DateCreate.ToString("dd-MM-yyyy")
                    },
                    File = new Domain.Intergrations.File
                    {
                        Name = attachment?.ValidName ?? typeNameRu,
                        Content = file
                    }
                }
            };

            var result = _lkIntergarionHelper.CallWebService(sendMessageBody, SoapActions.SendMessage);
            return Ok(result);
        }
    }
}