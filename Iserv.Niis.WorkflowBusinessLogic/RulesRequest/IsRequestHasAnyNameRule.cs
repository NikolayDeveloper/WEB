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
    public class IsRequestHasAnyNameRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval()
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));

            var isRequestHasAnyName = !string.IsNullOrEmpty(request.NameRu)
                || !string.IsNullOrEmpty(request.NameEn)
                || !string.IsNullOrEmpty(request.NameKz);

            return isRequestHasAnyName;
        }
    }
}
