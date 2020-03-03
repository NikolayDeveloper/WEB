using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.AccountingData
{
    /// <summary>
    /// Заявка - Контрагенты История(RF_CUSTOMER_HISTORY)
    /// </summary>
    public class CustomerPatentLinkHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public int PatentLinkTypeId { get; set; }
        public int PatentId { get; set; }
        public int? CustomerId { get; set; }
        public int? AddressId { get; set; }
        public bool? IsMention { get; set; }
        public DateTimeOffset? DateBegin { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
    }
}