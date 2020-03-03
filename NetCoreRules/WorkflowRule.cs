using System;

namespace NetCoreRules
{
    public class WorkflowRule<TWorkflowRequest, TWorkflowStageRule> where TWorkflowStageRule : BaseRule<TWorkflowRequest>
    {
        public WorkflowRule(IRuleExecutor<TWorkflowStageRule> ruleExecutor, Func<BaseRule<TWorkflowRequest>, bool> ruleFunc)
        {
            RuleExecutor = ruleExecutor;
            RuleFunc = ruleFunc;
        }

        public IRuleExecutor<TWorkflowStageRule> RuleExecutor { get; }
        public Func<TWorkflowStageRule, bool> RuleFunc { get; }
    }
}
