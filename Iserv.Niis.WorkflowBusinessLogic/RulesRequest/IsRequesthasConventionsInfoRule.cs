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
    public class IsRequesthasConventionsInfoRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval()
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));

            var isRequestHasConventions = request.RequestConventionInfos.Any(ci =>
                ci.CountryId.HasValue && (!new[] {"01", "02"}.Contains(request.RequestType?.Code)
                || ci.DateInternationalApp.HasValue
                && !string.IsNullOrEmpty(ci.RegNumberInternationalApp)));

            return isRequestHasConventions;
        }
    }
}
