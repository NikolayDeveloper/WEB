using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.RulesRequest;

namespace NetCoreWorkflow.WorkFlows.Requests
{
    /// <summary>
    /// Рабочий процесс по заявке "Международные товарные знаки"
    /// </summary>
    public class RequestInternationalTradeMarkWorkflow : BaseRequestWorkflow
    {
        public RequestInternationalTradeMarkWorkflow(
            IIntegrationStatusUpdater integrationStatusUpdater,
            IIntegrationDocumentUpdater integrationDocumentUpdater,
            IMapper mapper) : base(integrationStatusUpdater, integrationDocumentUpdater, mapper)
        {
            //InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Переход по текущему этапу вручную", "A66D2ECC-B8DB-4DFA-A9EE-85252BA7B15C")
                .UseForAllStages()
                .And<IsRequestBeingSentToTheSameStageRule>(r => r.Eval())
                .Then(SendRequestToSameStage());
            WorkflowStage("Из любого этапа на этап 'Отозвано'", "87D53C60-271B-4BFA-A78B-787A861206A7")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionOfApplicationRevocation }))
                .Then(SendRequestToNextStage(RouteStageCodes.RequestCanceled));

            WorkflowStage("Из любого этапа на этап 'Делопроизводство приостановлено'", "F10188E8-155B-41DB-BE1E-05552FEF807F")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForSuspensionOfOfficeWork }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_2_1));

            WorkflowStage("Ручной переход этапов", "4E03EDD7-85BF-4171-ABBF-DE51FCB2B48C")
                .UseForAllStages()
                .And<IsRequestExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "E0EA4676-37F0-4E7C-AB76-CBD1055B5A8E")
                .UseForAllStages()
                .And<IsRequestExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            //TODO
            WorkflowStage("Из этапа 'Ожидание срока рассмотрения заявки' на этап 'Выбор исполнителя полной экспертизы МТЗ'", "CD56BB75-6060-421A-9D4A-BFAC954008F5")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_1)
                //.And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval())
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_1));

            WorkflowStage("Из этапа 'Выбор исполнителя полной экспертизы МТЗ' на этап 'Вынесено экспертное заключение об охране'", "2D58989D-B2B0-4AB3-85B0-B7CEAD7B32CA")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_2)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PreliminaryPositiveExpertConclusion }, RouteStageCodes.DocumentOutgoing_02_1))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_3));

            WorkflowStage("Из этапа 'Вынесено экспертное заключение об охране' на этап 'Вынесено экспертное заключение об отказе'", "7D1560C1-7250-4828-905E-BD78E923E64D")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_2)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(
                    new[] {
                        DicDocumentTypeCodes.NegativeRegistrationExpertConclusion,
                        DicDocumentTypeCodes.PreliminaryRejection,
                        DicDocumentTypeCodes.PreliminaryRejectionDisclamated,
                        DicDocumentTypeCodes.PreliminaryPartialRejection,
                        DicDocumentTypeCodes.Rejection,
                        DicDocumentTypeCodes.PartialRejection }, RouteStageCodes.DocumentOutgoing_02_1))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_3_1));

            WorkflowStage("Из этапа 'Утверждено директором' на этап 'Публикация заключения об отказе в ВОИС'", "D81D8D9D-08E6-45E3-BBEC-E82C4C850EC3")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_4)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] {
                        DicDocumentTypeCodes.NegativeRegistrationExpertConclusion,
                        DicDocumentTypeCodes.PreliminaryRejection,
                        DicDocumentTypeCodes.PreliminaryRejectionDisclamated,
                        DicDocumentTypeCodes.PreliminaryPartialRejection,
                        DicDocumentTypeCodes.Rejection,
                        DicDocumentTypeCodes.PartialRejection }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_4_1_0));

            WorkflowStage("Из этапа 'Публикация заключения об отказе в ВОИС' на этап 'Продление срока'", "3372FA0A-4FDD-4338-9C29-0EAAA5F91698")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_4_1_0)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForExtendTimeRorObjections }))
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.ExpertiseConclusionObjectionTermExtensionMonthly }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_4_5_1));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'", "C8840B7B-6AF3-4530-8F73-F5532BD1EF5C")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_2_0)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForExtendTimeRorResponse }))
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.RequestAnswerTimeExtensionForMonth }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_4_5_1));

            WorkflowStage("Из этапа 'Публикация заключения об отказе в ВОИС' на этап 'Возражение на предварительный или частичный отказ'", "F6A027AA-3339-4545-B3FD-38C148819F03")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_4_1_0)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Objection }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_5));

            WorkflowStage("Из этапа 'Продление срока' на этап 'Возражение на предварительный или частичный отказ'", "600C402E-04B8-4D76-8FF7-75B30BB1D15A")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_4_5_1)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Objection }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_5));

            WorkflowStage("Из этапа 'Направлен запрос' на этап основного сценария",
                    "09D37DE6-5589-4DB6-9C20-7CBA031B3E9A")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_2_0)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(ReturnFromSendRequestScenario());

            WorkflowStage("Из этапа 'Публикация заключения об отказе в ВОИС' на этап 'Заключение окончательное по истечении срока'", "3DB6C07B-60A6-4B9F-8015-4C6F5CD995D7")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_4_1_0)
                .And<IsRequestExpiredOnCurrentStageRule>(r=>r.Eval(RouteStageCodes.ITZ_03_3_4_1_1))
                .And<IsRequestHasNotDocumentWithCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Objection }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_4_1_1));
            
            WorkflowStage("Из этапа 'Возражение на предварительный или частичный отказ' на этап 'Вынесение окончательного решения'", "067B65F0-8998-4687-BCE5-0B747EF30108")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_5)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] {
                        DicDocumentTypeCodes.FinalPositiveExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusionPatent }, RouteStageCodes.DocumentOutgoing_02_1))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_8));

            WorkflowStage("Из этапа 'Заключение окончательное по истечении срока' на этап 'Вынесение окончательного решения'", "F426C7C0-92B8-417A-BCC2-B0F7FEF8BB85")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_4_1_1)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] {
                        DicDocumentTypeCodes.FinalPositiveExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusionPatent }, RouteStageCodes.DocumentOutgoing_02_1))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_8));

            WorkflowStage("Из этапа 'Вынесение окончательного решения' на этап 'На утверждение директору окончательных решений'", "5701304F-BF67-4673-A43C-26A6264BE88F")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_8)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] {
                        DicDocumentTypeCodes.FinalPositiveExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusionPatent }, RouteStageCodes.DocumentOutgoing_02_3))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_9));

            WorkflowStage("Из этапа 'Возражение на предварительный или частичный отказ' на этап 'Направлен запрос'", "83D33B46-FB3F-45AD-922A-25C740D1C23B")
                 .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_5)
                 .And<IsDocumentHasOutgoingNumberAndSendDateByCodeRule>(r => r.Eval(DicDocumentTypeCodes.NotificationOfTerminationWorkMTZ_1))
                 .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_2_0));

            WorkflowStage("Из этапа 'Вынесено экспертное заключение об охране' на этап 'Утверждено директором'", "BDA65F2C-171D-41AA-8DE0-C662955F1C85")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_3)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] {
                        DicDocumentTypeCodes.PreliminaryPositiveExpertConclusion,
                        DicDocumentTypeCodes.NegativeRegistrationExpertConclusion,
                        DicDocumentTypeCodes.PreliminaryRejection,
                        DicDocumentTypeCodes.PreliminaryRejectionDisclamated,
                        DicDocumentTypeCodes.PreliminaryPartialRejection,
                        DicDocumentTypeCodes.Rejection,
                        DicDocumentTypeCodes.PartialRejection,
                        DicDocumentTypeCodes.FinalPositiveExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusionPatent}, RouteStageCodes.DocumentOutgoing_03_1))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_4));

            WorkflowStage("Из этапа 'Вынесено экспертное заключение об отказе' на этап 'Утверждено директором'", "970CF06C-41ED-483C-BC0C-8B1310CBC4F3")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_3_1)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] {
                        DicDocumentTypeCodes.PreliminaryPositiveExpertConclusion,
                        DicDocumentTypeCodes.NegativeRegistrationExpertConclusion,
                        DicDocumentTypeCodes.PreliminaryRejection,
                        DicDocumentTypeCodes.PreliminaryRejectionDisclamated,
                        DicDocumentTypeCodes.PreliminaryPartialRejection,
                        DicDocumentTypeCodes.Rejection,
                        DicDocumentTypeCodes.PartialRejection,
                        DicDocumentTypeCodes.FinalPositiveExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusionPatent}, RouteStageCodes.DocumentOutgoing_03_1))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_4));

            WorkflowStage("Из этапа 'На утверждение директору окончательных решений' на этап 'Утверждено директором'", "5A99FF8E-8668-4D58-B86D-FDDD2B447842")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_9)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] {
                        DicDocumentTypeCodes.PreliminaryPositiveExpertConclusion,
                        DicDocumentTypeCodes.NegativeRegistrationExpertConclusion,
                        DicDocumentTypeCodes.PreliminaryRejection,
                        DicDocumentTypeCodes.PreliminaryRejectionDisclamated,
                        DicDocumentTypeCodes.PreliminaryPartialRejection,
                        DicDocumentTypeCodes.Rejection,
                        DicDocumentTypeCodes.PartialRejection,
                        DicDocumentTypeCodes.FinalPositiveExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusion,
                        DicDocumentTypeCodes.FinalNegativeExpertConclusionPatent}, RouteStageCodes.DocumentOutgoing_03_1))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_4));

            WorkflowStage("Из этапа 'Утверждено директором' на этап 'Передано в МЮ РК'", "D08DDC67-2D6F-41B9-B10F-CAE1CFEC9F51")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.RegistersOfExpertOpinionsInMjOfRk))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_4_2));

            WorkflowStage("Из этапа 'Передано в МЮ РК' на этап 'Возвращено с МЮ РК'", "564BD392-5AAE-4148-A300-1CE7B032BEF3")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_4_2)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.DecisionOfAuthorizedBody }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_4_3));

            WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Публикация в ВОИС'", "B51573BE-EB6C-4646-ADDF-98BBD97D9220")
                .WhenCurrentStageCode(RouteStageCodes.ITZ_03_3_4_3)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesRule>(r => r.Eval(new[] { DicDocumentTypeCodes.ItmAccompanyingNote, DicDocumentTypeCodes.ItmAccompanyingNoteProtection }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_4_5));
        }
    }
}
