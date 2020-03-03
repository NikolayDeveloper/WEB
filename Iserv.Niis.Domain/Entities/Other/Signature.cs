using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Other
{
    /// <summary>
    /// Подписи
    /// </summary>
    public class Signature : IEntityHasFile<int>
    {
        public DateTimeOffset? DateCreate { get; set; }
        public DateTimeOffset? DateUpdate { get; set; }
        public DateTimeOffset DateFrom { get; set; }
        public DateTimeOffset DateTo { get; set; }
        public string Note { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public byte[] File { get; set; }
        public string FileType { get; set; }
        public string FileFingerPrint { get; set; }

        #region Relationships

        public int? UserId { get; set; }
        public ApplicationUser User { get; set; }

        #endregion
    }
}