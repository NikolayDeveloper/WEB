using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Payment
{
    public class GetPaymentInvoicesByContractIdAndTariffCodesQuery : BaseQuery
    {
        public List<PaymentInvoice> Execute(int contractId, string[] tariffCodes)
        {
            var paymentInvoices = Uow.GetRepository<PaymentInvoice>()
                .AsQueryable()
                .Include(i => i.Status)
                .Where(i => i.ContractId == contractId && tariffCodes.Contains(i.Tariff.Code))
                .ToList();

            return paymentInvoices;
        }
    }
}