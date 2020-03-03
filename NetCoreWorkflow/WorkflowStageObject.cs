using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreRules;

namespace NetCoreWorkflow
{
    public class WorkflowStageObject<TWorkflowRequest> : IWorkflowCurrentStageCodeCondition<TWorkflowRequest>, IWorkflowProcessor<TWorkflowRequest>
    {
        private readonly IRuleExecutor _ruleExecutor;
        internal TWorkflowRequest WorkflowRequest { get; set; }

        private string _workflowStageDescription;

        private readonly string _uniqStageId;
        private bool IsStageUniqGuidAlreadyExists => _workflowStageObjects.Any(r => r._uniqStageId == _uniqStageId);

        public string CurrentWorkflowStageCode { get; private set; }

        public bool IsCommonStage { get; private set; }

        public readonly List<WorkflowRule<TWorkflowRequest, BaseRule<TWorkflowRequest>>> WorkflowStageRules;
        public Action WorkflowStageAction;
        public Action<TWorkflowRequest> HandWorkflowStageAction;

        /// <summary>
        /// список из NetCoreBaseWorkflow
        /// </summary>
        private readonly List<WorkflowStageObject<TWorkflowRequest>> _workflowStageObjects;

        public WorkflowStageObject(IRuleExecutor ruleExecutor, List<WorkflowStageObject<TWorkflowRequest>> workflowStageObjects, string uniqStageId, string workflowStageDescription)
        {
            _workflowStageDescription = workflowStageDescription;
            _workflowStageObjects = workflowStageObjects;
            _uniqStageId = uniqStageId;

            _ruleExecutor = ruleExecutor;
            WorkflowStageRules = new List<WorkflowRule<TWorkflowRequest, BaseRule<TWorkflowRequest>>>();
        }

        public IWorkflowProcessor<TWorkflowRequest> UseForAllStages()
        {
            IsCommonStage = true;
            return this;
        }

        public IWorkflowProcessor<TWorkflowRequest> WhenCurrentStageCode(string currentWorkflowStageCode)
        {
            CurrentWorkflowStageCode = currentWorkflowStageCode;
            return this;
        }

        public IWorkflowProcessor<TWorkflowRequest> And<TWorkflowStageRule>(Func<TWorkflowStageRule, bool> ruleFunc) where TWorkflowStageRule : BaseRule<TWorkflowRequest>
        {
            AddRule(ruleFunc);
            return this;
        }
        public void Then(Action workflowStageAction)
        {
            WorkflowStageAction = workflowStageAction;
            if (IsStageUniqGuidAlreadyExists)
            {
                throw new Exception("Идентификаторы этапов должны быть уникальными");
            }

            _workflowStageObjects.Add(this);
        }

        public void ThenSendToNextHandStage(Action<TWorkflowRequest> handWorkflowStageAction)
        {
            HandWorkflowStageAction = handWorkflowStageAction;
            if (IsStageUniqGuidAlreadyExists)
            {
                throw new Exception("Идентификаторы этапов должны быть уникальными");
            }

            _workflowStageObjects.Add(this);
        }

        private void AddRule<TWorkflowStageRule>(Func<TWorkflowStageRule, bool> ruleFunc) where TWorkflowStageRule : BaseRule<TWorkflowRequest>
        {
            var ruleExeutor = _ruleExecutor.GetRule<TWorkflowStageRule>();
            var workflowRuleStore = new WorkflowRule<TWorkflowRequest, BaseRule<TWorkflowRequest>>(ruleExeutor, rule => ruleFunc((TWorkflowStageRule)rule));
            WorkflowStageRules.Add(workflowRuleStore);
        }
    }
}
