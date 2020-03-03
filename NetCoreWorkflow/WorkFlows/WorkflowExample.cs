using System;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;

namespace NetCoreWorkflow.WorkFlows
{
    public class WorkflowExample : NetCoreBaseWorkflow<RequestWorkFlowRequest, Request>
    {
        public WorkflowExample()
        {
            InitPaymentFlow();
        }

        private void InitPaymentFlow()
        {
            WorkflowStage("Применение оплаты на услуги Проведение экспертизы заявки на регистрацию товарных знаков, знаков обслуживания", "78063D58-DC59-4AA4-8C06-E664D8574A5D")
                .WhenCurrentStageCode("TM03.2.2")
                //.And<RequestHasDocumentWithTypeRule>(r => r.Eval(DicDocumentTypeCodes.NotificationOfTrademarkRequestRegistation))
                .Then(SendRequestToNextStage("TM03.2.2.3"));

            WorkflowStage("Применение оплаты на услуги Проведение экспертизы заявки на регистрацию товарных знаков, знаков обслуживания", "78063D58-DC59-4AA4-8C06-E664D8574A5D")
                .WhenCurrentStageCode("TM03.2.2")
                //.And<RequestHasDocumentWithTypeRule>(r => r.Eval(DicDocumentTypeCodes.NotificationOfTrademarkRequestRegistation))
                .Then(SendRequestToNextStage("TM03.2.2.3"));

            WorkflowStage("Применение оплаты на услуги Проведение экспертизы заявки на регистрацию товарных знаков, знаков обслуживания", "01F97C80-319D-4286-AD93-A487DC96AF1B")
                .WhenCurrentStageCode("TM03.2.2.1")
                .Then(SendRequestToNextStage("TM03.2.2.3"));

            WorkflowStage("Применение оплаты на услуги Проведение экспертизы заявки на регистрацию товарных знаков, знаков обслуживания", "D55F6B2D-C5BF-4CEB-957B-10DAB20EC74D")
                .WhenCurrentStageCode("TM03.3.7.0")
                //.And<RequestHasDocumentWithTypeRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForRestoreTime))
                //.And<RequestHasPaidInvoiceWithTariffCodeRule>(r => r.Eval(DicTariff.Codes.RequestAnswerMissedTimeRestoration))
                .Then(SendRequestToNextStage("TM03.2.2.3"));
        }

        private static Action SendRequestToNextStage(string nextStageCode)
        {
            return () =>
            {
                //Executor.GetCommand<>() //Обрабатываем изменение статуса
            };
        }

        protected override string CurrentWorkflowStageCode => WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentStage.Code;
    }
}
