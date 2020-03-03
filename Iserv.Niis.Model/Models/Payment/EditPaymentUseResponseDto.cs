namespace Iserv.Niis.Model.Models.Payment
{
    public class EditPaymentUseResponseDto
    {
        public bool AmountIsGreaterThanPaymentInvoiceReminder { get; set; }
        public decimal PaymentInvoiceReminder { get; set; }
        public bool AmountIsGreaterThanPaymentReminder { get; set; }
        public decimal PaymentReminder { get; set; }
        public bool PaymentInvoiceNewReminderIsGreaterThan100KZT { get; set; }
        public bool Success { get; set; }
    }
}