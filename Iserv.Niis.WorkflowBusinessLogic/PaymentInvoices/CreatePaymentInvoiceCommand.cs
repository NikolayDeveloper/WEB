using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices
{
    public class CreatePaymentInvoiceCommand : BaseCommand
    {
        public void Execute(PaymentInvoice paymentInvoice)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            paymentInvoiceRepository.Create(paymentInvoice);
            Uow.SaveChanges();
        }
    }
}
