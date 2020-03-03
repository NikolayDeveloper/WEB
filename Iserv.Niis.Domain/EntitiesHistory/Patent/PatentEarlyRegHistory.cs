using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.Patent
{
    /// <summary>
    /// Связанные Заявки / ОД История(WT_PT_EARLYREG_HISTORY)
    /// </summary>
    public class PatentEarlyRegHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public int? PatentId { get; set; }
        public int? EarlyRegTypeId { get; set; }
        public int? PCTType { get; set; }
        public int? RegCountryId { get; set; }
        public string RegNumber { get; set; }
        public DateTimeOffset? RegDate { get; set; }
        public string NameSD { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DateF1 { get; set; }
        public DateTimeOffset? DateF2 { get; set; }
    }
}