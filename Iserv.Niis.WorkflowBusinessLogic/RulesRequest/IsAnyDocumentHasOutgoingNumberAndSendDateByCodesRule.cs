using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsAnyDocumentHasOutgoingNumberAndSendDateByCodesRule : BaseRule<RequestWorkFlowRequest>
    {
        private readonly IRuleExecutor _ruleExecutor;
        public IsAnyDocumentHasOutgoingNumberAndSendDateByCodesRule(IRuleExecutor ruleExecutor)
        {
            _ruleExecutor = ruleExecutor;
        }

        public bool Eval(string[] documentTypeCodes)
        {
            foreach (var documentTypeCode in documentTypeCodes)
            {
                var documents = Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>().Process(r => r.Execute(WorkflowRequest.RequestId, new[] { documentTypeCode }));

                var isHasDocumentWithOutgoingNumberAndSendingDate = documents.Any(r => string.IsNullOrEmpty(r.OutgoingNumber) == false && r.SendingDate.HasValue == true);
                
                if(isHasDocumentWithOutgoingNumberAndSendingDate == true)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
