using System;
using System.Linq;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.WorkflowBusinessLogic.Contracts;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.RulesContract;

namespace NetCoreWorkflow.WorkFlows.Contracts
{
    public class BaseContractWorkflow : NetCoreBaseWorkflow<ContractWorkFlowRequest, Contract>
    {
        public BaseContractWorkflow()
        {
            InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "73E7493E-E942-4377-B622-675F71CF8BB3")
                .UseForAllStages()
                .And<IsContractExistsNextRouteStageByCodeRule>(r =>
                    r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code,
                        WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendContractDocumentToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "126BE89D-985B-47A9-8CEE-8924E21BE6E6")
                .UseForAllStages()
                .And<IsContractExistsPreviousRouteStageByCodeRule>(r =>
                    r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code,
                        WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendContractDocumentToNextHandStage());
        }

        protected override string CurrentWorkflowStageCode => WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow?.CurrentStage?.Code;

        protected Action<ContractWorkFlowRequest> SendContractDocumentToNextHandStage()
        {
            return (_workflowRequest) =>
            {
                SendContractToNextStage(_workflowRequest.NextStageCode).Invoke();
            };
        }

        protected Action SendContractToNextStage(string stageCode)
        {
            return () =>
            {
                var nextWorkFlow = CreateRequestWorkFlow(stageCode);
                Executor.GetCommand<CreateContractWorkflowCommand>().Process(r => r.Execute(nextWorkFlow));

                var contract = Executor.GetQuery<GetContractByIdQuery>().Process(r => r.Execute(WorkflowRequest.CurrentWorkflowObject.Id));
                contract.CurrentWorkflow = nextWorkFlow;
                contract.CurrentWorkflowId = nextWorkFlow.Id;

                Executor.GetCommand<UpdateContractCommand>().Process(r => r.Execute(contract));
                contract = Executor.GetQuery<GetContractByIdQuery>().Process(r => r.Execute(contract.Id));


                Executor.GetCommand<UpdateMarkAsExecutedWorkflowTaskEvenstByRequestIdCommand>().Process(r => r.Execute(contract.Id));

                #region Планирование автоэтапов

                var nextStageOrders = Executor.GetQuery<GetNextDicRouteStageOrdersByCodeQuery>().Process(r => r.Execute(contract.CurrentWorkflow.CurrentStage.Code));

                var automaticStages = nextStageOrders.Where(r => r.IsAutomatic == true);

                foreach (var autoStageOrder in automaticStages)
                {
                    var now = NiisAmbientContext.Current.DateTimeProvider.Now;

                    var workflowTaskQueue = new WorkflowTaskQueue
                    {
                        ContractId = contract.Id,
                        ResolveDate = now.AddMinutes(5), //TODO: специальное вычисление даты уведомления
                        ResultStageId = autoStageOrder.NextStageId,
                        ConditionStageId = autoStageOrder.CurrentStage.Id,
                    };

                    Executor.GetCommand<CreateWorkflowTaskQueueCommand>().Process(r => r.Execute(workflowTaskQueue));
                }
                #endregion
            };
        }

        private Iserv.Niis.Domain.Entities.Contract.ContractWorkflow CreateRequestWorkFlow(string nextStageCode)
        {
            var nextStage = Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(r => r.Execute(nextStageCode));

            return new Iserv.Niis.Domain.Entities.Contract.ContractWorkflow
            {
                CurrentUserId = WorkflowRequest.NextStageUserId,
                OwnerId = WorkflowRequest.CurrentWorkflowObject.Id,
                CurrentStageId = nextStage.Id,
                FromStageId = WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentStageId,
                FromUserId = NiisAmbientContext.Current.User.Identity.UserId,
                RouteId = nextStage.RouteId,
                IsComplete = nextStage.IsLast,
                IsSystem = nextStage.IsSystem,
                IsMain = nextStage.IsMain
            };
        }
    }
}