using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument
{
    public class IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule:BaseRule<ProtectionDocumentWorkFlowRequest>
    {
        public bool Eval(params string[] typeCodes)
        {
            var documents = Executor.GetQuery<GetDocumentsByProtectionDocIdAndTypeCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.ProtectionDocId, typeCodes));
            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(WorkflowRequest.ProtectionDocId));
            if (protectionDoc == null)
            {
                return false;
            }
            return documents.Any(d => d.DateCreate > protectionDoc.CurrentWorkflow.DateCreate);
        }
    }
}
