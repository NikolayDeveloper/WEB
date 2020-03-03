using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument
{
    public class DoesDocumentHaveOutgoingNumberByCodesRule: BaseRule<ProtectionDocumentWorkFlowRequest>
    {
        public bool Eval(string[] codes)
        {
            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.ProtectionDocId));

            if (protectionDoc == null)
            {
                return false;
            }

            var hasDocumentWithOutgoingNumberByCode = protectionDoc.Documents.Any(d =>
                codes.Contains(d.Document.Type.Code) && !string.IsNullOrEmpty(d.Document.OutgoingNumber));

            return hasDocumentWithOutgoingNumberByCode;
        }
    }
}
