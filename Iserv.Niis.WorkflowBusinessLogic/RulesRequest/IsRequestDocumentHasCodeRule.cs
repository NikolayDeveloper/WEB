using System.Linq;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestDocumentHasCodeRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] typeCodes)
        {
            var document = Executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.DocumentId));

            if (document == null)
            {
                return false;
            }
            if (document.Requests.All(r => r.RequestId != WorkflowRequest.RequestId))
            {
                return false;
            }

            return typeCodes.Contains(document.Type.Code);
        }
    }
}