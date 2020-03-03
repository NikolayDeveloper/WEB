using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasPreviousStageByCodeRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string previousStageCode)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));

            return request?.CurrentWorkflow?.FromStage?.Code == previousStageCode;
        }
    }
}
