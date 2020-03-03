using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.RulesRequest;

namespace NetCoreWorkflow.WorkFlows.Requests
{
    /// <summary>
    /// Рабочий процесс по заявке "Селекционные достижения"
    /// </summary>
    public class RequestSelectiveAchievementsWorkflow : BaseRequestWorkflow
    {
        public RequestSelectiveAchievementsWorkflow(
            IIntegrationStatusUpdater integrationStatusUpdater,
            IIntegrationDocumentUpdater integrationDocumentUpdater,
            IMapper mapper) : base(integrationStatusUpdater, integrationDocumentUpdater, mapper)
        {
            //InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "6582E8E3-A856-4FAF-8448-9558488CAFAC")
                .UseForAllStages()
                .And<IsRequestExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Из любого этапа на этап 'Отозвано'", "87D53C60-271B-4BFA-A78B-787A861206A7")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionOfApplicationRevocation }))
                .Then(SendRequestToNextStage(RouteStageCodes.RequestCanceled));

            WorkflowStage("Из любого этапа на этап 'Делопроизводство приостановлено'", "F10188E8-155B-41DB-BE1E-05552FEF807F")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForSuspensionOfOfficeWork }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_2_1));

            WorkflowStage("Ручной возврат этапов", "7B03DAE6-054D-40CF-BE7C-DDC8708CCD28")
                .UseForAllStages()
                .And<IsRequestExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Переход по текущему этапу вручную", "28B1C7E9-6F0D-41FE-9A45-40F558B9086A")
                .UseForAllStages()
                .And<IsRequestBeingSentToTheSameStageRule>(r => r.Eval())
                .Then(SendRequestToSameStage());

            WorkflowStage("Из этапа 'Ввод оплаты' на этап 'Готовые для передачи на предварительную экспертизу'", "8070453F-76C7-4428-AA2A-F7130C9D75A3")
                .WhenCurrentStageCode(RouteStageCodes.SA_02_2)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.SelectiveAchievementsExaminationDigital, DicTariff.Codes.SelectiveAchievementsExaminationPaper }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_02_2_0));

            WorkflowStage("Из этапа 'Ожидаемые оплату за подачу' на этап 'Готовые для передачи на предварительную экспертизу'", "F562DEF4-D64A-4D26-8B59-7DD11BAE0486")
                .WhenCurrentStageCode(RouteStageCodes.SA_02_2_1)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.SelectiveAchievementsExaminationDigital, DicTariff.Codes.SelectiveAchievementsExaminationPaper }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_02_2_0));

            WorkflowStage("Из этапа 'Ввод оплаты' на этап 'Ожидаемые оплату за подачу'", "C2D7C5E4-C521-427F-8900-4A77CB0FC4A5")
                .WhenCurrentStageCode(RouteStageCodes.SA_02_2)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PaymentInvoice }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_02_2_1));

            WorkflowStage("Из этапа 'Запрос предварительной экспертизы' на этап 'Предварительная экспертиза'", "4FAC4568-1D4A-447B-B37B-56294FB18894")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_2_1)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_2));

            WorkflowStage("Из этапа 'Предварительная экспертиза' на этап 'Направлены в Госкомиссию на проверку наименования'", "D020AC92-5FDB-441F-9380-2FBB9580700C")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_2)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SA_1_SOPR))
                .And<IsRequestHasAnySelectionAchieveTypeCodesRule>(r => r.Eval(new[] { DicSelectionAchieveTypeCodes.Agricultural }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_1));

            WorkflowStage("Из этапа 'Предварительная экспертиза' на этап 'Направлены в Госкомиссию на проверку наименования'",
                "1F0B5328-01F2-4FEB-B0C1-371CE93480CB")
               .WhenCurrentStageCode(RouteStageCodes.SA_03_2)
               .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SA_SOPR_1))
               .And<IsRequestHasAnySelectionAchieveTypeCodesRule>(r => r.Eval(new[] { DicSelectionAchieveTypeCodes.AnimalHusbandry }))
               .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_1));

            WorkflowStage("Из этапа 'Предварительная экспертиза' на этап 'Запрос предварительной экспертизы'", "096E4097-1029-4D87-873A-E7F2057A753B")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_2)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SelectiveAchievementRequest))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_2_1));

            WorkflowStage("Из этапа 'Направлены в Госкомиссию на проверку наименования' на этап 'Возвращено с Госкомиссии'", "B398261F-932B-4767-A2A0-71059513FA55")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_1)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.NamingConclusion }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_2));

            WorkflowStage("Из этапа 'Возвращено с Госкомиссии' на этап 'Вынесено экспертное заключение предварительной экспертизы'", "077B222F-ECA4-44F0-92F3-EC73A853F9F1")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_2)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.SelectiveAchievementExpertConclusionNegative }, RouteStageCodes.DocumentOutgoing_02_1))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_3));

            WorkflowStage("Из этапа 'Возвращено с Госкомиссии' на этап 'Вынесено экспертное заключение предварительной экспертизы'", "EA102DBE-A72B-4585-89F7-E6EBE76CDCF7")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_2)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.SelectiveAchievementExpertConclusionPositive }, RouteStageCodes.DocumentOutgoing_02_1))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_3));

            //Todo Возможно, триггер у двух этапов ниже - по генерации исх. номера у соответствующих документов
            WorkflowStage("Из этапа 'Вынесено экспертное заключение предварительной экспертизы' на этап 'Утверждено директором'", "68D56048-FD50-4FC8-99C9-23DEFF93ADEF")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_3)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.SelectiveAchievementExpertConclusionNegative }, RouteStageCodes.DocumentOutgoing_03_1))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_4));

            WorkflowStage("Из этапа 'Вынесено экспертное заключение предварительной экспертизы' на этап 'Утверждено директором'", "E49F1E39-C655-4617-8817-7F3E2033B9EE")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_3)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.SelectiveAchievementExpertConclusionPositive }, RouteStageCodes.DocumentOutgoing_03_1))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_4));

            WorkflowStage("Из этапа 'Утверждено директором' на этап 'Передано в МЮ РК'", "33C7A1CC-DDFB-43B0-9988-98A52306E730")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SA_006_014_0))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_5));

            WorkflowStage("Из этапа 'Передано в МЮ РК' на этап 'Возвращено с МЮ РК'", "E9A0E616-1649-490D-87CC-633443C86D84")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_5)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.DecisionOfAuthorizedBody }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_6));

            WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Экспертиза на патентоспособность'", "304DFB0F-4E8E-4A0C-8D33-91D9D0983E4A")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_6)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SelectiveAchievementExpertConclusionNotification))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3));

            //Todo Триггера в постановке нет
            /*WorkflowStage("Из этапа 'Решение Апелляционного совета' на этап 'Экспертиза на патентоспособность'", "")
                .WhenCurrentStageCode(RouteStageCodes.SAApellationConclusion)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SelectiveAchievementExpertConclusionNotification))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3));*/

            //WorkflowStage("Из этапа 'Отказано в дальнейшем рассмотрении' на этап 'Апелляционный совет'", "72EE2822-326F-4C37-8B26-60D500818F6F")
            //    .WhenCurrentStageCode(RouteStageCodes.SA_03_3_52)
            //    .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Objection }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.SAApellation));

            //WorkflowStage("Из этапа 'Апелляционный совет' на этап 'Решение Апелляционного совета'", "80C49596-E35B-4C84-8390-E078DC647982")
            //    .WhenCurrentStageCode(RouteStageCodes.SAApellation)
            //    .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.DecisionOfAppealsBoard }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.SAApellationConclusion));

            WorkflowStage("Из этапа 'Экспертиза на патентоспособность' на этап 'Направлено в Госкомиссию (патентоспособность)'", "020FFDAF-9494-41B1-A8C8-3E32C8501A76")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SA_SOPR_2))
                .And<IsRequestHasAnySelectionAchieveTypeCodesRule>(r => r.Eval(new[] { DicSelectionAchieveTypeCodes.Agricultural }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_7));

            WorkflowStage("Из этапа 'Экспертиза на патентоспособность' на этап 'Направлено в Госкомиссию (патентоспособность)'",
                "3013FD73-84CE-4300-AC0F-C9787935B53A")
               .WhenCurrentStageCode(RouteStageCodes.SA_03_3)
               .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SA_2_SOPR))
               .And<IsRequestHasAnySelectionAchieveTypeCodesRule>(r => r.Eval(new[] { DicSelectionAchieveTypeCodes.AnimalHusbandry }))
               .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_7));

            WorkflowStage("Из этапа 'Экспертиза на патентоспособность' на этап 'Запрос экспертизы'", "97059415-A3B7-4083-BCD7-53FA856D30FA")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SelectiveAchievementRequest))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_2_1));

            WorkflowStage("Из этапа 'Направлено в Госкомиссию (патентоспособность)' на этап 'Возвращено с Госкомиссии после патентоспособности'", "D03FF871-B0BF-4F5B-82DA-927EB5A317E3")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_7)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PatentableConclusion }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_7_0));

            WorkflowStage("Из этапа 'Возвращено с Госкомиссии после патентоспособности' на этап 'Передано в МЮ РК (заключение Госкомиссии)'", "126D4600-C335-4314-854D-40D581EECE3F")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_7_0)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SA_006_014_1))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_8));

            WorkflowStage("Из этапа 'Передано в МЮ РК (заключение Госкомиссии)' на этап 'Возвращено с МЮ РК'", "527BBEC4-8957-463F-A577-017E3F66323D")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_8)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.DecisionOfAuthorizedBody }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_3_6));

            WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Подготовка для передачи в Госреестр'", "CB52F849-6C80-45C6-B871-5C277575C37D")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_6)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SelectiveAchievementPatentNotification))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_4));

            //Todo Триггера в постановке нет
            /*WorkflowStage("Из этапа 'Решение Апелляционного совета' на этап 'Подготовка для передачи в Госреестр'", "")
                .WhenCurrentStageCode(RouteStageCodes.SAApellationConclusion)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SelectiveAchievementExpertConclusionNotification))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_4));*/

            WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Отказано в выдаче патента'", "9ABE93B2-248D-47D4-8F13-963617E1115B")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_6)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.SelectiveAchievementPatentRefuseNotification))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_4_1));

            //WorkflowStage("Из этапа 'Отказано в выдаче патента' на этап 'Апелляционный совет'", "56405C15-3C7A-4ED7-B7B3-A8DF44A9CD3F")
            //    .WhenCurrentStageCode(RouteStageCodes.SA_03_4_1)
            //    .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Objection }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.SAApellation));

            WorkflowStage("Из этапа 'Подготовка для передачи в Госреестр' на этап 'Готовые для передачи в Госреестр'", "DFBAD6B0-3CE5-4541-8CB7-5E73CF67D5B5")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_4)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_5));

            WorkflowStage("Из этапа 'Отсутствует оплата' на этап 'Готовые для передачи в Госреестр'", "D6B93C91-A24F-48C2-9FCE-9FEB5E69C278")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_10)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent }))
                .Then(SendRequestToNextStage(RouteStageCodes.SA_03_5));

            WorkflowStage("Из этапа 'Направлен запрос' на этап основного сценария",
                    "09D37DE6-5589-4DB6-9C20-7CBA031B3E9A")
                .WhenCurrentStageCode(RouteStageCodes.SA_03_3_1)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(ReturnFromSendRequestScenario());
        }
    }
}
