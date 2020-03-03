namespace Iserv.Niis.BusinessLogic.PaymentsJournal.Dto
{
    public class BoundPaymentDto
    {
        public int PaymentId { get; set; }

        public int PaymentInvoiceId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public bool ForceBounded { get; set; }
    }
}