using System.IO;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.FileConverter;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.Material.Incoming;

namespace Iserv.Niis.Features.Helpers
{
    public interface IAttachmentHelper
    {
        Attachment NewFileObject(MaterialAttachmentDto attachment, int userId, int documentId, string bucketName,
            FileStream file, bool isMain = false);

        Attachment NewPdfObject(MaterialAttachmentDto attachment, int userId, int documentId, string bucketName,
            FileStream file, bool isMain = false);
    }
}