using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Привязка типа документа и группы(такая сущность для возможности привязывать один тип для нескольких групп)
    /// </summary>
    public class DicDocumentTypeGroupType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        /// <summary>
        /// Группа типа
        /// </summary>
        public int DocumentTypeGroupId { get; set; }
        public DicDocumentTypeGroup DocumentTypeGroup { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public int DocumentTypeId { get; set; }
        public DicDocumentType DocumentType { get; set; }
    }
}
