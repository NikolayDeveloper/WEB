using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsAnyDocumentHasOutgoingNumberByCodesRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] documentTypeCode)
        {
            var documents = Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>().Process(r => r.Execute(WorkflowRequest.RequestId, documentTypeCode));

            var isHasDocumentWithOutgoingNumber = documents.Any(r => string.IsNullOrEmpty(r.OutgoingNumber) == false);

            return isHasDocumentWithOutgoingNumber;
        }
    }
}
