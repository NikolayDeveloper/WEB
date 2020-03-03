using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Payment
{
    public class CreatePaymentUseCommand: BaseCommand
    {
        public int Execute(PaymentUse paymentUse)
        {
            var repo = Uow.GetRepository<PaymentUse>();
            repo.Create(paymentUse);
            Uow.SaveChanges();

            return paymentUse.Id;
        }
    }
}
