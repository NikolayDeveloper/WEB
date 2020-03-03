using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.PaymentExecutors
{
    public class CreatePaymentExecutorCommand: BaseCommand
    {
        public async Task ExecuteAsync(PaymentExecutor executor)
        {
            var repo = Uow.GetRepository<PaymentExecutor>();
            await repo.CreateAsync(executor);
            await Uow.SaveChangesAsync();
        }
    }
}
