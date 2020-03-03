namespace Iserv.Niis.Model.Models.Payment
{
    public class PaymentReturnAmountResponseDto
    {
        public bool PaymentUsesExist { get; set; }
        public bool ReturnAmountIsGreaterThanPaymentReminder { get; set; }
        public bool Success { get; set; }
    }
}