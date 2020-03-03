using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;
using Iserv.Niis.Domain.Entities.Payment;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsExceededIcgsRequestsPaidForRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] tariffCodes, int amount = 3, decimal underpaymentAmount = 100)
        {
            var paymentInvoices = Executor.GetQuery<GetPaymentInvoicesByRequestIdAndTariffCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, tariffCodes))
                .Where(i => IsUnderPaid(i, underpaymentAmount) || i.Status.Code == DicPaymentStatusCodes.Credited)
                .ToList();
            var totalTariffCount = paymentInvoices.Sum(i => i.TariffCount);
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));

            return totalTariffCount >= request.ICGSRequests.Count - amount;
        }

        private bool IsUnderPaid(PaymentInvoice invoice, decimal underpaymentAmount)
        {
            return invoice.Tariff.Price * (decimal)0.12 + invoice.Tariff.Price <= invoice.PaymentUses.Sum(pu => pu.Amount) + underpaymentAmount &&
                   invoice.Status.Code == DicPaymentStatusCodes.Notpaid;
        }
    }
}
