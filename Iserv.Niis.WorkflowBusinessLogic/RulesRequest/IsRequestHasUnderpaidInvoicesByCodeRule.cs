using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;
using Iserv.Niis.Domain.Entities.Payment;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasUnderpaidInvoicesByCodeRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] tariffCodes, decimal amount = 100)
        {
            var invoices = Executor.GetQuery<GetPaymentInvoicesByRequestIdAndTariffCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, tariffCodes)).ToList();

            if (invoices.Any(i =>
                i.Status.Code == DicPaymentStatusCodes.Credited))
            {
                return true;
            }
            var result = invoices.Any(i => IsUnderPaid(i, amount));

            return result;
        }

        private bool IsUnderPaid(PaymentInvoice invoice, decimal amount)
        {
            return invoice.Tariff.Price * (decimal) 0.12 + invoice.Tariff.Price <= invoice.PaymentUses.Sum(pu => pu.Amount) + amount &&
                   invoice.Status.Code == DicPaymentStatusCodes.Notpaid;
        }
    }
}
