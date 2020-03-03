using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] documentTypeCodes)
        {
            foreach (var documentTypeCode in documentTypeCodes)
            {
                var documents = Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>().Process(r => r.Execute(WorkflowRequest.RequestId, new[] { documentTypeCode }));

                var isHasDocumentWithOutgoingNumberAndSendingDate = documents.Any(r => string.IsNullOrEmpty(r.OutgoingNumber) == false 
                && r.SendingDate.HasValue == true
                && r.DateCreate >= WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.DateCreate);
                if (isHasDocumentWithOutgoingNumberAndSendingDate == true)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
