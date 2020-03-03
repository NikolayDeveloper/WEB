using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Payment
{
    public class UpdateRequestPaymentInvoicesByProtectionDocIdCommand : BaseCommand
    {
        public void Execute(int requestId, int protectionDocId)
        {
            var dicTariffRepo = Uow.GetRepository<DicTariff>()
                .AsQueryable()
                .Where(d => d.Code == DicTariffCodes.TrademarNmptRegistrationAndPublishing || d.Code == DicTariffCodes.TimeRestore)
                .Select(d => d.Id)
                .ToList();

            var paymentInvoices = Uow.GetRepository<PaymentInvoice>()
                .AsQueryable()
                .Where(i => i.RequestId == requestId && dicTariffRepo.Contains(i.TariffId))
                .ToList();

            foreach (var paymentInvoice in paymentInvoices)
            {
                paymentInvoice.ProtectionDocId = protectionDocId;
            }

            Uow.SaveChanges();
        }
    }
}