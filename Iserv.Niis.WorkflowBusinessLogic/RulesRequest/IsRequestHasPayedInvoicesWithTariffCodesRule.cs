using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasPayedInvoicesWithTariffCodesRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] tariffCodes)
        {
            var paymentInvoices = Executor.GetQuery<GetPaymentInvoicesByRequestIdAndTariffCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, tariffCodes));

            if (!paymentInvoices.Any())
                return false;

            return paymentInvoices
                .All(i => new[] { DicPaymentStatusCodes.Credited, DicPaymentStatusCodes.Charged }.Contains(i.Status.Code));
        }
    }
}