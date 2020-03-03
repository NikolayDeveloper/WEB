using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    public class CreatePaymentInvoiceCommand : BaseCommand
    {
        public async Task<int> ExecuteAsync(PaymentInvoice paymentInvoice)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            await paymentInvoiceRepository.CreateAsync(paymentInvoice);
            await Uow.SaveChangesAsync();
            return paymentInvoice.Id;
        }
    }
}