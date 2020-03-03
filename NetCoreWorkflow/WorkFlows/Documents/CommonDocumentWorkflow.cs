using System;
using System.Linq;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.RulesDocument;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowDocuments;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;

namespace NetCoreWorkflow.WorkFlows.Documents
{
    /// <summary>
    /// Рабочий процесс по Документам
    /// </summary>
    public class CommonDocumentWorkflow : NetCoreBaseWorkflow<DocumentWorkFlowRequest, Document>
    {
        protected override string CurrentWorkflowStageCode => WorkflowRequest.PrevStageCode;

        public CommonDocumentWorkflow()
        {
            InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "73E7493E-E942-4377-B622-675F71CF8BB3")
                .UseForAllStages()
                .And<IsDocumentExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.PrevStageCode, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendDocumentToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "2010FEFB-F9D2-4027-89BB-D0DF1F3C0144")
                .UseForAllStages()
                .And<IsDocumentExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.PrevStageCode, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendDocumentToNextHandStage());
        }

        private Action<DocumentWorkFlowRequest> SendDocumentToNextHandStage()
        {
            return (_workflowRequest) =>
            {
                SendContractToNextStage(_workflowRequest.NextStageCode, _workflowRequest.PrevStageCode).Invoke();
            };
        }

        private Action SendContractToNextStage(string stageCode, string prevStageCode)
        {
            return () =>
            {
                var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(r => r.Execute(WorkflowRequest.CurrentWorkflowObject.Id));
                var curentWorkflow = document.CurrentWorkflows.Where(d => d.CurrentStage.Code == prevStageCode).FirstOrDefault();

                if (curentWorkflow == null)
                    throw new Exception("Не найден предыдущий этап");
               
                curentWorkflow.IsCurent = false;
                Executor.GetCommand<UpdateDocumentWokFlowCommand>().Process(r => r.Execute(curentWorkflow));


                var nextWorkFlow = CreateDocumentWorkflow(stageCode, curentWorkflow.CurrentStageId.GetValueOrDefault(0));
                nextWorkFlow.IsCurent = true;
                Executor.GetCommand<CreateDocumentWorkflowCommand>().Process(r => r.Execute(nextWorkFlow));

                //Теперь логика во флаге IsCurent
                //document.CurrentWorkflow = nextWorkFlow;
                //document.CurrentWorkflowId = nextWorkFlow.Id;
                //Executor.GetCommand<UpdateDocumentCommand>().Process(r => r.Execute(document));

                document = Executor.GetQuery<GetDocumentByIdQuery>().Process(r => r.Execute(document.Id));
            };
        }

        private DocumentWorkflow CreateDocumentWorkflow(string nextStageCode, int currentStageId)
        {
            var nextStage = Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(r => r.Execute(nextStageCode));
            return new DocumentWorkflow
            {
                OwnerId = WorkflowRequest.CurrentWorkflowObject.Id,
                CurrentStageId = nextStage.Id,
                CurrentUserId = WorkflowRequest.NextStageUserId,
                FromStageId = currentStageId,
                FromUserId = NiisAmbientContext.Current.User.Identity.UserId,
                RouteId = nextStage.RouteId,
                IsComplete = nextStage.IsLast,
                IsSystem = nextStage.IsSystem
            };
        }
    }
}