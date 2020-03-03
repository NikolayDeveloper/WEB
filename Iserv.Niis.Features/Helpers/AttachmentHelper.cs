using System;
using System.IO;
using System.Security.Cryptography;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.Infrastructure;
using Iserv.Niis.Model.Models.Material;

namespace Iserv.Niis.Features.Helpers
{
    public class AttachmentHelper : IAttachmentHelper
    {
        private readonly IFileConverter _fileConverter;

        public AttachmentHelper(IFileConverter fileConverter)
        {
            _fileConverter = fileConverter;
        }

        public Attachment NewPdfObject(MaterialAttachmentDto attachment, int userId,
            int documentId, string bucketName, FileStream file, bool isMain = false)
        {
            var newAttachment = new Attachment
            {
                OriginalName = attachment.Name,
                ValidName = attachment.Name.MakeValidFileName(),
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

        public Attachment NewFileObject(MaterialAttachmentDto attachment, int userId,
            int documentId, string bucketName, FileStream file, bool isMain = false)
        {
            var newAttachment = new Attachment
            {
                OriginalName = attachment.Name,
                ValidName = attachment.Name.MakeValidFileName(),
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