using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.References
{
    /// <summary>
    /// Типы и Шаблоны Документов История(CL_DOCUMENT_HISTORY)
    /// </summary>
    public class RefDocumentTypeHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string TemplateFileHtml { get; set; }
        public int? TemplateFileId { get; set; }
        public bool? IsUnique { get; set; }
        public int? Order { get; set; }
        public bool? IsRequireSigning { get; set; }
        public string TemplateFingerPrint { get; set; }
        public int? RouteId { get; set; }
        public int? DocClassificationId { get; set; }
    }
}