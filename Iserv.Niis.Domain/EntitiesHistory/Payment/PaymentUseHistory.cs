using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.Payment
{
    /// <summary>
    ///Распределение Оплат История(WT_PL_PAYMENT_USE_HISTORY)
    /// </summary>
    public class PaymentUseHistory : Entity<int>, IHistoryEntity
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
        public int? PaymentId { get; set; }
        public int? PaymentInvoiceId { get; set; }
        public decimal Amount { get; set; }
        public int? CreateUserId { get; set; }
        public string AmountWithoutNds { set; get; }
    }
}