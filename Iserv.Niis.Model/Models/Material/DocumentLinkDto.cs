using Iserv.Niis.Domain.Enums;

namespace Iserv.Niis.Model.Models.Material
{
    /// <summary>
    /// Сcылка на документ
    /// </summary>
    public class DocumentLinkDto
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Родитель
        /// </summary>
        public int ParentDocumentId { get; set; }
        public string ParentDocumentTypeName { get; set; }
        public string ParentDocumentNumber { get; set; }
        public DocumentType ParentDocumentType { get; set; }

        /// <summary>
        /// Дочерний
        /// </summary>
        public int ChildDocumentId { get; set; }
        public string ChildDocumentTypeName { get; set; }
        public string ChildDocumentNumber { get; set; }
        public DocumentType ChildDocumentType { get; set; }

        /// <summary>
        /// Флаг удаления
        /// </summary>
        public bool? NeedRemove { get; set; }
    }
}
