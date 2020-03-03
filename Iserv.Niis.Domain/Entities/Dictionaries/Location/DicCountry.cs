using System;
using System.Collections.Generic;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.Dictionaries.Location
{
    /// <summary>
    /// Страны
    /// </summary>
    public class DicCountry : DictionaryEntity<int>, IClassification<DicCountry>, IHaveConcurrencyToken
    {
        public DicCountry()
        {
            Childs = new HashSet<DicCountry>();
        }

        public int? Order { get; set; }

        #region Relationships

        public int? ContinentId { get; set; }
        public DicContinent Continent { get; set; }
        public int? ParentId { get; set; }
        public DicCountry Parent { get; set; }
        public ICollection<DicCountry> Childs { get; set; }
        public ICollection<DicCustomer> DicCustomers { get; set; }

        #endregion
    }
}