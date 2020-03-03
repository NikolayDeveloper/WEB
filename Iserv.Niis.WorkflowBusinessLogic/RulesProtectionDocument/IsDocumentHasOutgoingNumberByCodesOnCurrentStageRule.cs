using System.Linq;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument
{
    public class IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule : BaseRule<ProtectionDocumentWorkFlowRequest>
    {
        public bool Eval(string[] documentTypeCodes)
        {
            var documents = Executor.GetQuery<GetDocumentsByProtectionDocIdAndTypeCodesQuery>()
                .Process(q => q.Execute(WorkflowRequest.ProtectionDocId, documentTypeCodes));
            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(WorkflowRequest.ProtectionDocId));
            if (protectionDoc == null)
            {
                return false;
            }

            var isHasDocumentWithOutgoingNumber = documents.Any(d => d.CurrentWorkflows.All(cwf => cwf.DateCreate > protectionDoc.CurrentWorkflow.DateCreate) &&
                !string.IsNullOrEmpty(d.OutgoingNumber));

            return isHasDocumentWithOutgoingNumber;
        }
    }
}
