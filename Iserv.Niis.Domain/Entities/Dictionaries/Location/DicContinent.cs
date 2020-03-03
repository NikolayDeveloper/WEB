using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries.Location
{
    /// <summary>
    /// Континенты
    /// </summary>
    public class DicContinent : DictionaryEntity<int>, IClassification<DicContinent>, IHaveConcurrencyToken
    {
        public DicContinent()
        {
            Childs = new HashSet<DicContinent>();
        }

        public int? ParentId { get; set; }
        public DicContinent Parent { get; set; }
        public int? Order { get; set; }
        public ICollection<DicContinent> Childs { get; set; }
    }
}