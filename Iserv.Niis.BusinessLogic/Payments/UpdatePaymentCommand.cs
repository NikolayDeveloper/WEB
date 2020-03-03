using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Payments
{
    public class UpdatePaymentCommand : BaseCommand
    {
        public void Execute(Payment payment)
        {
            var paymentRepository = Uow.GetRepository<Payment>();
            paymentRepository.Update(payment);
            Uow.SaveChanges();
        }
    }
}