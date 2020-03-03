using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsDocumentSignedAtStageRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] documentCodes, string documentStageCode)
        {
            var document = Executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.DocumentId));
            if (document == null)
            {
                return false;
            }
            if (!documentCodes.Contains(document.Type.Code))
            {
                return false;
            }
            if (document.Requests.All(r => r.RequestId != WorkflowRequest.RequestId))
            {
                return false;
            }

            return document.CurrentWorkflows.Any(cwf => cwf.CurrentStage.Code == documentStageCode && cwf.DocumentUserSignature != null);
        }
    }
}
