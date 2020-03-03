using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasNotRouteStagesByCodesRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] routeStageCodes)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));
            var requestWorkflows = Executor.GetQuery<GetRequestWorkflowsByRequestIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.RequestId));

            return requestWorkflows.All(w => routeStageCodes.Contains(w.CurrentStage.Code) == false);
        }
    }
}
