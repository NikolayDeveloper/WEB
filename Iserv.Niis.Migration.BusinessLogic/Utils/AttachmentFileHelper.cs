using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Models;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.Niis.Utils.Abstractions;
using Iserv.Niis.Utils.Constans;
using Iserv.Niis.Utils.Helpers;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Utils
{
    public class AttachmentFileHelper : BaseHelper
    {
        private readonly IFileStorage _fileStorage;
        private readonly IGenerateHash _generateHash;
        private readonly OldNiisDocumentDataService _oldNiisDocumentDataService;
        private readonly AppConfiguration _appConfiguration;

        private NewNiisAttachmentFileService _newNiisAttachmentFileService = null;

        public AttachmentFileHelper(
            IFileStorage fileStorage,
            OldNiisDocumentDataService oldNiisDocumentDataService,
            IGenerateHash generateHash,
            AppConfiguration appConfiguration)
        {
            _fileStorage = fileStorage;
            _oldNiisDocumentDataService = oldNiisDocumentDataService;
            _generateHash = generateHash;
            _appConfiguration = appConfiguration;
        }

        public void AttachFilesToRequests(List<Request> requests, NiisWebContextMigration niisWebContext,
            ref int migratedFilesCount)
        {
            var documentFiles =
                _oldNiisDocumentDataService.GetDdDocumentDatas(requests.Select(r => r.Barcode).ToList());
            if (documentFiles.Any() == false)
            {
                return;
            }

            foreach (var request in requests)
            {
                var documentData = documentFiles.SingleOrDefault(d => d.Id == request.Barcode);
                if (documentData == null)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(documentData.FileName))
                {
                    documentData.FileName = $"{request.Barcode}.{FileTypes.Pdf}";
                }
                var attachment = new Attachment
                {
                    AuthorId = request.UserId ?? _appConfiguration.AuthorAttachmentDocumentId,
                    ContentType = FileTypeHelper.GetContentType(documentData.FileName),
                    BucketName = $"old-request-{request.Barcode}",
                    IsMain = true,
                    CopyCount = request.CopyCount,
                    PageCount = request.PageCount,
                    DateCreate = documentData.DateCreate,
                    DateUpdate = documentData.DateUpdate,
                    OriginalName = documentData.FileName,
                    Length = documentData.File.Length,
                    ValidName = documentData.FileName.MakeValidFileName(),
                    Hash = _generateHash.GenerateFileHash(documentData.File)
                };

                _newNiisAttachmentFileService = new NewNiisAttachmentFileService(niisWebContext);

                _newNiisAttachmentFileService.CreateAttachment(attachment);
                _fileStorage.AddAsync(attachment.BucketName, attachment.ValidName, documentData.File,
                    attachment.ContentType).Wait();

                request.MainAttachmentId = attachment.Id;

                niisWebContext.SaveChanges();

                migratedFilesCount++;
            }
        }

        public void AttachFilesToDocuments(List<Document> documents, NiisWebContextMigration niisWebContext,
            ref int migratedFilesCount)
        {
            var documentFiles =
                _oldNiisDocumentDataService.GetDdDocumentDatas(documents.Select(r => r.Barcode).ToList());
            if (documentFiles.Any() == false)
            {
                return;
            }

            foreach (var document in documents)
            {
                var documentData = documentFiles.SingleOrDefault(d => d.Id == document.Barcode);
                if (documentData == null)
                {
                    continue;
                }

                if (documentData.FileName == null)
                {
                    return;
                }

                var validFileName = $"{document.Barcode}.{Path.GetExtension(documentData.FileName)}";

                var attachment = new Attachment
                {
                    DocumentId = document.Id,
                    AuthorId = _appConfiguration.AuthorAttachmentDocumentId,
                    ContentType = FileTypeHelper.GetContentType(documentData.FileName),
                    BucketName = $"old-document-{document.Barcode}",
                    IsMain = true,
                    DateCreate = documentData.DateCreate,
                    DateUpdate = documentData.DateUpdate,
                    OriginalName = documentData.FileName,
                    Length = documentData.File.Length,
                    ValidName = validFileName,
                    Hash = _generateHash.GenerateFileHash(documentData.File)
                };

                niisWebContext.Attachments.Add(attachment);
                niisWebContext.SaveChanges();

                _fileStorage.AddAsync(attachment.BucketName, attachment.ValidName, documentData.File,
                    attachment.ContentType).Wait();

                document.MainAttachmentId = attachment.Id;
                niisWebContext.SaveChanges();

                migratedFilesCount++;
            }
        }

        public int? GetMainAttachmentId(int oldDocumentId, NiisWebContextMigration niisWebContext)
        {
            var documentData = _oldNiisDocumentDataService.GetDocumentData(oldDocumentId);
            if (documentData == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(documentData.FileName))
            {
                documentData.FileName = $"{oldDocumentId}.{FileTypes.Pdf}";
            }

            var attachment = new Attachment
            {
                AuthorId = _appConfiguration.AuthorAttachmentDocumentId,
                ContentType = FileTypeHelper.GetContentType(documentData.FileName),
                BucketName = $"old-document-{oldDocumentId}",
                IsMain = true,
                DateCreate = documentData.DateCreate,
                DateUpdate = documentData.DateUpdate,
                OriginalName = documentData.FileName,
                Length = documentData.File.Length,
                ValidName = documentData.FileName.MakeValidFileName(),
                Hash = _generateHash.GenerateFileHash(documentData.File)
            };

            _newNiisAttachmentFileService = new NewNiisAttachmentFileService(niisWebContext);

            _newNiisAttachmentFileService.CreateAttachment(attachment);
            _fileStorage.AddAsync(attachment.BucketName, attachment.ValidName, documentData.File,
                attachment.ContentType).Wait();

            return attachment.Id;
        }
    }
}