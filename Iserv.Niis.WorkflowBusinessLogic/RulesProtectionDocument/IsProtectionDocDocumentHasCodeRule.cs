using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument
{
    public class IsProtectionDocDocumentHasCodeRule: BaseRule<ProtectionDocumentWorkFlowRequest>
    {
        public bool Eval(string[] typeCodes)
        {
            var document = Executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.DocumentId));

            if (document == null)
            {
                return false;
            }
            if (document.ProtectionDocs.All(r => r.ProtectionDocId != WorkflowRequest.ProtectionDocId))
            {
                return false;
            }

            return typeCodes.Contains(document.Type.Code);
        }
    }
}
