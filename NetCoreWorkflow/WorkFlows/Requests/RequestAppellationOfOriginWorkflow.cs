using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.RulesRequest;

namespace NetCoreWorkflow.WorkFlows.Requests
{
    /// <summary>
    /// Рабочий процесс по заявке "Наименование места происхождения товара"
    /// </summary>
    public class RequestAppellationOfOriginWorkflow : BaseRequestWorkflow
    {
        public RequestAppellationOfOriginWorkflow(
            IIntegrationStatusUpdater integrationStatusUpdater,
            IIntegrationDocumentUpdater integrationDocumentUpdater,
            IMapper mapper) : base(integrationStatusUpdater, integrationDocumentUpdater, mapper)
        {
            //InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Из любого этапа на этап 'Отозвано'", "A6B70E55-22A1-4F11-9F94-16C92BD33999")
                .UseForAllStages()
                .And<IsRequestHasNotRouteStagesByCodesRule>(r => r.Eval(new[] { RouteStageCodes.RequestCanceled }))
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionOfApplicationRevocation }))
                .Then(SendRequestToNextStage(RouteStageCodes.RequestCanceled));

            WorkflowStage("Из любого этапа на этап 'Делопроизводство приостановлено'", "496C9818-07A6-4239-84B6-713F438B5E2C")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForSuspensionOfOfficeWork }))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_2_1));

            WorkflowStage("Ручной переход этапов", "A773EE8D-D937-4CDB-B4F3-1B0280D3B4A2")
                .UseForAllStages()
                .And<IsRequestExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "F5D99696-8FB0-488C-BBC8-9ED3C57E1D15")
                .UseForAllStages()
                .And<IsRequestExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Переход по текущему этапу вручную", "ABB7EAE7-0AEF-4F9A-BD91-34D20109389C")
                .UseForAllStages()
                .And<IsRequestBeingSentToTheSameStageRule>(r => r.Eval())
                .Then(SendRequestToSameStage());

            WorkflowStage("Из этапа 'Ввод оплат' на этап 'Выбор исполнителя полной экспертизы'", "99782917-4728-4DF3-B74D-9C3A10DF7012")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_02_2)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_078 }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_1));

            WorkflowStage("Из этапа 'Формирование данных заявки' на этап 'Делопроизводство прекращено'", "809449C6-7C2D-4325-BEB9-956A707228FF")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_02_1)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionOfApplicationRevocation }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_02_2_1));

            WorkflowStage("Из этапа 'Экспертиза' на этап 'Вынесено экспертное заключение'", "935A6DDE-59CE-47DD-8298-DBC19951F1CD")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.RegisterNmptExpertConclusion,
                        DicDocumentTypeCodes.ExpertNmptRegisterRefusalOpinion
                    }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_3));

            WorkflowStage("Из этапа 'Вынесено экспертное заключение' на этап 'На утверждение директору'", "A163990B-68F5-4FA6-8497-D6A98BDDB091")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_3)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.RegisterNmptExpertConclusion,
                        DicDocumentTypeCodes.ExpertNmptRegisterRefusalOpinion
                    },
                    RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_3_0));

            WorkflowStage("Из этапа 'На утверждение директору' на этап 'Утверждено директором'", "3874CA51-C9F5-4B12-B7E8-228EC6E8964F")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_3_0)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.RegisterNmptExpertConclusion,
                        DicDocumentTypeCodes.ExpertNmptRegisterRefusalOpinion,
                        DicDocumentTypeCodes.ExpertRefusalNmptFinalConclusion
                    },
                    RouteStageCodes.DocumentOutgoing_02_3))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_4));

            WorkflowStage("Из этапа 'Ввод оплат' на этап 'Ожидает оплату за приём и проведение экспертизы'", "A5810503-2CC9-4902-8DF2-17FFB6D82BDE")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_02_2)
                .And<IsRequestHasNotPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_078 }))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.POL2_0))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_02_2_0));

            WorkflowStage("Из этапа 'Ожидает оплату за приём и проведение экспертизы' на этап 'Ввод оплат'", "6F634F28-295D-4042-B334-418AD1FB9883")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_02_2_0)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes._001_002,
                    DicDocumentTypeCodes.IN001_032,
                    DicDocumentTypeCodes._001_002_1
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_02_2));

            WorkflowStage("Из этапа 'Ожидает оплату за приём и проведение экспертизы' на этап 'Делопроизводство прекращено'", "91412770-AE5D-4A8B-9BEB-4C3BD2731715")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_02_2_0)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(new[] 
                {
                    DicDocumentTypeCodes.NotificationOfPaymentlessOfficeWorkTermination,
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_02_2_1));

            WorkflowStage("Из этапа 'Экспертиза' на этап 'Направлен запрос'", "C5D8287C-077D-4FFD-9011-8F283F46FD11")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.RequestForFullExamination))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2_1));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Экспертиза'", "6C6C40EA-FC4D-4097-A475-73B0FA95AD24")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_1)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'", "CB31BB98-046F-4CBD-AFE9-8A3AACCBA62E")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_1)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForExtendTimeRorResponse }))
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[] { DicTariff.Codes.RequestAnswerTimeExtensionForMonth }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2_2));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'", "E6F8D283-FF66-47C8-BF15-47C1A4A55945")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032))
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse, DicDocumentTypeCodes._001_002_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[] { DicTariff.Codes.RequestAnswerTimeExtensionForMonth }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2_2));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'", "D96BFF8C-198B-4A63-8B9E-7795D245AF61")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_002_1))
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse, DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[] { DicTariff.Codes.RequestAnswerTimeExtensionForMonth }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2_2));

            WorkflowStage("Из этапа 'Продление срока' на этап 'Экспертиза'", "B08999F0-2A62-4863-9AE5-8F83364C5B7A")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_2)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2));

            WorkflowStage("Из этапа 'Утверждено директором' на этап 'Передано в МЮ РК'", "5CCC1036-A36B-443C-914F-EA1E13E1AFF0")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.RegistersOfExpertOpinionsInMjOfRk))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_5));

            WorkflowStage("Из этапа 'Передано в МЮ РК' на этап 'Возвращено с МЮ РК'", "F9ECA60B-126F-451E-B949-C2E4CC1938FA")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_5)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.DecisionOfAuthorizedBody }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_6));

            WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Направлено заявителю заключение об отказе'", "76EBB161-6C68-4BB7-A1AA-98695DA93AAB")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_6)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.ExpertNmptRegisterRefusalOpinion))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_3_1));

            WorkflowStage("Из этапа 'Направлено заявителю заключение об отказе' на этап 'Возражение на предварительный или частичный отказ'", "52532294-C0EA-40BC-BFD4-E6782A92CB58")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_3_1)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Objection }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_3_2));

            WorkflowStage("Из этапа 'Направлено заявителю заключение об отказе' на этап 'Продление срока'", "E5B69672-B5E5-4D7E-BF4D-4EC69A0984B6")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_3_1)
                .And<IsRequestDocumentHasCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.PetitionForExtendTimeRorObjections }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2_2));

            WorkflowStage("Из этапа 'Продление срока' на этап 'Возражение на предварительный или частичный отказ'", "E4FE9379-B30A-4863-940B-A42D4FB6B977")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_2)
                .And<IsRequestDocumentHasCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.Objection }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_3_2));

            WorkflowStage("Из этапа 'Возражение на предварительный или частичный отказ' на этап 'Вынесено окончательное экспертное заключение'", "71219AC6-321D-426A-9924-2A4CF4E37C4D")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_3_2)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.ExpertRefusalNmptFinalConclusion
                    },
                    RouteStageCodes.DocumentOutgoing_02_1))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_3_3));

            WorkflowStage("Из этапа 'Вынесено окончательное экспертное заключение' на этап 'На утверждение директору'", "239D1D83-4659-49AD-83B0-5307ED56D27D")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_3_3)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.ExpertRefusalNmptFinalConclusion
                    },
                    RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_3_0));

            WorkflowStage("Из этапа 'Подготовка для передачи в ГР' на этап 'Готовые для передачи в ГР'", "45A2B5E3-95F0-42FC-8D12-AD6B26644D26")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_7)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032,
                    DicDocumentTypeCodes._001_002_1
                }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[] { DicTariff.Codes.TmNmptRegistration }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_8));

            WorkflowStage("Из этапа 'Подготовка для передачи в ГР' на этап 'Готовые для передачи в ГР'", "A8446112-4B55-4CA2-AC70-1C5DE7D5E1C0")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_7)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[] { DicTariff.Codes.TmNmptRegistration }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_8));

            WorkflowStage("Из этапа 'Делопроизводство прекращено' на этап 'Ожидание восстановления пропущенного срока'", "BFE5B638-ADB6-4EB5-BED9-C469D717C4CD")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_02_2_1)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.PetitionForRestoreTime
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2_3));

            WorkflowStage("Из этапа 'Ожидание восстановления пропущенного срока' на этап 'Ввод оплат'", "E0AA92DA-8CD6-4696-9897-311863EE5C7E")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_3)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes._001_002,
                    DicDocumentTypeCodes.IN001_032,
                    DicDocumentTypeCodes._001_002_1
                }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.RequestAnswerMissedTimeRestoration }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_02_2));

            WorkflowStage("Из любого этапа на этап 'Внесение изменений'", "241A808E-CAAA-4B7A-AC01-08431D089D8E")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.PetitionForChanging
                }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] {
                    DicTariff.Codes.NEW_056,
                    DicTariff.Codes.NEW_018,
                    DicTariff.Codes.NEW_017
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2_4));

            WorkflowStage("Из любого этапа на этап 'Внесение изменений'", "920E1C29-957F-4470-BA6E-365DD8E7AA58")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.PetitionForChanging
                }))
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicTariff.Codes.NEW_056,
                    DicTariff.Codes.NEW_018,
                    DicTariff.Codes.NEW_017
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2_4));

            WorkflowStage("Из любого этапа на этап 'Внесение изменений'", "86D9F4F0-93BA-4F37-A274-143F2C08EB1E")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes._001_002,
                    DicDocumentTypeCodes.IN001_032,
                    DicDocumentTypeCodes._001_002_1
                }))
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForChanging))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicTariff.Codes.NEW_056,
                    DicTariff.Codes.NEW_018,
                    DicTariff.Codes.NEW_017
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2_4));

            WorkflowStage("Из этапа 'Внесение изменений' на этап 'Экспертиза'", "3DCA3976-D56E-4B7A-86D9-9B754FB60EA6")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UV_TZ_VN_IZM }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2));

            WorkflowStage("Из этапа 'Внесение изменений' на этап 'Экспертиза'", "843456A0-5338-4E03-B701-89FAFF803142")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UV_TZ_VN_IZM_ADR, DicDocumentTypeCodes.OUT_UV_Pred_izmen_yur_adr_v1_19 }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2));

            WorkflowStage("Из этапа 'Внесение изменений' на этап 'Экспертиза'", "EB573603-123B-4DB3-80D2-6CAFAF92EAF6")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UV_TZ_VN_IZM_NAIMEN_ZAYAV }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2));

            WorkflowStage("Из этапа 'Внесение изменений' на этап 'Экспертиза'", "EC327B3D-976D-4C04-9471-4AB0C4B8E4BB")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UV_TZ_VN_IZM_PERECH_TOV }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2));

            WorkflowStage("Из этапа 'Внесение изменений' на этап 'Экспертиза'", "AF8F1C20-3764-4ADB-8BF3-C513ABD1D7A6")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UV_TZ_VN_IZM_PRED_ZAYAV }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2));

            WorkflowStage("Из этапа 'Внесение изменений' на этап 'Экспертиза'", "F7F10C0A-7A08-4B3C-BF68-37F79A18FED4")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UV_TZ_VN_IZM_YUR_ADR_PER }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2));

            WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'направлено заявителю заключение об окончательном отказе'", "5D5DFBB2-CB0B-45DC-B707-24DB08D34E6B")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_6)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.ExpertRefusalNmptFinalConclusion))
                .And<IsRequestHasMoreThanDocumentsByCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.DecisionOfAuthorizedBody }, 1))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_3_4_0));

            WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'направлено заявителю заключение об окончательном отказе'", "F8CF6FC8-A3E3-4126-8F8A-85BA6C59917C")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_6)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.DecisionOfAuthorizedBody }))
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes.ExpertRefusalNmptFinalConclusion))
                .And<IsRequestHasMoreThanDocumentsByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.DecisionOfAuthorizedBody }, 1))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_3_4_0));

            WorkflowStage("Из этапа 'Апелляционный совет' на этап 'Решение Апелляционного совета'", "304A95EC-B169-43A0-98C7-E264E4A0A3B8")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_3_5)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.DecisionOfAppealsBoard
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_3_6));

            WorkflowStage("Из этапа 'Решение Апелляционного совета' на этап 'Подготовка для передачи в ГР'", "D66A2BFC-6AC6-43DB-AF24-622C49B70269")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_3_6)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(new[] {
                    DicDocumentTypeCodes.NotificationOfRegistrationDecision
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_7));

            //Автоэтапы
            WorkflowStage("Из любого этапа на этап 'Экспертиза'", "9F349B55-452B-4F71-821B-050DF782493F")
                .UseForAllStages()
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForChanging))
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.NMPT_03_2))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_03_2));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Делопроизводство прекращено'", "D678277A-131F-4567-8B69-17713779921E")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_03_2_1)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.NMPT_02_2_1))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_02_2_1));

            WorkflowStage("Из этапа 'Ожидает оплату за приём и проведение экспертизы' на этап 'Делопроизводство прекращено'", "80BB3674-1EC0-4DAD-A41C-4F1F0F1F0D17")
                .WhenCurrentStageCode(RouteStageCodes.NMPT_02_2_0)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.NMPT_02_2_1))
                .Then(SendRequestToNextStage(RouteStageCodes.NMPT_02_2_1));
        }
    }
}
