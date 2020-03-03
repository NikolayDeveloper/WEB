using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    public class CreateRangePaymentInvoiceCommand : BaseCommand
    {
        public async Task ExecuteAsync(List<PaymentInvoice> paymentInvoices)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            await paymentInvoiceRepository.CreateRangeAsync(paymentInvoices);
            await Uow.SaveChangesAsync();
        }
    }
}