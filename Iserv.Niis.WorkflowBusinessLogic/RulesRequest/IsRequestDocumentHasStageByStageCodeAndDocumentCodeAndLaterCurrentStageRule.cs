using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule : BaseRule<RequestWorkFlowRequest>
    {
        private IRuleExecutor _ruleExecutor;
        public IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule(IRuleExecutor ruleExecutor)
        {
            _ruleExecutor = ruleExecutor;
        }

        public bool Eval(string[] documentTypeCodes, string stageCode)
        {
            if (WorkflowRequest.DocumentId == 0)
            {
                return Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>()
                    .Process(q => q.Execute(WorkflowRequest.RequestId, documentTypeCodes))
                    .Any(d => d.CurrentWorkflows.Any(cwf => cwf.CurrentStage.Code.Equals(stageCode) && cwf.DateCreate > WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.DateCreate));
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
            return document.CurrentWorkflows.Any(cwf => cwf.CurrentStage.Code == stageCode && cwf.DateCreate > WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.DateCreate);
            //return document.CurrentWorkflow.CurrentStage.Code == stageCode && IsDocumentLaterCurrentStage(document);
        }
    }
}
