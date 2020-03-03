using System.Linq;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Payments
{
    public class CheckExistPaymentCommand : BaseCommand
    {
        public bool Execute(Payment payment)
        {
            var paymentRepository = Uow.GetRepository<Payment>();

            var result = paymentRepository
                .AsQueryable()
                .Any(d => d.PaymentNumber == payment.PaymentNumber 
                        && d.Payment1CNumber == payment.Payment1CNumber
                        && d.PurposeDescription == payment.PurposeDescription);

            return result;
        }
    }
}