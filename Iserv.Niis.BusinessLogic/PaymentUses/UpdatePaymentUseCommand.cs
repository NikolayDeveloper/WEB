using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.PaymentUses
{
    public class UpdatePaymentUseCommand : BaseCommand
    {
        public void Execute(PaymentUse paymentUse)
        {
            var paymentUseRepository = Uow.GetRepository<PaymentUse>();
            paymentUseRepository.Update(paymentUse);
            Uow.SaveChanges();
        }
    }
}