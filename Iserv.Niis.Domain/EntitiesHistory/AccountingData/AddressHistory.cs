using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.AccountingData
{
    /// <summary>
    /// Адреса История(WT_ADDRESS_HISTORY)
    /// </summary>
    public class AddressHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public int? ContinentId { get; set; }
        public int? CountryId { get; set; }
        public int? LocationId { get; set; }
        public string PostCode { get; set; }
    }
}