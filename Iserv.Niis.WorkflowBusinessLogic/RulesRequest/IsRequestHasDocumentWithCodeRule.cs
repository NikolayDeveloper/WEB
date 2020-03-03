using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasDocumentWithCodeRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(params string[] typeCodes)
        {
            return Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>().Process(r => r.Execute(WorkflowRequest.RequestId, typeCodes)).Any();
        }
    }
}