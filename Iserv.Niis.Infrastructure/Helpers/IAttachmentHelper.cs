using System.IO;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Model.Models;

namespace Iserv.Niis.Infrastructure.Helpers
{
    public interface IAttachmentHelper
    {
        Attachment NewFileObject(AttachmentDto attachment, int userId, string bucketName,
            FileStream file, string originalName, string fileName, bool isMain = false, int? documentId = null);

        Attachment NewPdfObject(AttachmentDto attachment, int userId, string bucketName,
            FileStream file, string originalName, string fileName, bool isMain = false, int? documentId = null);
    }
}