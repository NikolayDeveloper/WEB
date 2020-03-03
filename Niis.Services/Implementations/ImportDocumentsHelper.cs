using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.BusinessLogic.Attachments;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.DocumentUserSignature;
using Iserv.Niis.BusinessLogic.Workflows.Documents;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Domain.OldNiisEntities;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Services.Dapper;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Utils.Abstractions;
using Iserv.Niis.Utils.Constans;
using Iserv.Niis.Utils.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Microsoft.Extensions.Configuration;
using CreateDocumentCommand = Iserv.Niis.BusinessLogic.Documents.CreateDocumentCommand;
using Document = Iserv.Niis.Domain.Entities.Document.Document;
using UpdateDocumentCommand = Iserv.Niis.WorkflowBusinessLogic.Document.UpdateDocumentCommand;

namespace Iserv.Niis.Services.Implementations
{
    public class ImportDocumentsHelper : BaseImportHelper, IImportDocumentsHelper
    {
        #region Constructor

        private readonly IGenerateHash _generateHash;
        private readonly IFileStorage _fileStorage;

        public ImportDocumentsHelper(
            IConfiguration configuration, 
            DictionaryHelper dictionaryHelper, 
            IFileStorage fileStorage,
            IGenerateHash generateHash) 
            : base(configuration, dictionaryHelper)
        {
            _fileStorage = fileStorage;
            _generateHash = generateHash;

            SavedDocuments = new List<Document>();
        }
        
        public const int DeveloperUserId = 1;

        private List<Document> SavedDocuments { get; }

        #endregion
        
        public async Task ImportFromDb(string number, int requestId)
        {
            var refSqlQuey = string.Format(ImportSqlQueryHelper.RefSqlQuery, string.Join(", ", ObjectType.GetRequestRouteIds()), string.Join(", ", ObjectType.GetMaterilsRouteIds()), number);
            var oldRefDocuments = await SqlDapperConnection.QueryAsync<RfMessageDocument>(refSqlQuey, TargetConnectionString);
            foreach (var oldRefDocument in oldRefDocuments)
            {
                var refDocument = CreateRefDocument(oldRefDocument, requestId);
                if (refDocument == null) continue;

                var oldDocumentId = refDocument.DocumentId;

                //Document
                var newDocumentId = await FillDocument(refDocument.DocumentId);
                if (newDocumentId == 0) continue;

                refDocument.DocumentId = newDocumentId;
                Executor.GetCommand<CreateRequestDocumentCommand>().Process(d => d.Execute(refDocument));

                refDocument.DateCreate = new DateTimeOffset(oldRefDocument.DateCreate.GetValueOrDefault(DateTime.Now));

                Executor.GetCommand<UpdateRequestDocumentCommand>().Process(d => d.Execute(refDocument));

                await FillDocRefs(oldDocumentId, newDocumentId);
            }
        }

        public async Task ImportContractFromDb(int oldContractId, int newContractId)
        {
            TargetAttachmentConnectionString = Configuration.GetConnectionString("NiisDesctopAttachmentsConnection");
            TargetConnectionString = Configuration.GetConnectionString("NiisDesctopConnection");
            SourceConnectionString = Configuration.GetConnectionString("DefaultConnection");

            var refSqlQuey = string.Format(ImportSqlQueryHelper.ContractDocRefSqlQuery, string.Join(", ", ObjectType.GetContractRouteIds()), string.Join(", ", ObjectType.GetMaterilsRouteIds()), oldContractId);
            var oldRefDocuments = await SqlDapperConnection.QueryAsync<RfMessageDocument>(refSqlQuey, TargetConnectionString);
            foreach (var oldRefDocument in oldRefDocuments)
            {
                var refDocument = CreateContractRefDocument(oldRefDocument, newContractId);
                if (refDocument == null) continue;

                var oldDocumentId = refDocument.DocumentId;

                //Document
                var newDocumentId = await FillDocument(refDocument.DocumentId);
                if (newDocumentId == 0) continue;

                refDocument.DocumentId = newDocumentId;
                Executor.GetCommand<CreateContractDocumentCommand>().Process(d => d.Execute(refDocument));

                await FillDocRefs(oldDocumentId, newDocumentId);
            }
        }

        private async Task FillDocRefs(int oldDocumentId, int newDocumentId)
        {
            //DocumentWorkflow
            await FillDocumentWorkflows(oldDocumentId, newDocumentId);

            //UserSignature
            await FillDocumentUserSignatures(oldDocumentId);

            //Attachments
            await FillAttachments(oldDocumentId, newDocumentId);

            try
            {
                ////CurrentWorkflow
                //var currentWorkflowSqlQuery =
                //    string.Format(ImportSqlQueryHelper.DocumentsCurrentWorkflowSqlQuery, newDocumentId);
                //await SqlDapperConnection.ExecuteAsync(currentWorkflowSqlQuery, SourceConnectionString);

                //MainAttachment
                var setMainAttachmentSqlQuery =
                    string.Format(ImportSqlQueryHelper.SetDocumentsMainAttachmentSqlQuery, newDocumentId);
                await SqlDapperConnection.ExecuteAsync(setMainAttachmentSqlQuery, SourceConnectionString);

                //Status
                var setStatusSqlQuery =
                    string.Format(ImportSqlQueryHelper.DocumentsSetStatusSqlQuery, newDocumentId);
                await SqlDapperConnection.ExecuteAsync(setStatusSqlQuery, SourceConnectionString);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #region RefDocument

        private RequestDocument CreateRefDocument(RfMessageDocument oldRefDocument, int requestId)
        {
            try
            {
                var requestDocument = new RequestDocument
                {
                    DateCreate = new DateTimeOffset(oldRefDocument.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldRefDocument.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DocumentId = oldRefDocument.RefdocumentId,
                    ExternalId = oldRefDocument.Id,
                    RequestId = requestId,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks)
                };

                return requestDocument;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ContractDocument CreateContractRefDocument(RfMessageDocument oldRefDocument, int contractId)
        {
            try
            {
                var contractDocument = new ContractDocument
                {
                    DateCreate = new DateTimeOffset(oldRefDocument.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldRefDocument.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DocumentId = oldRefDocument.RefdocumentId,
                    ExternalId = oldRefDocument.Id,
                    ContractId = contractId,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks)
                };

                return contractDocument;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        #endregion

        #region Documen

        private async Task<int> FillDocument(int refDocumentId)
        {
            try
            {
                var sqlQuery = string.Format(ImportSqlQueryHelper.DocumentSqlQuery, string.Join(", ", ObjectType.GetMaterilsRouteIds()), refDocumentId);
                var oldDocuments = await SqlDapperConnection.QueryAsync<DdDocumentExtension>(sqlQuery, TargetConnectionString);
                var ddDocuments = oldDocuments.ToList();
                if (!ddDocuments.Any()) return 0;
                var document = await CreateDocument(ddDocuments.FirstOrDefault());
                var newDocumentId = await Executor.GetCommand<CreateDocumentCommand>().Process(d => d.ExecuteAsync(document));

                document.DateCreate = new DateTimeOffset(ddDocuments.First().DateCreate.GetValueOrDefault(DateTime.Now));

                Executor.GetCommand<UpdateDocumentCommand>().Process(d => d.Execute(document));

                document.Id = newDocumentId;
                SavedDocuments.Add(document);

                return newDocumentId;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private async Task<Document> CreateDocument(DdDocumentExtension oldDocument)
        {
            var typeId = DictionaryHelper.GetNullableDictionaryIdByCode(nameof(DicDocumentType), oldDocument.DoctypeCode) ??
                         DictionaryHelper.GetNullableDictionaryIdByExternalId(nameof(DicDocumentType), oldDocument.DoctypeId);

            if (typeId == null || typeId == 0) return null;

            var type = DictionaryHelper.GetDictionaryById(nameof(DicDocumentType), typeId.Value);
            var route = DictionaryHelper.GetDictionaryById(nameof(DicRoute), type.RouteId);

            var documentType = GenerateHelper.GetDocumentType(route.Code);

            var customerId = GetObjectId<DicCustomer>(oldDocument.CustomerId);
            if ((customerId == null || customerId == 0) && oldDocument.CustomerId.HasValue)
                customerId = await GetCustomer(oldDocument.CustomerId.Value);

            var document = new Document
            {
                AddresseeId = customerId,
                Barcode = oldDocument.Id,
                DateCreate = new DateTimeOffset(oldDocument.DateCreate.GetValueOrDefault(DateTime.Now)),
                DateUpdate = new DateTimeOffset(oldDocument.Stamp.GetValueOrDefault(DateTime.Now)),
                DepartmentId = GetObjectId<DicDepartment>(oldDocument.DepartmentId),
                DivisionId = GetObjectId<DicDivision>(oldDocument.DivisionId),
                DocumentNum = oldDocument.DocumNum,
                DocumentType = documentType,
                ExternalId = oldDocument.Id,
                IncomingNumber = oldDocument.InoutNum,
                IncomingNumberFilial = oldDocument.InnumAdd,
                IsDeleted = false,
                IsFinished = GenerateHelper.StringToNullableBool(oldDocument.IsComplete),
                NameEn = oldDocument.DescriptionMlEn,
                NameRu = oldDocument.DescriptionMlRu,
                NameKz = oldDocument.DescriptionMlKz,
                OutgoingNumber = oldDocument.Outnum,
                ReceiveTypeId = GetObjectId<DicReceiveType>(oldDocument.SendType),
                SendingDate = GetNullableDate(oldDocument.DocumDate),
                Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                TypeId = typeId.Value,
                PageCount = oldDocument.PageCount,
                WasScanned = false,
                StatusId = DictionaryHelper.GetDictionaryIdByCode(nameof(DicDocumentStatus), DicDocumentStatusCodes.Completed)
            };

            return document;
        }

        #endregion

        #region DocumentWorkflows

        private async Task FillDocumentWorkflows(int oldDocuemntId, int newDocumentId)
        {
            var wfSqlQuery = string.Format(ImportSqlQueryHelper.DocumentsWfSqlQuery, string.Join(", ", ObjectType.GetMaterilsRouteIds()), oldDocuemntId);
            var oldWorkflows = await SqlDapperConnection.QueryAsync<WtPtWorkoffice>(wfSqlQuery, TargetConnectionString);
            foreach (var oldWorkflow in oldWorkflows)
            {
                var documentWorkflow = CreateWorkflow(oldWorkflow, newDocumentId);
                if (documentWorkflow == null) continue;
                Executor.GetCommand<CreateDocumentWorkflowCommand>().Process(d => d.Execute(documentWorkflow));

                documentWorkflow.DateCreate = new DateTimeOffset(oldWorkflow.DateCreate.GetValueOrDefault(DateTime.Now));

                Executor.GetCommand<UpdateDocumentWorkflowCommand>().Process(d => d.Execute(documentWorkflow));
            }
        }

        private DocumentWorkflow CreateWorkflow(WtPtWorkoffice oldWorkoffice, int newDocumentId)
        {
            try
            {
                var wf = new DocumentWorkflow
                {
                    ControlDate = GetNullableDate(oldWorkoffice.ControlDate),
                    CurrentStageId = GetObjectId<DicRouteStage>(oldWorkoffice.ToStageId),
                    CurrentUserId = GetUserId(oldWorkoffice.ToUserId),
                    DateCreate = new DateTimeOffset(oldWorkoffice.DateCreate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldWorkoffice.Stamp.GetValueOrDefault(DateTime.Now)),
                    Description = oldWorkoffice.Description,
                    ExternalId = oldWorkoffice.Id,
                    FromStageId = GetObjectId<DicRouteStage>(oldWorkoffice.FromStageId),
                    FromUserId = GetUserId(oldWorkoffice.FromUserId),
                    IsComplete = GenerateHelper.StringToNullableBool(oldWorkoffice.IsComplete),
                    IsMain = false,
                    IsSystem = GenerateHelper.StringToNullableBool(oldWorkoffice.IsSystem),
                    OwnerId = newDocumentId,
                    RouteId = GetObjectId<DicRoute>(oldWorkoffice.TypeId),
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                    IsCurent = GetCurentFlag(oldWorkoffice)
                };

                return wf;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        private bool GetCurentFlag(WtPtWorkoffice oldWorkoffice)
        {
            return !GenerateHelper.StringToNullableBool(oldWorkoffice.IsComplete) ?? false;
        }

        #endregion

        #region UserSignature

        private async Task FillDocumentUserSignatures(int oldDocuemntId)
        {
            var userSignatureSqlQuery = string.Format(ImportSqlQueryHelper.UserSignatureSqlQuery, string.Join(", ", ObjectType.GetMaterilsRouteIds()), oldDocuemntId);
            var oldUsersSignaturs = await SqlDapperConnection.QueryAsync<TbDocumentUsersSignature>(userSignatureSqlQuery, TargetConnectionString);
            foreach (var oldUsersSignatur in oldUsersSignaturs)
            {
                var usersSignatur = CreateUsersSignatur(oldUsersSignatur);
                if (usersSignatur == null) continue;
                await Executor.GetCommand<CreateDocumentUserSignatureCommand>().Process(d => d.Execute(usersSignatur));

                usersSignatur.DateCreate = new DateTimeOffset(oldUsersSignatur.FlSignDate.GetValueOrDefault(DateTime.Now));

                await Executor.GetCommand<UpdateDocumentUserSignatureCommand>().Process(d => d.Execute(usersSignatur));
            }
        }

        private DocumentUserSignature CreateUsersSignatur(TbDocumentUsersSignature oldUsersSignature)
        {
            try
            {
                var userId = GetUserId(oldUsersSignature.FlUserId);
                if (userId == null || userId == 0) return null;

                var workflowId = GetObjectId<DocumentWorkflow>(oldUsersSignature.FlDocUId);
                if (workflowId == null || workflowId == 0) return null;

                var userSignature = new DocumentUserSignature
                {
                    DateCreate = new DateTimeOffset(oldUsersSignature.FlSignDate.GetValueOrDefault(DateTime.Now)),
                    DateUpdate = new DateTimeOffset(oldUsersSignature.FlSignDate.GetValueOrDefault(DateTime.Now)),
                    ExternalId = oldUsersSignature.Id,
                    IsValidCertificate = true,
                    PlainData = oldUsersSignature.FlFingerPrint,
                    SignedData = oldUsersSignature.FlSignedData,
                    SignerCertificate = oldUsersSignature.FlSignerCertificate,
                    Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks),
                    UserId = userId.Value,
                    WorkflowId = workflowId.Value
                };

                return userSignature;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Attachments

        private async Task FillAttachments(int oldDocumentId, int newDocumentId)
        {
            var attachmentsSqlQuery = string.Format(ImportSqlQueryHelper.DocumentsAttachmentsSqlQuery, oldDocumentId);
            var oldAttachments = await SqlDapperConnection.QueryAsync<DocumentData>(attachmentsSqlQuery, TargetAttachmentConnectionString);
            foreach (var oldAttachment in oldAttachments)
            {
                var attachment = await CreateAttachment(oldAttachment, newDocumentId);
                if (attachment == null) continue;
                await Executor.GetCommand<CreateAttachmentCommand>().Process(d => d.ExecuteAsync(attachment));
            }
        }

        private async Task<Attachment> CreateAttachment(DocumentData oldAttachment, int newDocumentId)
        {
            try
            {
                var fileName = oldAttachment.FileName;
                var file = oldAttachment.File;

                var documentInfo = SavedDocuments.FirstOrDefault(d => d.Id == newDocumentId);
                if (documentInfo == null) return null;

                var extentionPath = GetDocumentTypeName((byte)documentInfo.DocumentType);
                var bucketName = GetBucketName(null, newDocumentId, null);

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = $"{Guid.NewGuid()}.{FileTypes.Pdf}";
                }

                var originalName = GetFolderWithOriginalName(null, newDocumentId, null, fileName, extentionPath);
                var validName = fileName.MakeValidFileName();
                var contentType = FileTypeHelper.GetContentType(fileName);

                await _fileStorage.AddAsync(bucketName, originalName, file, contentType);

                var attachment = new Attachment
                {
                    AuthorId = DeveloperUserId,
                    ContentType = contentType,
                    BucketName = bucketName,
                    IsMain = true,
                    CopyCount = documentInfo.CopyCount,
                    PageCount = documentInfo.PageCount,
                    DateCreate = DateTimeOffset.Now,
                    DateUpdate = DateTimeOffset.Now,
                    OriginalName = originalName,
                    Length = file.Length,
                    ValidName = validName,
                    Hash = _generateHash.GenerateFileHash(file),
                    ExternalId = documentInfo.ExternalId,
                    DocumentId = newDocumentId,
                    IsDeleted = false
                };

                return attachment;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}