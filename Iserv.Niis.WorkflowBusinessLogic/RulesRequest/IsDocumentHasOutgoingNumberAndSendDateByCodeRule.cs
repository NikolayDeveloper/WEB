using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsDocumentHasOutgoingNumberAndSendDateByCodeRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string documentTypeCode)
        {
            var document = Executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.DocumentId));

            if (document == null)
            {
                return false;
            }
            if (document.Type.Code != documentTypeCode)
            {
                return false;
            }
            if (document.Requests.All(r => r.RequestId != WorkflowRequest.RequestId))
            {
                return false;
            }

            var isHasDocumentWithOutgoingNumberAndSendingDate = !string.IsNullOrEmpty(document.OutgoingNumber) && document.SendingDate.HasValue;

            return isHasDocumentWithOutgoingNumberAndSendingDate;
        }
    }
}
