using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    public class DeletePhisicallyPaymentInvoiceCommand: BaseCommand
    {
        public void Execute(PaymentInvoice paymentInvoice)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            paymentInvoiceRepository.Delete(paymentInvoice);
            Uow.SaveChanges();
        }
    }
}
