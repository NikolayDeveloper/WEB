using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Payments
{
    public class CreatePaymentCommand : BaseCommand
    {
        public async Task<int> ExecuteAsync(Payment payment)
        {
            var paymentRepository = Uow.GetRepository<Payment>();
            await paymentRepository.CreateAsync(payment);
            await Uow.SaveChangesAsync();
            return payment.Id;
        }
    }
}