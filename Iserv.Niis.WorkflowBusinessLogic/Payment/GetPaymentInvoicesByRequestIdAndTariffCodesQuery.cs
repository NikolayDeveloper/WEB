using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Payment
{
    public class GetPaymentInvoicesByRequestIdAndTariffCodesQuery : BaseQuery
    {
        public List<PaymentInvoice> Execute(int requestId, string[] tariffCodes)
        {
            var paymentInvoices = Uow.GetRepository<PaymentInvoice>()
                .AsQueryable()
                .Include(i => i.Status)
                .Include(i => i.PaymentUses)
                .Include(i => i.Tariff)
                .Where(i => i.RequestId == requestId && tariffCodes.Contains(i.Tariff.Code))
                .ToList();

            return paymentInvoices;
        }
    }
}