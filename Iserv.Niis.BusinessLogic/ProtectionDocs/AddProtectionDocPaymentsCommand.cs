using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class AddProtectionDocPaymentsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<PaymentInvoice> invoices)
        {
            var protectionDocRepository = Uow.GetRepository<PaymentInvoice>();
            await protectionDocRepository.CreateRangeAsync(invoices);
            await Uow.SaveChangesAsync();
        }
    }
}
