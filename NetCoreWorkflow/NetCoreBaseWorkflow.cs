using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.WorkflowBusinessLogic;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
//using NetCoreDI;
using NetCoreRules;

namespace NetCoreWorkflow
{
    public abstract class NetCoreBaseWorkflow<TWorkflowRequest, TEntity> where TWorkflowRequest : WorkflowRequest<TEntity> where TEntity : class, new()
    {
        private IRuleExecutor _ruleExecutor;
        protected IRuleExecutor RuleExecutor => _ruleExecutor ?? (_ruleExecutor = AmbientContext.Current.Resolver.ResolveObject<IRuleExecutor>());

        private IExecutor _executor;
        protected IExecutor Executor => _executor ?? (_executor = AmbientContext.Current.Resolver.ResolveObject<IExecutor>());

        protected List<WorkflowStageObject<TWorkflowRequest>> WorkflowStageObjects;

        //todo: используется для создания логики перехода на этап надо придумать нормальное название
        protected WorkflowStageObject<TWorkflowRequest> CurrentWorkflowStageObject;

        protected TWorkflowRequest WorkflowRequest { get; private set; }

        protected abstract string CurrentWorkflowStageCode { get; }


        protected NetCoreBaseWorkflow()
        {
            WorkflowStageObjects = new List<WorkflowStageObject<TWorkflowRequest>>();
        }

        protected IWorkflowCurrentStageCodeCondition<TWorkflowRequest> WorkflowStage(string workflowStageDescription, string uniqStageId)
        {
            CurrentWorkflowStageObject = new WorkflowStageObject<TWorkflowRequest>(RuleExecutor, WorkflowStageObjects, uniqStageId, workflowStageDescription);
            return CurrentWorkflowStageObject;
        }

        public void SetWorkflowRequest(TWorkflowRequest workflowRequest)
        {
            WorkflowRequest = workflowRequest;
        }

        public void Process()
        {
            var currentCodeWorkflowStages = WorkflowStageObjects.Where(s => s.IsCommonStage
                                                                || s.CurrentWorkflowStageCode == CurrentWorkflowStageCode
                                                                || IsCurrentWorkflowStageCodeNotDefined);

            foreach (var workflowStage in currentCodeWorkflowStages)
            {
                var canWorkflowStageActionExecute = true;
                foreach (var stageRule in workflowStage.WorkflowStageRules)
                {
                    stageRule.RuleExecutor.InitRule(r => r.SetWorkflowRequest(WorkflowRequest));

                    var isStageRulesReturnTrue = stageRule.RuleExecutor.Process(stageRule.RuleFunc);
                    if (isStageRulesReturnTrue == false)
                    {
                        canWorkflowStageActionExecute = false;
                        break;
                    }
                }

                if (canWorkflowStageActionExecute)
                {
                    workflowStage.HandWorkflowStageAction?.Invoke(WorkflowRequest);

                    workflowStage.WorkflowStageAction?.Invoke();

                    return;
                }
            }
        }

        private bool IsCurrentWorkflowStageCodeNotDefined => string.IsNullOrEmpty(CurrentWorkflowStageCode);
    }
}
