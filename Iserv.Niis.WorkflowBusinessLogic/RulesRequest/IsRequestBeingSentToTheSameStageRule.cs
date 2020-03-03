using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestBeingSentToTheSameStageRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval()
        {
            return !string.IsNullOrEmpty(WorkflowRequest.PrevStageCode) && !string.IsNullOrEmpty(WorkflowRequest.NextStageCode) && WorkflowRequest.PrevStageCode == WorkflowRequest.NextStageCode;
        }
    }
}
