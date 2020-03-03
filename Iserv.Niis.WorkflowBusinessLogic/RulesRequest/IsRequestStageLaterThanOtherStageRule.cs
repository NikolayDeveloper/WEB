using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestStageLaterThanOtherStageRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string stageCode, string otherStageCode)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));
            var requestWorkflows = Executor.GetQuery<GetRequestWorkflowsByRequestIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.RequestId));

            var stage = requestWorkflows.LastOrDefault(rw => rw.CurrentStage.Code == stageCode);

            var otherStage = requestWorkflows.LastOrDefault(rw => rw.CurrentStage.Code == otherStageCode);

            if (stage == null) return false;
            return otherStage == null || stage.DateCreate > otherStage.DateCreate;
        }
    }
}
