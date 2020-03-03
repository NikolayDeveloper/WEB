using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Международная Классификация Изобразительных Элементов Товарных Знаков
    /// </summary>
    public class DicICFEM : DictionaryEntity<int>, IClassification<DicICFEM>, IHaveConcurrencyToken
    {
        public int? ParentId { get; set; }
        public DicICFEM Parent { get; set; }
        public ICollection<DicICFEM> Childs { get; set; }
    }
}