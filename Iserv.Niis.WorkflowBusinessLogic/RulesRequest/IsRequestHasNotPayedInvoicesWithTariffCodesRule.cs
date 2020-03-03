using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasNotPayedInvoicesWithTariffCodesRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] tariffCodes)
        {
            return Executor.GetQuery<GetPaymentInvoicesByRequestIdAndTariffCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, tariffCodes))
                .All(i => i.Status.Code == DicPaymentStatusCodes.Notpaid);
        }
    }
}