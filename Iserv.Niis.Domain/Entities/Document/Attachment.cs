using System;
using System.ComponentModel.DataAnnotations.Schema;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Domain.Entities.Document
{
    /// <summary>
    /// Прикрепленный файл
    /// </summary>
    public class Attachment : Entity<int>, IHaveFileAttachment, ISoftDeletable
    {
        public string OriginalName { get; set; }
        public string ValidName { get; set; }
        public bool IsMain { get; set; }
        public int? CopyCount { get; set; }
        public int? PageCount { get; set; }
        public string BucketName { get; set; }
        public long Length { get; set; }
        public string ContentType { get; set; }
        public string Hash { get; set; }
        public string FileUrl { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public int? ProtectionDocId { get; set; }
        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }

        public int? DocumentId { get; set; }
        public Document Document { get; set; }

        public int? RequestId { get; set; }
        public Request.Request Request { get; set; }

        public Document MainDocument { get; set; }

        [NotMapped]
        public string AttachmentSize => Length.BytesToString();

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
    }
}
