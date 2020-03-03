using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;
using Iserv.Niis.WorkflowBusinessLogic.Requests;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasAnySelectionAchieveTypeCodesRule :  BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string [] protectionDocSubTypeCodes)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));

            var result = protectionDocSubTypeCodes.Contains(request.SelectionAchieveType?.Code ?? string.Empty);

            return result;
        }
    }
}
