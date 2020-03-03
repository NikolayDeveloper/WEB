using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.RulesRequest;

namespace NetCoreWorkflow.WorkFlows.Requests
{
    /// <summary>
    /// Рабочий процесс по заявке "Промышленные образцы"
    /// </summary>
    public class RequestIndustrialDesignsWorkflow : BaseRequestWorkflow
    {
        public RequestIndustrialDesignsWorkflow(
            IIntegrationStatusUpdater integrationStatusUpdater,
            IIntegrationDocumentUpdater integrationDocumentUpdater,
            IMapper mapper) : base(integrationStatusUpdater, integrationDocumentUpdater, mapper)
        {
            //InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "D45C91B5-94C3-4787-BCD9-C358EFBFC966")
                .UseForAllStages()
                .And<IsRequestExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "C525D689-99D9-4057-88D3-CDABE8C89AEB")
                .UseForAllStages()
                .And<IsRequestExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Переход по текущему этапу вручную", "96B63E3B-F7E6-411B-BE19-13ED38B6E709")
                .UseForAllStages()
                .And<IsRequestBeingSentToTheSameStageRule>(r => r.Eval())
                .Then(SendRequestToSameStage());

            WorkflowStage("Из этапа 'Ожидает оплаты за подачу' на этап 'Признаны неподанными'", "0764AC99-BF46-4AD8-A72D-5C16A3073952")
                .WhenCurrentStageCode(RouteStageCodes.PO_02_2_0)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.PO8_1))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_02_3));

            WorkflowStage("Из этапа 'Ввод оплаты' на этап 'Готовые на передачу на формальную экспертизу'", "60F5E573-AE20-4E13-8A3F-22844D405F1E")
                .WhenCurrentStageCode(RouteStageCodes.PO_02_2)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.IndustrialDesignExaminationOnPurpose, DicTariff.Codes.IndustrialDesignExaminationEmail }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_0));

            WorkflowStage("Из этапа 'Ожидает оплаты за подачу' на этап 'Готовые на передачу на формальную экспертизу'", "A49DB459-931D-4BA6-933B-03BCC2AC9415")
                .WhenCurrentStageCode(RouteStageCodes.PO_02_2_0)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.IndustrialDesignExaminationOnPurpose, DicTariff.Codes.IndustrialDesignExaminationEmail }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_0));
            
            WorkflowStage("Из этапа 'Ввод оплаты' на этап 'Ожидает оплаты за подачу'", "E8D091B0-749B-4526-B234-61E0F2822583")
                .WhenCurrentStageCode(RouteStageCodes.PO_02_2)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.POL2_0))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_02_2_0));

            WorkflowStage("Из этапа 'Продление срока' на этап 'Формальная экспертиза'", "E257CD7C-D02B-49D3-9649-8335637D7391")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2_1_0)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_2));

            WorkflowStage("Из этапа 'Ожидание восстановления пропущенного срока' на этап 'Формальная экспертиза'", "A79709FE-94C4-4B65-B30D-1CCB7D055547")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_4)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForRestoreTime }))
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_2));

            WorkflowStage("Из этапа 'Формальная экспертиза' на этап 'Подготовка на экспертизу по существу'", "71084F05-F2E5-475C-A115-837ABCE4F720")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.PO1_1))
                .And<IsRequestHasNotPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_015, DicTariff.Codes.NEW_016 }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_2_01));

            WorkflowStage("Из этапа 'Формальная экспертиза' на этап 'Запрос экспертизы'", "8F28C5FA-320D-4989-BA83-567E86E8D0B7")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.PO7))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_2_1));

            WorkflowStage("Из этапа 'Запрос экспертизы' на этап 'Продление срока'", "DD94432C-1AA9-4F51-BEC7-99FC25A9181F")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2_1)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForExtendTimeRorResponse }))
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_2_1_0));

            WorkflowStage("Из этапа 'Формальная экспертиза' на этап 'Готовые на экспертизу по существу'", "F15EDA73-FAC6-4C83-9C04-28B76363F1E9")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2)                
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.PO1))
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_015 }))
                .And<IsRequestHasPayedIfExistsInvoicesWithTariffCodeRule>(r => r.Eval( DicTariff.Codes.NEW_016 ))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_2_03));

            WorkflowStage("Из этапа 'Подготовка на экспертизу по существу' на этап 'Готовые на экспертизу по существу'", "EFFAA463-EF79-4AFC-97AB-FA5CF07FFE4D")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2_01)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_015 }))
                .And<IsRequestHasPayedIfExistsInvoicesWithTariffCodeRule>(r => r.Eval(DicTariff.Codes.NEW_016))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_2_03));

            WorkflowStage("Из этапа 'Ожидание оплаты за экспертизу по существу' на этап 'Готовые на экспертизу по существу'", "09D34B2F-3012-4C63-82F8-260E375E105E")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2_02)
                .And<IsRequestDocumentHasCodeRule>(r=>r.Eval(new[] { DicDocumentTypeCodes.PetitionForRestoreTime }))
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest }))
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_015 }))
                .And<IsRequestHasPayedIfExistsInvoicesWithTariffCodeRule>(r => r.Eval(DicTariff.Codes.NEW_016))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_2_03));

            WorkflowStage("Из этапа 'Подготовка на экспертизу по существу' на этап 'Ожидание оплаты за экспертизу по существу'", "9308FEDE-C02D-417B-96DB-39C6E8144968")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2_01)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval( DicDocumentTypeCodes.PO8 ))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_2_02));

            WorkflowStage("Из этапа 'Продление срока' на этап 'Экспертиза по существу'", "96B233A7-65D7-4C4C-A020-A11D117672AD")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2_1_0)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_3));

            WorkflowStage("Из этапа 'Ожидание восстановления' на этап 'Экспертиза по существу'", "01C77DBD-E8AC-4A6F-87B4-074E76E713CE")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_4)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForRestoreTime }))
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_3));

            WorkflowStage("Из этапа 'Экспертиза по существу' на этап 'Вынесено экспертное заключение'", "F6060D4A-04C7-4E2E-87AE-43BAB17772CB")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_3)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(
                    new[] { DicDocumentTypeCodes.PO5,
                             DicDocumentTypeCodes.PO4_1,
                             DicDocumentTypeCodes.PO7_1,
                             DicDocumentTypeCodes.PO7_2,
                             DicDocumentTypeCodes.PO5_KZ,
                             DicDocumentTypeCodes.PO4,
                             DicDocumentTypeCodes.PO5_1111,
                             DicDocumentTypeCodes.PO5_10
                        }, RouteStageCodes.DocumentOutgoing_02_1))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_4));

            WorkflowStage("Из этапа 'Экспертиза по существу' на этап 'Запрос экспертизы'", "83520ED7-447D-4072-8936-5F431777F2C4")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_3)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.Z_PO7))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_2_1));

            WorkflowStage("Из этапа 'Запрос экспертизы' на этап 'Ожидание восстановления'", "AF4FEE7E-2DC9-488E-B3FC-6C83DF6E2747")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2_1)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.UV_PO8))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_4));

            WorkflowStage("Из этапа 'Вынесено экспертное заключение' на этап 'Утверждено директором'", "8D065CD5-CC21-4EAD-A37D-5534F0B3CF1B")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_4)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(
                    new[] { DicDocumentTypeCodes.PO5,
                             DicDocumentTypeCodes.PO4_1,
                             DicDocumentTypeCodes.PO7_1,
                             DicDocumentTypeCodes.PO7_2,
                             DicDocumentTypeCodes.PO5_KZ,
                             DicDocumentTypeCodes.PO4,
                             DicDocumentTypeCodes.PO5_1111,
                             DicDocumentTypeCodes.PO5_10
                        }, RouteStageCodes.DocumentOutgoing_03_1))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_5));

            WorkflowStage("Из этапа 'Утверждено директором' на этап 'Передано в МЮ РК'", "DCFD4072-7C63-4C81-9B66-01372D354803")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_5)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.RegistersOfExpertOpinionsInMjOfRk))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_6));

            WorkflowStage("Из этапа 'Передано в МЮ РК' на этап 'Возвращено с МЮ РК'", "13D3C234-5D5A-4E4C-A1E9-9CE24F2B2CAC")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_6)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.DecisionOfAuthorizedBody }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_7));

            WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Подготовка для передачи в Госреестр'", "B17FA50D-098E-4663-A467-A7EDCDF65A5B")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_7)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.UV_P_PO))
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UV_P_PO }, RouteStageCodes.DocumentOutgoing_03_1))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_8));

            WorkflowStage("Из этапа 'Рассмотрение возражения на Апелляционном совете' на этап 'Подготовка для передачи в Госреестр'", "DD4C3ABB-B082-4B26-979A-CAD654D12472")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_7_0_3)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.UV_P_PO))
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UV_P_PO }, RouteStageCodes.DocumentOutgoing_03_1))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_8));

            WorkflowStage("Из этапа 'Направление заявителю экспертного заключения об отказе с выявленным аналогом' на этап 'Рассмотрение возражения на Апелляционном совете'", "0F4A4B86-373C-4EB4-8CD8-2EBB9DC7FB76")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_7_0_1)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Objection }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_7_0_3));

            WorkflowStage("Из этапа 'Подготовка для передачи в Госреестр' на этап 'Готовые для передачи в Госреестр'", "C51DCB6E-E750-4A95-AF90-6FB2E283F345")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_8)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] {
                    DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_9));

            WorkflowStage("Из этапа 'Отсутствует оплата' на этап 'Готовые для передачи в Госреестр'", "07211718-FC29-402E-9A21-10BC52146D1B")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_8_0)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] {
                    DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent
                }))
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.PO_03_9));

            WorkflowStage("Из этапа 'Запрос экспертизы' на этап основного сценария",
                    "09D37DE6-5589-4DB6-9C20-7CBA031B3E9A")
                .WhenCurrentStageCode(RouteStageCodes.PO_03_2_1)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(ReturnFromSendRequestScenario());
        }
    }
}
