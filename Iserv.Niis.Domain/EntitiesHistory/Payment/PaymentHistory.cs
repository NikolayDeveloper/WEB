using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.Payment
{
    /// <summary>
    /// Платежи История(WT_PL_PAYMENT_HISTORY)
    /// </summary>
    public class PaymentHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public int? CustomerId { get; set; }
        public string PaymentNumber { get; set; }
        public decimal? Amount { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public string Payment1CNumber { get; set; }
        public bool? IsPrePayment { get; set; }
        public string PurposeDescription { get; set; }
        public string AssignmentDescription { get; set; }
        public decimal? CurrencyAmount { get; set; }
        public decimal? CurrencyRate { get; set; }
        public string CurrencyType { get; set; }
        public string CustomerRnn { set; get; }
        public string CustomerXin { set; get; }
        public decimal? RemainderAmount { set; get; }
        public decimal? PaymentUseAmountSum { set; get; }
    }
}