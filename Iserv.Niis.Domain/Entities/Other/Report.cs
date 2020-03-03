using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Other
{
    /// <summary>
    /// Отчет
    /// </summary>
    public class Report : Entity<int>
    {
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public int? TemplateDataFileId { get; set; }
    }
}