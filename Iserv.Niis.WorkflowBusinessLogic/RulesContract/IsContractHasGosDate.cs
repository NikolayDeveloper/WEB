using Iserv.Niis.WorkflowBusinessLogic.Contracts;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesContract
{
    public class IsContractHasGosDate : BaseRule<ContractWorkFlowRequest>
    {
        public bool Eval()
        {
            var contract = Executor.GetQuery<GetContractByIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.ContractId));
            return contract.GosDate.HasValue;
        }
    }
}