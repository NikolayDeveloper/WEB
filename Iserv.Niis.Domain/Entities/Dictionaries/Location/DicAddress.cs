using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.EntitiesHistory.AccountingData;

namespace Iserv.Niis.Domain.Entities.Dictionaries.Location
{
    /// <summary>
    /// Адреса
    /// </summary>
    public class DicAddress : DictionaryEntity<int>, IHistorySupport, IHaveConcurrencyToken
    {
        public int? ContinentId { get; set; }
        public DicContinent Continent { get; set; }
        public string PostCode { get; set; }

        #region Relationshis

        public int? LocationId { get; set; }
        public DicLocation Location { get; set; }
        public int? CountryId { get; set; }
        public DicCountry Country { get; set; }

        #endregion

        public Type GetHistoryEntity()
        {
            return typeof(AddressHistory);
        }
    }
}