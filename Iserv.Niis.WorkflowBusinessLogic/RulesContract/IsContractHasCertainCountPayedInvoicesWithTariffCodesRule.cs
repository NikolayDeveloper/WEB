using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesContract
{
    public class IsContractHasCertainCountPayedInvoicesWithTariffCodesRule : BaseRule<ContractWorkFlowRequest>
    {
        public bool Eval(string[] tariffCodes, int count)
        {
            var paymentInvoices = Executor.GetQuery<GetPaymentInvoicesByContractIdAndTariffCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.ContractId, tariffCodes))
                .Where(i => new[] {DicPaymentStatusCodes.Credited, DicPaymentStatusCodes.Charged}
                    .Contains(i.Status.Code)).ToList();
            return paymentInvoices.Count >= count;
        }
    }
}