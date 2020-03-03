using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Payment
{
    public class CreateInvoiceCommand: BaseCommand
    {
        public int Execute(PaymentInvoice invoice)
        {
            var repo = Uow.GetRepository<PaymentInvoice>();
            repo.Create(invoice);

            Uow.SaveChanges();
            return invoice.Id;
        }
    }
}
