using System;
using NetCoreRules;

namespace NetCoreWorkflow
{
    public interface IWorkflowCurrentStageCodeCondition<TWorkflowRequest>
    {
        IWorkflowProcessor<TWorkflowRequest> WhenCurrentStageCode(string currentWorkflowStageCode);
        IWorkflowProcessor<TWorkflowRequest> UseForAllStages();
    }
}
