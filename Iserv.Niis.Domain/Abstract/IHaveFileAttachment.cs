using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Abstract
{
    public interface IHaveFileAttachment
    {
        string OriginalName { get; set; }
        string ValidName { get; set; }
        bool IsMain { get; set; }
        int? CopyCount { get; set; }
        int? PageCount { get; set; }
        string BucketName { get; set; }
        long Length { get; set; }
        string ContentType { get; set; }
        string Hash { get; set; }
        string FileUrl { get; set; }
        int AuthorId { get; set; }
        ApplicationUser Author { get; set; }
        int? DocumentId { get; set; }
        Document Document { get; set; }
    }
}
