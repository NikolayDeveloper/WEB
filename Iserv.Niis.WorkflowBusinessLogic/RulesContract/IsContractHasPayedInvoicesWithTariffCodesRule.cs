using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesContract
{
    public class IsContractHasPayedInvoicesWithTariffCodesRule : BaseRule<ContractWorkFlowRequest>
    {
        public bool Eval(string[] tariffCodes)
        {
            var paymentInvoices = Executor.GetQuery<GetPaymentInvoicesByContractIdAndTariffCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.ContractId, tariffCodes));

            if(paymentInvoices.Any() == false)
            {
                return false;
            }

            var result = paymentInvoices.All(i => new[] { DicPaymentStatusCodes.Credited, DicPaymentStatusCodes.Charged }
                    .Contains(i.Status.Code));

            return result;
        }
    }
}