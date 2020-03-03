using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] tariffCodes)
        {
            var paymentInvoices = Executor.GetQuery<GetPaymentInvoicesByRequestIdAndTariffCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, tariffCodes));

            if (!paymentInvoices.Any())
                return false;

            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));
            if (request == null)
            {
                return false;
            }

            return paymentInvoices
                .Any(i => new[] { DicPaymentStatusCodes.Credited, DicPaymentStatusCodes.Charged }.Contains(i.Status.Code) && i.DateCreate > request.CurrentWorkflow.DateCreate);
        }
    }
}
