using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasAnyMainIpcRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval()
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));
            return request != null ? request.IPCRequests.Any(i => i.IsMain) : false;
        }
    }
}
