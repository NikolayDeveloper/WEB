using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IntegrationShepUploader;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.ExternalServices.Features.Constants;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.Models;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Utils.Abstractions;
using Iserv.Niis.Utils.Constans;
using Iserv.Niis.Utils.Helpers;
using File = System.IO.File;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils
{
    public class IntegrationAttachFileHelper
    {
        private readonly AppConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly IGenerateHash _generateHash;
        private readonly NiisWebContext _niisContext;
        private readonly DictionaryHelper _dictionaryHelper;

        public IntegrationAttachFileHelper(
            NiisWebContext niisContext,
            IFileStorage fileStorage,
            AppConfiguration configuration,
            IGenerateHash generateHash,
            DictionaryHelper dictionaryHelper)
        {
            _niisContext = niisContext;
            _fileStorage = fileStorage;
            _configuration = configuration;
            _generateHash = generateHash;
            _dictionaryHelper = dictionaryHelper;
        }

        public void AttachFile(AttachedFileModel attachedFile, Document document)
        {
            var attachment = new Attachment
            {
                DateCreate = DateTimeOffset.Now,
                DocumentId = document.Id,
                IsMain = attachedFile.IsMain,
                CopyCount = attachedFile.CopyCount,
                Length = attachedFile.Length,
                AuthorId = _configuration.AuthorAttachmentDocumentId,
                PageCount = attachedFile.PageCount,
                OriginalName = "integration/" + GetDocumentTypeName(document.DocumentType) + "/" + Guid.NewGuid(), //attachedFile.Name,
                ValidName = attachedFile.Name.MakeValidFileName(),
                ContentType = FileTypeHelper.GetContentType(attachedFile.Name),
                Hash = _generateHash.GenerateFileHash(attachedFile.File),
                BucketName = "documents"
            };
            var attachmentId = CreateAttachment(attachment);
            Task.Run(() => _fileStorage.AddAsync(attachment.BucketName, attachment.OriginalName, attachedFile.File, attachment.ContentType)).Wait();

            document.MainAttachmentId = attachmentId;
        }

        public void AttachFile(AttachedFileModel attachedFile, int requestId)
        {
            var attachment = new Attachment
            {
                DateCreate = DateTimeOffset.Now,
                RequestId = requestId,
                IsMain = attachedFile.IsMain,
                CopyCount = attachedFile.CopyCount,
                Length = attachedFile.Length,
                AuthorId = _configuration.AuthorAttachmentDocumentId,
                PageCount = attachedFile.PageCount,
                OriginalName = "integration/" + requestId + "/" + Guid.NewGuid(), //attachedFile.Name,
                ValidName = attachedFile.Name.MakeValidFileName(),
                ContentType = FileTypeHelper.GetContentType(attachedFile.Name),
                Hash = _generateHash.GenerateFileHash(attachedFile.File),
                BucketName = "requests"
            };
            CreateAttachment(attachment);
            Task.Run(() => _fileStorage.AddAsync(attachment.BucketName, attachment.OriginalName, attachedFile.File, attachment.ContentType)).Wait();
        }


        private string GetDocumentTypeName(DocumentType documentType)
        {
            switch (documentType)
            {
                case DocumentType.Incoming:
                    return "incoming";
                case DocumentType.Outgoing:
                    return "outgoing";
                case DocumentType.Internal:
                    return "internal";
                case DocumentType.DocumentRequest:
                    return "request";
                default: return "unknown";
            }
        }

        public byte[] ShepFileDownload(ShepFile shepFile)
        {
            ConfigShepUploader.UrlDownload =
                @"http://" + _configuration.ServerPepIp + @"/SHEP_UL_FileUploaderService/DownloadFile?FileId={0}";

            var bytes = ShepFileClient.Download(shepFile.ID, shepFile.Name, shepFile.Md5hash);

            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("ШЭП-файл пуст. ShepFile.ID = " + shepFile.ID);

            return bytes;
        }

        public ShepFile ShepFileUpload(byte[] file, string fileName)
        {
            ConfigShepUploader.UrlWebService = @"http://" + _configuration.ServerPepIp +
                                               @"/SHEP_UL_FileUploaderService/UploaderService";

            var shepFile = new ShepFile
            {
                Name = fileName,
                Md5hash = ShepFileClient.GetHash(file),
                ID = ShepFileClient.Upload(file, fileName)
            };
            return shepFile;
        }

        public byte[] GetFile(string bucketName, string objectName)
        {
            return AsyncHelpers.RunSync(() => _fileStorage.GetAsync(bucketName, objectName));
        }

        public byte[] DownloadPepRequisitionFile(string chainId, string iin, string shepFileId = null)
        {
            if ("ShepFileID".Equals(shepFileId, StringComparison.OrdinalIgnoreCase))
                return null;
            var url = CommonConstants.PepRequisitionFileUrl.Replace("[url]", _configuration.ServerPepIp);

            var request = WebRequest.Create(string.Format(url, chainId, iin));
            request.Method = "GET";
            request.Timeout = ConfigShepUploader.TimoutMinutes * 60 * 1000;

            var dateTime = DateTime.Now;
            var localFilename = dateTime.ToString("yyyy-MM-dd ") + "at " + dateTime.ToString("HH-mm-ss-fff ");
            localFilename += "@ " + chainId + ".pdf";
            localFilename = Path.Combine(_configuration.FolderRequisitionFile, localFilename);

            using (var response = request.GetResponse())
            {
                using (var answerStream = response.GetResponseStream())
                {
                    using (var fileStream = new FileStream(
                        localFilename, FileMode.CreateNew, FileAccess.Write))
                    {
                        CopyStream(answerStream, fileStream);
                    }
                }
            }

            return File.ReadAllBytes(localFilename);
        }

        public byte[] GetImageFile(AttachedFile[] attachedFiles, bool isPep)
        {
            int dicDocTypeImageDeclarantId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDocumentType), DicDocumentTypeCodes._001_001_1A);
            if (isPep)
            {
                return attachedFiles
                           .Where(x => x.Type.UID == dicDocTypeImageDeclarantId)
                           .Select(x => ShepFileDownload(x.File.ShepFile))
                           .FirstOrDefault() ?? attachedFiles
                           .Where(x =>
                               FileTypeHelper.GetFileExtension(x.File.ShepFile?.Name)
                                   .Equals(FileTypes.Png, StringComparison.CurrentCultureIgnoreCase) ||
                               FileTypeHelper.GetFileExtension(x.File.ShepFile?.Name).Equals(FileTypes.Jpeg,
                                   StringComparison.CurrentCultureIgnoreCase))
                           .Select(x => ShepFileDownload(x.File.ShepFile))
                           .FirstOrDefault();
            }
            return attachedFiles
                       .Where(x => x.Type.UID == dicDocTypeImageDeclarantId)
                       .Select(x => x.File.Content)
                       .FirstOrDefault() ?? attachedFiles
                       .Where(x => FileTypeHelper.GetFileExtension(x.File.Name)
                                       .Equals(FileTypes.Png, StringComparison.CurrentCultureIgnoreCase) ||
                                   FileTypeHelper.GetFileExtension(x.File.Name)
                                       .Equals(FileTypes.Jpeg, StringComparison.CurrentCultureIgnoreCase))
                       .Select(x => x.File.Content)
                       .FirstOrDefault();
        }

        #region PrivateMethods

        private int CreateAttachment(Attachment attachment)
        {
            _niisContext.Attachments.Add(attachment);
            _niisContext.SaveChanges();
            return attachment.Id;
        }
        private void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[32768];
            int read;

            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, read);
        }

        #endregion
    }
}