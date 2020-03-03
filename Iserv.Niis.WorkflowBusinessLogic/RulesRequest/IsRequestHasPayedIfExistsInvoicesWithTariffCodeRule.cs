using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasPayedIfExistsInvoicesWithTariffCodeRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string tariffCode)
        {
            var invoice = Executor.GetQuery<GetPaymentInvoicesByRequestIdAndTariffCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, new[] { tariffCode })).FirstOrDefault();

            if (invoice == null)
            {
                return true;
            }

            var isPayed = new[] { DicPaymentStatusCodes.Credited, DicPaymentStatusCodes.Charged }.Contains(invoice.Status.Code);

            return isPayed;
        }
    }
}