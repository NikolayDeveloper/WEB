namespace Iserv.Niis.Model.Models.Payment
{
    public class PaymentUseDto
    {
        public int? PaymentId { get; set; }
        public int? PaymentInvoiceId { get; set; }
        public decimal Amount { get; set; }
        public int? CreateUserId { get; set; }
        public string AmountWithoutNds { set; get; }
        public string Description { get; set; }
    }
}