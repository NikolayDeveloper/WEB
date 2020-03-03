using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.Dictionaries.Location
{
    /// <summary>
    /// Локации
    /// </summary>
    public class DicLocation : DictionaryEntity<int>, IClassification<DicLocation>, IHaveConcurrencyToken
    {
        public DicLocation()
        {
            Childs = new HashSet<DicLocation>();
        }

        public int? Order { get; set; }
        public string StatId { get; set; }
        public string StatParentId { get; set; }

        #region Relationships

        public int? ParentId { get; set; }
        public DicLocation Parent { get; set; }
        public ICollection<DicLocation> Childs { get; set; }

        public int? CountryId { get; set; }
        public DicCountry Country { get; set; }

        public int TypeId { get; set; }
        public DicLocationType Type { get; set; }

        #endregion
    }
}