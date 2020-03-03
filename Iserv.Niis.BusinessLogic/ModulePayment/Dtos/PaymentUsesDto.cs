using Iserv.Niis.Domain.Entities.Payment;
using System;

namespace Iserv.Niis.BusinessLogic.ModulePayment.Dtos
{
    public class PaymentUsesDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public static Func<PaymentUse, PaymentUsesDto> ToPaymentUsesDto = paymentUse =>
        {
            return new PaymentUsesDto
            {
                Id = paymentUse.Id,
                Amount = paymentUse.Amount
            };
        };
    }
}
