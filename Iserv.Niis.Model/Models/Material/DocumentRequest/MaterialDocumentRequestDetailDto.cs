using System;

namespace Iserv.Niis.Model.Models.Material.DocumentRequest
{
    /// <summary>
    /// Документы Заявки
    /// </summary>
    public class MaterialDocumentRequestDetailDto : MaterialDetailDto
    {
        public int? Barcode { get; set; }
        public int? TypeId { get; set; }
        public DateTimeOffset? DateCreate { get; set; }
        public DateTimeOffset? DocumentDate { get; set; }
        public UserInputDto UserInput { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameKz { get; set; }
        public bool HasTemplate { get; set; }
    }
}
