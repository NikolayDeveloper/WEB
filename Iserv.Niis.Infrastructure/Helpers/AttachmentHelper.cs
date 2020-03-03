using System;
using System.IO;
using System.Security.Cryptography;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.Model.Models;

namespace Iserv.Niis.Infrastructure.Helpers
{
    public class AttachmentHelper : IAttachmentHelper
    {
        private readonly IFileConverter _fileConverter;

        public AttachmentHelper(IFileConverter fileConverter)
        {
            _fileConverter = fileConverter;
        }

        public Attachment NewPdfObject(AttachmentDto attachment, int userId, string bucketName, 
            FileStream file, string originalName, string fileName, bool isMain = false, int? documentId = null)
        {
            var newAttachment = new Attachment
            {
                OriginalName = originalName,
                ValidName = fileName.MakeValidFileName(),
                IsMain = isMain,
                CopyCount = attachment.CopyCount,
                PageCount = _fileConverter.PageCount(file),
                Length = file.Length,
                ContentType = attachment.ContentType,
                Hash = GenerateFileHash(file),
                AuthorId = userId,
                BucketName = bucketName,
                DocumentId = documentId
            };

            return newAttachment;
        }

        public Attachment NewFileObject(AttachmentDto attachment, int userId, string bucketName, 
            FileStream file, string originalName, string fileName, bool isMain = false, int? documentId = null)
        {
            var newAttachment = new Attachment
            {
                OriginalName = originalName,
                ValidName = fileName.MakeValidFileName(),
                IsMain = isMain,
                CopyCount = attachment.CopyCount,
                PageCount = attachment.PageCount,
                Length = file.Length,
                ContentType = attachment.ContentType,
                Hash = GenerateFileHash(file),
                AuthorId = userId,
                BucketName = bucketName,
                DocumentId = documentId
            };
            return newAttachment;
        }

        private string GenerateFileHash(FileStream stream)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(stream)).Replace("-", "").ToLower();
        }

        private string GenerateFileHash(byte[] bytes)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(bytes)).Replace("-", "").ToLower();
        }
    }
}