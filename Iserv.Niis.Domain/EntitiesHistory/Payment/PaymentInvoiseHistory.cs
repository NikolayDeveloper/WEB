using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.Payment
{
    /// <summary>
    /// Выставленные счета История(WT_PL_FIXPAYMENT_HISTORY)
    /// </summary>
    public class PaymentInvoiseHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public int? TariffId { get; set; }
        public decimal Coefficient { get; set; }
        public decimal Nds { get; set; }
        public int? PatentId { get; set; }
        public decimal? TariffAmount { get; set; }
        public decimal PenaltyPercent { get; set; }
        public int? TariffCount { get; set; }
        public bool? IsComplete { get; set; }
        public DateTimeOffset? OverdueDate { get; set; }
        public DateTimeOffset? DateFact { get; set; }
        public DateTimeOffset? DateComplete { get; set; }
        public int? CreateUserId { get; set; }
        public decimal PaymentInvoiceUseSum { set; get; }
        public decimal PaymentInvoiceRemainder { set; get; }
        public decimal PaymentInvoiceSum { set; get; }
        public decimal PaymentInvoiceWithNds { set; get; }
    }
}