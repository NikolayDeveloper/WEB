using System.Linq;
using Iserv.Niis.WorkflowBusinessLogic.ContractDocuments;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesContract
{
    public class IsAnyDocumentHasOutgoingNumberByCodesInContractRule : BaseRule<ContractWorkFlowRequest>
    {
        public bool Eval(string[] documentTypeCode)
        {
            var documents = Executor.GetQuery<GetDocumentsByContractIdAndTypeCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.ContractId, documentTypeCode));

            var isHasDocumentWithOutgoingNumber = documents.Any(r => string.IsNullOrEmpty(r.OutgoingNumber) == false);

            return isHasDocumentWithOutgoingNumber;
        }
    }
}