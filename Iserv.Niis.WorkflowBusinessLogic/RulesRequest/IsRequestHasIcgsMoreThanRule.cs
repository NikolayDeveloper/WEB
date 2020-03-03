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
    public class IsRequestHasIcgsMoreThanRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(int count = 1)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));

            var isIcgsMoreThan = request.ICGSRequests.Count > count;

            return isIcgsMoreThan;
        }
    }
}
