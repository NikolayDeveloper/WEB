using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasNotDocumentWithCodeRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(params string[] typeCodes)
        {
            var hasDocument = Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>().Process(r => r.Execute(WorkflowRequest.RequestId, typeCodes)).Any();
            if (hasDocument)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}