using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.References
{
    /// <summary>
    /// Коды оплат История(SPT_VID_TARIFF_HISTORY)
    /// </summary>
    public class RefTariffHistory : Entity<int>, IHistoryEntity
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
        public int? PatentTypeId { get; set; }
        public decimal Price { get; set; }
        public string Limit { get; set; }
    }
}