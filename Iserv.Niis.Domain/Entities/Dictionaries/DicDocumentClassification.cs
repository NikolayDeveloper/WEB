using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Newtonsoft.Json;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Класификация документов
    /// </summary>
    public class DicDocumentClassification : DictionaryEntity<int>, IClassification<DicDocumentClassification>, IHaveConcurrencyToken
    {
        public DicDocumentClassification()
        {
            Childs = new HashSet<DicDocumentClassification>();
        }

        public int? ParentId { get; set; }
        public DicDocumentClassification Parent { get; set; }
        public ICollection<DicDocumentClassification> Childs { get; set; }

        public override string ToString()
        {
            return Parent == null
                ? NameRu
                : $"{Parent} \\ {NameRu}";
        }
    }
}