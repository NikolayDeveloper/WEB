using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Payment
{
    public class GetPaymentInvoicesByProtectionDocIdAndTariffCodesQuery: BaseQuery
    {
        public List<PaymentInvoice> Execute(int protectionDocId, string[] tariffCodes)
        {
            var paymentInvoices = Uow.GetRepository<PaymentInvoice>()
                .AsQueryable()
                .Include(i => i.Status)
                .Where(i => i.ProtectionDocId == protectionDocId && tariffCodes.Contains(i.Tariff.Code))
                .ToList();

            return paymentInvoices;
        }
    }
}
