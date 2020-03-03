using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.AccountingData
{
    /// <summary>
    /// Информация о патентно поверенном(WT_CUSTOMER_HISTORY)
    /// </summary>
    public class CustomerAttorneyInfoHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public int CustomerId { get; set; }
        public string RegCode { get; set; }
        public string GovReg { get; set; }
        public DateTimeOffset? GovRegDate { get; set; }
        public string FieldOfActivity { get; set; }
        public string FieldOfKnowledge { get; set; }
        public string WorkPlace { get; set; }
        public string Language { get; set; }
        public DateTimeOffset? DateCard { get; set; }
        public DateTimeOffset? DateDisCard { get; set; }
        public DateTimeOffset? DateBeginStop { get; set; }
        public DateTimeOffset? DateEndStop { get; set; }
        public string SomeDate { get; set; }
        public string PaymentOrder { get; set; }
        public DateTimeOffset? DatePublic { get; set; }
        public string PublicRedefine { get; set; }
        public string Redefine { get; set; }
        public string Education { get; set; }
    }
}