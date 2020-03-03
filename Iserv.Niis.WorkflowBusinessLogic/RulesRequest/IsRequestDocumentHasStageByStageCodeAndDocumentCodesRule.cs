using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestDocumentHasStageByStageCodeAndDocumentCodesRule : BaseRule<RequestWorkFlowRequest>
    {
        private readonly IRuleExecutor _ruleExecutor;
        public IsRequestDocumentHasStageByStageCodeAndDocumentCodesRule(IRuleExecutor ruleExecutor)
        {
            _ruleExecutor = ruleExecutor;
        }

        public bool Eval(string[] documentTypeCodes, string[] stageCodes)
        {
            if (WorkflowRequest.DocumentId == 0)
            {
                return Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>()
                    .Process(q => q.Execute(WorkflowRequest.RequestId, documentTypeCodes))
                    .Any(d => d.CurrentWorkflows.Any(cwf => stageCodes.Contains(cwf.CurrentStage.Code)));
            }

            var document = Executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.DocumentId));

            if (document == null)
            {
                return false;
            }
            if (!documentTypeCodes.Contains(document.Type.Code))
            {
                return false;
            }
            if (document.Requests.All(r => r.RequestId != WorkflowRequest.RequestId))
            {
                return false;
            }
            return document.CurrentWorkflows.Any(cwf => stageCodes.Contains(cwf.CurrentStage.Code));
        }
    }
}
