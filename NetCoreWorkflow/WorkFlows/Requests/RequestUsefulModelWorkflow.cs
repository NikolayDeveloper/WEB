using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.RulesRequest;

namespace NetCoreWorkflow.WorkFlows.Requests
{
    /// <summary>
    /// Рабочий процесс по заявке "Полезные модели"
    /// </summary>
    public class RequestUsefulModelWorkflow : BaseRequestWorkflow
    {
        public RequestUsefulModelWorkflow(
            IIntegrationStatusUpdater integrationStatusUpdater,
            IIntegrationDocumentUpdater integrationDocumentUpdater,
            IMapper mapper) : base(integrationStatusUpdater, integrationDocumentUpdater, mapper)
        {
            //InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Из любого этапа на этап 'Отозвано'", "48320ABD-8DEA-4F32-8342-2F46498A8CC2")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionOfApplicationRevocation }))
                .Then(SendRequestToNextStage(RouteStageCodes.RequestCanceled));

            WorkflowStage("Из любого этапа на этап 'Делопроизводство приостановлено'", "B1ABF469-11D8-45FD-BE44-DC613172F887")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForSuspensionOfOfficeWork }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_02_2_0));

            WorkflowStage("Ручной переход этапов", "466BD9C6-02E1-471C-A484-977A548959FC")
                .UseForAllStages()
                .And<IsRequestExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "47E25F89-4263-454C-BF26-CC6F79583444")
                .UseForAllStages()
                .And<IsRequestExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Переход по текущему этапу вручную", "B5CF79A6-A6A6-405A-939F-B3939A5542FF")
                .UseForAllStages()
                .And<IsRequestBeingSentToTheSameStageRule>(r => r.Eval())
                .Then(SendRequestToSameStage());

            WorkflowStage("Из этапа 'Секретка' на этап 'Ввод оплат'", "915AFF54-E242-4A24-B886-6CF481044899")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_1_0)
                .And<IsDocumentSignedAtStageRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.СertificateOfConfidentiality }, RouteStageCodes.DocumentInternal_1_1))
                .Then(SendToNextStageAndCreatePaymentInvoice(RouteStageCodes.UM_02_2, DicTariff.Codes.InventionExaminationOnUsefulModelOnPaper, DicPaymentStatusCodes.Notpaid));

            WorkflowStage("Из этапа 'Ввод оплат' на этап 'Ожидает оплату за подачу ПМ'", "D1A8AAC5-9627-4426-A8A2-D60AF9838795")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.POL2_0))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_02_2_7));

            WorkflowStage("Из этапа 'Ожидает оплату за подачу ПМ' на этап 'Ввод оплат'", "D6BB70B8-A9C1-4847-914F-276CD9C72897")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2_7)
                .And<IsRequestDocumentHasCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1}))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_02_2));

            WorkflowStage("Из этапа 'Ожидает оплату за подачу ПМ' на этап 'Признаны неподанными'", "B1B03EAD-A458-41D7-A2E5-5DAEFAFA9776")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2_7)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval( DicDocumentTypeCodes.NotPaymentRequest))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_3));

            WorkflowStage("Из этапа 'Ввод оплат' на этап 'Готовые для передачи на экспертизу'", "C6339B25-F442-4ABB-A8F1-62B9A8BABF31")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[]
                {
                    DicTariff.Codes.InventionExaminationOnUsefulModelOnOnline,
                    DicTariff.Codes.InventionExaminationOnUsefulModelOnPaper
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_02_2_1));

            WorkflowStage("Из этапа 'Ввод оплат' на этап 'Экспертиза заявки на выдачу патента ПМ'", "4CFE4A93-8369-4F27-BAA7-0619FFAED12A")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2)
                .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[]
                {
                    DicTariffCodes.UsefullModelPatentExaminationOnPurpose,
                    DicTariffCodes.UsefullModelPatentExaminationEmail,
                    DicTariff.Codes.AcceptanceApplicationsConventionalPriorityAafterDeadline
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_1));

            WorkflowStage("Из этапа 'Ожидает перевод материалов заявки ПМ' на этап 'Распределение на проведение экспертизы на ПМ'", "45D258B4-B584-4404-9F28-774993428545")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2_4)
                .And<IsRequestDocumentHasCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.OthersIncoming, DicDocumentTypeCodes._001_005}))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_1));

            WorkflowStage("Из этапа 'Формирование данных заявки' на этап 'Преобразование заявки на изобретение'", "E35B9BF7-A835-4A67-B1E1-3F1E3CACBCBF")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Author))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Correspondence))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Declarant))
                .And<IsRequestNotOfConventionTypeRule>(r => r.Eval(DicConventionTypeCodes.PctConvention))
                .And<IsRequestHasAnyNameRule>(r => r.Eval())
                .And<IsRequestHasReferatRule>(r => r.Eval())
                .And<IsRequestHasDateRule>(r => r.Eval())
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Формирование данных заявки' на этап 'Преобразование заявки на изобретение'", "B3F8772D-F2FC-4723-A84B-1853E96539E8")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Author))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Correspondence))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Declarant))
                .And<IsRequestOfConventionTypeRule>(r => r.Eval(DicConventionTypeCodes.PctConvention))
                .And<IsRequestHasAnyNameRule>(r => r.Eval())
                .And<IsRequestHasReferatRule>(r => r.Eval())
                .And<IsRequestHasDateRule>(r => r.Eval())
                .And<IsRequesthasConventionsInfoRule>(r => r.Eval())
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Секретка' на этап 'Преобразование заявки на изобретение'", "3890F5F8-39D8-466E-8474-7F75FDB7021E")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_1_0)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Ввод оплат' на этап 'Преобразование заявки на изобретение'", "C727A8A4-2CEA-49E8-AA64-D29D64BB867C")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Ожидает заверенную копию конв.зачвки ПМ' на этап 'Преобразование заявки на изобретение'", "5743E156-0EA7-4E4B-BF75-A5D40D0269CD")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2_2_2)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Готовые для передачи на экспертизу' на этап 'Преобразование заявки на изобретение'", "24842DBC-02CC-4B44-9383-A3753981697D")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Ожидает оплату за подачу ПМ' на этап 'Преобразование заявки на изобретение'", "5AFC6556-7373-45D5-B4B5-903424EC89B6")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2_7)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Ожидает перевод материалов заявки ПМ' на этап 'Преобразование заявки на изобретение'", "786B0335-B9FC-41BE-ACDE-A21ED2B3E33B")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2_4)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Распределение на проведение экспертизы на ПМ' на этап 'Преобразование заявки на изобретение'", "55F719D5-64E7-493B-B9C0-89C585732974")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Экспертиза заявки на выдачу патента на ПМ' на этап 'Преобразование заявки на изобретение'", "6DE888F8-D0A3-47F8-903B-DD20BB4E6E58")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_2)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Вынесено экспертное заключение' на этап 'Преобразование заявки на изобретение'", "7DA6B8A2-C5DC-46D9-B54A-94534DC0AE96")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_3)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Утверждено директором' на этап 'Преобразование заявки на изобретение'", "62B6A355-F84B-4B83-BBA1-1FACDF58B0CB")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_4)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Запрос экспертизы' на этап 'Преобразование заявки на изобретение'", "AA22257B-E667-48D9-A31A-17F4616D1812")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_2_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Продление срока ' на этап 'Преобразование заявки на изобретение'", "9D7DB060-2F72-4292-AAB9-5F0BB9F6F4CF")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2_6)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes._001_004G_1))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_4));

            WorkflowStage("Из этапа 'Экспертиза заявки на выдачу патента на ПМ' на этап 'Вынесено экспертное заключение'", "A35AD8C1-1105-46FE-8F1A-C5E7BC3564F8")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_2)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                        {
                            DicDocumentTypeCodes.RefusalToGratConclusion,
                            DicDocumentTypeCodes.GrantingUsefulModel
                        },
                        RouteStageCodes.DocumentInternal_1_1_4))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_3));

            WorkflowStage("Из этапа 'Вынесено экспертное заключение' на этап 'Утверждено руководством'", "75CC10C9-20F6-4BD1-99D3-46D6D1408595")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_3)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                        {
                            DicDocumentTypeCodes.RefusalToGratConclusion,
                            DicDocumentTypeCodes.GrantingUsefulModel
                        },
                        RouteStageCodes.DocumentInternal_IN01_1_0))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_4));

            WorkflowStage("Из этапа 'Утверждено руководством' на этап 'Передано на подготовку уведомления'", "65A7AE01-CA7A-49A2-8D47-FD7D5C0A0EAE")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_4)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                        {
                            DicDocumentTypeCodes.RefusalToGratConclusion,
                            DicDocumentTypeCodes.GrantingUsefulModel
                        },
                        RouteStageCodes.DocumentInternalIN01_1_3))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_6_0));

            WorkflowStage("Из этапа 'Утверждено директором' на этап 'Передано в МЮ РК'", "C0F388DA-B912-4C5A-A86B-5A816BCA8600")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.RegistersOfExpertOpinionsInMjOfRk))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_5));

            WorkflowStage("Из этапа 'Утверждено директором' на этап 'Отказанов в выдаче патента'", "04CADD48-668F-4219-93BE-99F2E8A8D450")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_4)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.RefusalToGratConclusion))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_4_0));

            WorkflowStage("Из этапа 'Передано в МЮ РК' на этап 'Возвращено с МЮ РК'", "23A94C63-A8A0-4491-AD1E-F076C5D9F66A")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_5)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] {DicDocumentTypeCodes.DecisionOfAuthorizedBody}))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_6));

            WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Направлено уведомление о принятии решения'", "7CD63873-861D-4C27-9C2E-EC562962A542")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_6)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicDocumentTypeCodes.UV_KPM
                    }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_7_0));

            WorkflowStage("Из этапа 'Передано на подготовку уведомления' на этап 'Направлено уведомление о принятии решения'", "71F7DE7A-8A34-4766-8B8B-4ABB5D9DD5E8")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_6_0)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicDocumentTypeCodes.UV_KPM
                    }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_7_0));

            WorkflowStage("Из этапа 'Направлено уведомление о принятии решения' на этап 'Отсутствует оплата, срок с правом восстановления'", "A00E1838-C337-4412-A24F-B7B6D7438BE7")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_7_0)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.S5))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_7_3));

           WorkflowStage("Из этапа 'Экспертиза заявки на выдачу патента на ПМ' на этап 'Запрос экспертизы'", "FC8B2C77-6FF9-4BA2-8E08-000DBE464837")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_2)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.ExpertizeRequest))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_1));

            WorkflowStage("Из этапа 'Продление срока ' на этап 'Экспертиза заявки на выдачу патента на ПМ'", "EEDBBBBB-32DE-4F51-B30B-36D8B2FD92EF")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2_6)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2));

            WorkflowStage("Из этапа 'Запрос экспертизы' на этап 'Продление срока '", "102075AA-B3A0-430D-8E32-0BC7B8389C4F")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_2_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[] {DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_02_2_6));

            WorkflowStage("Из этапа 'Запрос экспертизы' на этап 'Отозванные'", "A24F7520-91A7-4506-AAB0-7ED707E24772")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_2_1)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.UV_2PMZ))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_2));

            WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Отказанов в выдаче патента'", "698A9A5A-BEC1-4DDF-99DA-20ADF58E60CF")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_6)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.RefusalToGratConclusion))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_4_0));

            WorkflowStage("Из этапа 'Экспертиза заявки на выдачу патента (повторное по решению Апелляционного совета) ПМ' на этап 'Вынесено экспертное заключение'", "0F291F45-D3BC-4AB5-9700-483DA1D63648")
                .WhenCurrentStageCode(RouteStageCodes.UM_02_2_5)
                .And<IsDocumentSignedAtStageRule>(r =>
                    r.Eval(
                        new[]
                        {
                            DicDocumentTypeCodes.RefusalToGratConclusion, DicDocumentTypeCodes.GrantingUsefulModel
                        }, RouteStageCodes.DocumentOutgoing_02_1))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_3));

            WorkflowStage("Из этапа 'Отозванные' на этап 'Восстановление срока'", "5EFE605E-6B44-4209-B81C-39F734761B33")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_2_2)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new []{DicDocumentTypeCodes.PetitionForExtendTime}))
                .And<IsRequestHasNotDocumentWithCodeRule>(r => r.Eval(DicDocumentTypeCodes.NotificationOfUsefulModelFinalRecall))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_3_1));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Экспертиза заявки на выдачу патента на ПМ'", "54A99CB9-B3B6-434B-AFB1-78F842CD121C")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_3_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes.AnswerToRequest))
                .And<IsRequestHasMoreThanDocumentsByCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.ExpertizeRequest}))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[] {DicTariff.Codes.NEW_058}))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Отозванные'", "EB8B9420-2FA4-4736-B66F-B8ECC27782DF")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_3_1)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.NotificationOfUsefulModelFinalRecall))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_2_2));

            WorkflowStage("Из этапа 'Отсутствует ходатайство о досрочной публикации ПМ' на этап 'Готовые для передачи в Госреестр'", "073DCF8E-7D18-48B2-B83A-1E10B88B7FA3")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_3_7)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new []{DicDocumentTypeCodes.ApplicationForEarlyPublication }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_8));

            WorkflowStage("Из любого этапа на этап 'Внесение изменений'", "F8C17DED-3534-4531-B951-CE998EF25285")
                .UseForAllStages()
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.PetitionForChanging
                }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] {
                    DicTariff.Codes.NEW_056,
                    DicTariff.Codes.NEW_018,
                    DicTariff.Codes.NEW_017
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_02_2_7_0));

            WorkflowStage("Из любого этапа на этап 'Внесение изменений'", "C075B353-68D2-4DBD-A869-CAA013C5C9A0")
                .UseForAllStages()
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[]
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
                .Then(SendRequestToNextStage(RouteStageCodes.UM_02_2_7_0));

            WorkflowStage("Из этапа 'Запрос экспертизы' на этап основного сценария",
                    "09D37DE6-5589-4DB6-9C20-7CBA031B3E9A")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_2_1)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(ReturnFromSendRequestScenario());

            // Автоэтапы
            WorkflowStage("Из этапа 'Направлено уведомление о принятии решения' на этап 'Готовые для передачи в Госреестр'", "A02DFCAD-F030-47E2-BC9B-07C700D9CBD3")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_7_0)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1/*, DicDocumentTypeCodes._001_032*/))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicTariffCodes.BS2URegistrationAndPublishing
                }))
                //.And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
                //{
                //    DicTariff.Codes.StateFee2018
                //}))
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.StateServicesRequest }))
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.UM_03_8))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_8));

            WorkflowStage("Из этапа 'Отсутствует оплата, срок с правом восстановления' на этап 'Готовые для передачи в Госреестр'", "F615B51D-8D36-4817-B9FF-B392EC4F08EB")
                .WhenCurrentStageCode(RouteStageCodes.UM_03_7_3)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForRestoreTime))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicTariff.Codes.NEW_058
                }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent
                }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicTariff.Codes.StateFee2018
                }))
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.UM_03_8))
                .Then(SendRequestToNextStage(RouteStageCodes.UM_03_8));
        }
    }
}