using System.Linq;
using Iserv.Niis.WorkflowBusinessLogic.ContractDocuments;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesContract
{
    public class IsContractDocumentHasStageByStageCodeAndDocumentCodeRule : BaseRule<ContractWorkFlowRequest>
    {
        public bool Eval(string[] documentTypeCodes, string stageCode)
        {
            var documents = Executor.GetQuery<GetDocumentsByContractIdAndTypeCodesQuery>()
                .Process(c => c.Execute(WorkflowRequest.ContractId, documentTypeCodes));
            if (documents.Any())
            {
                var isDocuemtnHasWithStageCode = documents.Where(r => r.CurrentWorkflows.Any(cwf => cwf.CurrentStage.Code == stageCode)).Any();
                return isDocuemtnHasWithStageCode;
            }

            return false;
        }
    }
}