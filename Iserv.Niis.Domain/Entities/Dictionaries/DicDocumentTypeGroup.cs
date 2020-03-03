using Iserv.Niis.Domain.Abstract;
using System.Collections.Generic;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Группа для типа документа
    /// </summary>
    public class DicDocumentTypeGroup : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public DicDocumentTypeGroup()
        {
        }

        public DicDocumentTypeGroup(ICollection<DicDocumentTypeGroupType> documentTypes)
        {
            DocumentTypes = documentTypes;
        }

        /// <summary>
        /// Типы документов в группе
        /// </summary>
        public ICollection<DicDocumentTypeGroupType> DocumentTypes { get; set; }
    }
}
