using System.Linq;
using Iserv.Niis.WorkflowBusinessLogic.ContractDocuments;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesContract
{
    public class IsContractHasAnyDocumentWithCodesRule : BaseRule<ContractWorkFlowRequest>
    {
        public bool Eval(string[] typeCodes)
        {
            return Executor.GetQuery<GetDocumentsByContractIdAndTypeCodesQuery>().Process(c => c.Execute(WorkflowRequest.ContractId, typeCodes)).Any();
        }
    }
}