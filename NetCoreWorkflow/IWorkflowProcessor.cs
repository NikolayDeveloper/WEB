using System;
using NetCoreRules;

namespace NetCoreWorkflow
{
    public interface IWorkflowProcessor<TWorkflowRequest>
    {
        IWorkflowProcessor<TWorkflowRequest> And<TWorkflowStageRule>(Func<TWorkflowStageRule, bool> ruleFunc) where TWorkflowStageRule : BaseRule<TWorkflowRequest>;
        void Then(Action workflowStageAction);
        void ThenSendToNextHandStage(Action<TWorkflowRequest> workflowStageAction);
    }
}
