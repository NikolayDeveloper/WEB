using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using NetCoreCQRS;
using NetCoreCQRS.Handlers;
using System.Collections.Generic;

namespace Iserv.Niis.Workflow.Tests.TestData.Payments
{
    public class FillPaymentInvoicesHandler : BaseHandler
    {
        private readonly IExecutor _executor;

        public FillPaymentInvoicesHandler(IExecutor executor)
        {
            _executor = executor;
        }

        public int Execute()
        {
            var dicRouteStages = GetPaymentInvoices();
            _executor.GetCommand<CreatePaymentInvoiceCommand>().Process(c => c.Execute(dicRouteStages));
            return 1;
        }

        public List<PaymentInvoice> GetPaymentInvoices()
        {
            return new List<PaymentInvoice>()
            {
                new PaymentInvoice{  }

            };
        }
    }
}
