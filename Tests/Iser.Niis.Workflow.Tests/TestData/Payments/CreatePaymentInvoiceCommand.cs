using Iserv.Niis.Domain.Entities.Payment;
using NetCoreCQRS.Commands;
using System.Collections.Generic;

namespace Iserv.Niis.Workflow.Tests.TestData.Payments
{
    public class CreatePaymentInvoiceCommand : BaseCommand
    {
        public void Execute(List<PaymentInvoice> paymentInvoices)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            paymentInvoiceRepository.CreateRange(paymentInvoices);

            Uow.SaveChanges();
        }
    }
}
