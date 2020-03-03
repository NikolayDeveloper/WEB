using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.RulesRequest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreWorkflow.WorkFlows.Requests
{
    /// <summary>
    /// Рабочий процесс по заявке "Изобретения"
    /// </summary>
    public class RequestInventionsWorkflow : BaseRequestWorkflow
    {
        public RequestInventionsWorkflow(
            IIntegrationStatusUpdater integrationStatusUpdater,
            IIntegrationDocumentUpdater integrationDocumentUpdater,
            IMapper mapper) : base(integrationStatusUpdater, integrationDocumentUpdater, mapper)
        {
            //InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Из любого этапа на этап 'Отозвано'", "CA76F16B-8370-495D-8043-E19EFA3552EA")
                .UseForAllStages()
                .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionOfApplicationRevocation }))
                .Then(SendRequestToNextStage(RouteStageCodes.RequestCanceled));

            WorkflowStage("Из любого этапа на этап 'Делопроизводство приостановлено'", "7CA95B07-D91F-405B-9E62-13EAEE565CA6")
                .UseForAllStages()
                .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForSuspensionOfOfficeWork }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_9_0));

            WorkflowStage("Ручной переход этапов", "3218183E-8711-4BA7-ABCB-C1B0F5191C83")
                .UseForAllStages()
                .And<IsRequestExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "72EB9E46-EFE8-4950-B9B6-C0FD7FB04422")
                .UseForAllStages()
                .And<IsRequestExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Переход по текущему этапу вручную", "CE2DD6B9-726E-4FA9-B5AD-5DFEEA922A20")
                .UseForAllStages()
                .And<IsRequestBeingSentToTheSameStageRule>(r => r.Eval())
                .Then(SendRequestToSameStage());


            #region Новый основной сценарий

            WorkflowStage("Из этапа 'Секретка' на этап 'Ввод оплаты'", "46ADB959-328D-4D4B-A32E-B9170F4A1708")
               .WhenCurrentStageCode(RouteStageCodes.I_02_3)
               .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodesRule>
               (c => c.Eval(new[]
               {
                    DicDocumentTypeCodes.СertificateOfConfidentiality
               }, new[] { RouteStageCodes.DocumentInternalIN01_1_3 })) 
               .Then(SendToNextStageAndCreatePaymentInvoices
               (RouteStageCodes.I_02_2, new[] { string.Empty }, DicPaymentStatusCodes.Notpaid));


             WorkflowStage("Из этапа 'Ввод оплат' на этап 'Выбор исполнителя формальной экспертизы'", "0C345FD6-27DD-43FD-99AA-887B91C03769")
                .WhenCurrentStageCode(RouteStageCodes.I_02_2)
                .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[]
                {
                    DicTariff.Codes.InventionFormalExaminationEmail,
                    DicTariff.Codes.InventionFormalExaminationOnPurpose,
                    DicTariff.Codes.InventionAcceleratedFormalExaminationOnPurpose,
                    DicTariff.Codes.InventionAcceleratedFormalExaminationEmail,
                    DicTariff.Codes.AcceptanceApplicationsConventionalPriorityAafterDeadline
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_02_2_1_0_0));

            // Альтернативный сценарий
            WorkflowStage("Из этапа 'Формальная экспертиза изобретения' на этап 'Ожидает оплату за экспертизу по существу'", "4EBD00BC-DD4B-44DE-8DAC-14E9DBDE4E5A")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_1)
                .And<IsAnyDocumentHasOutgoingNumberByCodesRule>(r => r.Eval(new[]
                 {
                    DicDocumentTypeCodes.NotificationForPaymentlessPozitiveFormalExamination,
                    DicDocumentTypeCodes.NotificationPositiveResultFEWithoutPayingKz,
                 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_02_2_0));

            WorkflowStage("Из этапа 'Ожидает оплату за экспертизу по существу' на этап 'Распределение эксперту на проведение экспертизы по существу'", "971C13C3-B20F-4542-BBDE-C1E4787B5598")
                .WhenCurrentStageCode(RouteStageCodes.I_02_2_0)
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[]
                {
                    DicTariff.Codes.ExaminationOfApplicationForInventionMerits,
                    DicTariff.Codes.AcceleratedExaminationOfApplicationForInventionMerits,
                    DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithInternationalReport,
                    DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithSearchReport,
                    DicTariff.Codes.ExaminationEssentiallyAdditionallyIndependentClaimOverOne,
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_3));








            WorkflowStage("Из этапа 'Формальная экспертиза изобретения' на этап 'Распределение эксперту на проведение экспертизы по существу'", "7351BBA7-1B46-4E7A-92CE-9ECFC2291934")
                 .WhenCurrentStageCode(RouteStageCodes.I_03_2_1)
                 .And<IsAnyDocumentHasOutgoingNumberByCodesRule>(r => r.Eval(new[]
                  {
                    DicDocumentTypeCodes.NotificationForPozitiveFormalExamination,
                    DicDocumentTypeCodes.NotificationForPozitiveFormalExaminationKz,
                  }))
                .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[]
                {
                    DicTariff.Codes.ExaminationOfApplicationForInventionMerits,
                    DicTariff.Codes.AcceleratedExaminationOfApplicationForInventionMerits,
                    DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithInternationalReport,
                    DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithSearchReport,
                    DicTariff.Codes.ExaminationEssentiallyAdditionallyIndependentClaimOverOne,
                }))
                 .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_3));


             //WorkflowStage("Из этапа 'Поиск по Алмате УПИ' на этап 'Распределение эксперту на проведение экспертизы по существу (после поиска)'", "D7DDA598-6955-4712-AA3C-CFBE2F36D06C")
             //   .WhenCurrentStageCode(RouteStageCodes.I_03_2_2_0)
             //   .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
             //   {
             //       DicDocumentTypeCodes.IZ_OTCHET_POISK_P
             //    }))
             //   .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_3_0));

            WorkflowStage("Из этапа 'Поиск по Алмате УПИ' на этап 'Экспертиза по существу'", "1FB99C8C-2DED-435E-A2FF-954B28A2E267")
                 .WhenCurrentStageCode(RouteStageCodes.I_03_2_2_0)
                 .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
                 {
                     DicDocumentTypeCodes.IZ_OTCHET_POISK_P
                 }))
                 .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_4));


            WorkflowStage("Из этапа 'Поиск по Астане УПЭФиМ' на этап 'Экспертиза по существу'", "2F7BAA37-0CA3-4F80-ADD3-B1008F0CA72A")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_2_1)
                .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.InventionSearchReport
                 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_4));

            WorkflowStage("Из этапа 'Поиск по Астане УПЭХБиМ' на этап 'Экспертиза по существу'", "A8F873B6-C379-413E-9750-519CC4E6A85C")
               .WhenCurrentStageCode(RouteStageCodes.I_03_2_2_2)
               .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
               {
                    DicDocumentTypeCodes.InventionSearchReport
                }))
               .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_4));


            WorkflowStage("Из этапа 'Экспертиза по существу' на этап 'Вынесено решение и заключение'", "B99AB8AE-812B-4DD9-B0D1-31F1F48E39D3")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_4)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                        {
                            DicDocumentTypeCodes.DecisionConclusionGrantPatent,
                            DicDocumentTypeCodes.ConclusionOfInventionPatentGrantRefuse,
                        },
                        RouteStageCodes.DocumentInternal_1_1_4))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_2));

           WorkflowStage("Из этапа 'Вынесено решение и заключение' на этап 'На утверждении руководства'", "4F8C3CE7-1177-4962-946C-C5A043358E57")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_2)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicDocumentTypeCodes.DecisionConclusionGrantPatent,
                        DicDocumentTypeCodes.ConclusionOfInventionPatentGrantRefuse
                    },
                        RouteStageCodes.DocumentInternal_IN01_1_0))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_2_1));


            WorkflowStage("Из этапа 'На утверждении руководства' на этап 'Передано на подготовку уведомления'", "24A063BB-1657-4DFE-8F10-783DE4BAEAF3")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_2_1)
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.DecisionConclusionGrantPatent,
                        DicDocumentTypeCodes.ConclusionOfInventionPatentGrantRefuse
                    },
                    RouteStageCodes.DocumentInternalIN01_1_3))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_2_2));


           WorkflowStage("Из этапа 'Передано на подготовку уведомления' на этап 'Направлено решение заявителю'", "BAC8D903-330A-4D18-87EA-A815F69BB67F")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_2_2)
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[]
                    {
                        DicDocumentTypeCodes.NotificationOfDecisionPatentGrant,
                    }))
                .Then(SendToNextStageAndCreatePaymentInvoice(RouteStageCodes.I_03_3_2_3, DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent, DicPaymentStatusCodes.Notpaid));

            WorkflowStage("Из этапа 'Направлено решение заявителю' на этап 'Готовые для передачи в Госреестр'", "A97B916B-C594-4AE1-9EC3-10EB0D184B50")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_2_3)
                .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent }))
                .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.ApplicationIssuingSecurityDocument,
                 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_8));


           WorkflowStage("Из этапа 'Готовые для передачи в Госреестр' на этап 'Создание ОД на патент'", "80B98DDD-C7FE-4FC4-AB16-3610C8A96361")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_8)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_04_1))
                .Then(SendRequestToNextStage(RouteStageCodes.I_04_1));

            WorkflowStage("Из этапа 'Готовые для передачи в Госреестр' на этап 'Создание ОД на патент'", "61EE3FCF-D3EE-4704-93A2-0F9AB02758F1")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_8)
                .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.ApplicationForEarlyPublication }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_04_1));

            #endregion







            //WorkflowStage("Из этапа 'Секретка' на этап 'Ввод оплаты'", "46ADB959-328D-4D4B-A32E-B9170F4A1708")
            //    .WhenCurrentStageCode(RouteStageCodes.I_02_3)
            //    .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>
            //    (c => c.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.СertificateOfConfidentiality
            //    }, RouteStageCodes.DocumentInternal_1_1))
            //    .Then(SendToNextStageAndCreatePaymentInvoice(RouteStageCodes.I_02_2, DicTariff.Codes.InventionFormalExaminationOnPurpose, DicPaymentStatusCodes.Notpaid));

            //WorkflowStage("Из этапа 'Ввод оплат' на этап 'Готовые для передачи на формальную экспертизу'", "0C345FD6-27DD-43FD-99AA-887B91C03769")
            //    .WhenCurrentStageCode(RouteStageCodes.I_02_2)
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[]
            //    {
            //        DicTariff.Codes.InventionFormalExaminationEmail,
            //        DicTariff.Codes.InventionFormalExaminationOnPurpose,
            //        DicTariff.Codes.InventionAcceleratedFormalExaminationOnPurpose,
            //        DicTariff.Codes.InventionAcceleratedFormalExaminationEmail
            //    }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_02_2_1));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Ввод оплат' на этап 'Ожидает оплату за подачу'",
            //        "3B9148DE-FA79-43D1-9FD0-6012779271B8")
            //    .WhenCurrentStageCode(RouteStageCodes.I_02_2)
            //    .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
            //        r.Eval(DicDocumentTypeCodes.NotificationForInventionPaymentForIndividuals))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_02_2_0_0));

            //WorkflowStage("Из этапа 'Ввод оплат' на этап 'Ожидает оплату за подачу'",
            //        "8A549E52-10E5-4C8A-BED3-20BDF79A4C26")
            //    .WhenCurrentStageCode(RouteStageCodes.I_02_2)
            //    .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
            //        r.Eval(DicDocumentTypeCodes.NotificationForInventionPaymentForBeneficiaries))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_02_2_0_0));

            //WorkflowStage("Из этапа 'Ввод оплат' на этап 'Ожидает оплату за подачу'",
            //        "929560F4-8219-4E0D-B444-1ABA946B505C")
            //    .WhenCurrentStageCode(RouteStageCodes.I_02_2)
            //    .And<IsDocumentHasOutgoingNumberByCodeRule>(r => r.Eval(DicDocumentTypeCodes.P001_4))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_02_2_0_0));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Ожидает оплату за подачу' на этап 'Готовые для передачи на формальную экспертизу'", "011FF25C-F06E-4A68-AB27-64EFB3D87D1D")
            //      .WhenCurrentStageCode(RouteStageCodes.I_02_2_0_0)
            //      .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForExtendTime }))
            //      .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[]
            //      {
            //        DicTariff.Codes.InventionFormalExaminationEmail,
            //        DicTariff.Codes.InventionFormalExaminationOnPurpose,
            //        DicTariff.Codes.InventionAcceleratedFormalExaminationOnPurpose,
            //        DicTariff.Codes.InventionAcceleratedFormalExaminationEmail
            //      }))
            //      .Then(SendRequestToNextStage(RouteStageCodes.I_02_2_1));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Ожидает оплату за подачу' на этап 'Признаны неподанными или ошибочно зарегистрированные'", "2738D2B2-20FC-4E75-9E49-22DD9C488CF1")
            //      .WhenCurrentStageCode(RouteStageCodes.I_02_2_0_0)
            //      .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_04_0_0_1))
            //      .Then(SendRequestToNextStage(RouteStageCodes.I_04_0_0_1));

            //// TODO ВРЕМЕННО убираем (накопительный этап)
            ////WorkflowStage("Из этапа 'Готовые для передачи на формальную экспертизу' на этап 'Выбор исполнителя формальной экспертизы'", "4C3A72D4-2BD2-4047-9B26-5A4E1636235E")
            ////      .WhenCurrentStageCode(RouteStageCodes.I_02_2_1)
            ////      .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_02_2_1_0_0))
            ////      .Then(SendRequestToNextStage(RouteStageCodes.I_02_2_1_0_0));


            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Запрос экспертизы' на этап 'Формальная экспертиза изобретения'", "F5E75E4E-01EC-4DB1-952E-6ED3425E7331")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1)
            //    .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
            //    .And<IsRequestStageLaterThanOtherStageRule>(r => r.Eval(RouteStageCodes.I_03_2_1, RouteStageCodes.I_03_2_4))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_1));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Продление срока' на этап 'Формальная экспертиза изобретения'", "789B8F5F-771D-4D5D-9359-1173F7BD33F1")
            //   .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_0)
            //   .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
            //   .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest }))
            //   .And<IsRequestStageLaterThanOtherStageRule>(r => r.Eval(RouteStageCodes.I_03_2_1, RouteStageCodes.I_03_2_4))
            //   .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_1));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Восстановление срока' на этап 'Формальная экспертиза изобретения'", "37F355AD-2A75-415D-8905-F6759DBC8AC3")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_1)
            //    .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicTariff.Codes.NEW_032
            //    }))
            //    .And<IsRequestStageLaterThanOtherStageRule>(r => r.Eval(RouteStageCodes.I_03_2_1, RouteStageCodes.I_03_2_1_1))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_1));


            //WorkflowStage("Из этапа 'Формальная экспертиза изобретения' на этап 'Готовые для передачи на экспертизу по существу'", "7351BBA7-1B46-4E7A-92CE-9ECFC2291934")
            //     .WhenCurrentStageCode(RouteStageCodes.I_03_2_1)
            //     .And<IsAnyDocumentHasOutgoingNumberByCodesRule>(r => r.Eval(new[]
            //      {
            //        DicDocumentTypeCodes.NotificationForPozitiveFormalExamination,
            //        DicDocumentTypeCodes.NotificationForPozitiveFormalExaminationKz,
            //      }))
            //     .And<IsRequestHasAnyMainIpcRule>(r => r.Eval())
            //     .And<IsRequestHasCountIndependentItemsRule>(r => r.Eval())
            //     .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_1_1));



            //// Альтернативный сценарий
            WorkflowStage("Из этапа 'Формальная экспертиза изобретения' на этап 'Запрос экспертизы'", "CC587D2F-4D9D-46A6-AE8A-A507836FC779")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_1)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                        {
                            DicDocumentTypeCodes.RequestForFormalExamForInvention
                        }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1_1));

            //// Альтернативный сценарий
            WorkflowStage("Из этапа 'Запрос экспертизы' на этап 'Продление срока'", "123C7BC4-B8F2-4350-832C-244D35F09E71")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForExtendTimeRorResponse }))
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.FE6}))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariffCodes.BProlongationRequestDocumentsTerm }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1_1_0));

            //// Альтернативный сценарий
            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'", "E86D6D9A-E359-42FD-8AB4-B3E0A3B9F2BE")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_1)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForExtendTimeRorResponse }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1_1_0));

            //// Альтернативный сценарий
            WorkflowStage("Из этапа 'Запрос экспертизы' на этап 'Отозванные'", "AEEFBC53-17A7-47E5-8611-11626A3AA317")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_04_0))
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                        {
                            DicDocumentTypeCodes._006_0088,
                            //DicDocumentTypeCodes.FE13,
                            //DicDocumentTypeCodes.NotificationOfRevocationOfPatentApplication,
                        }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_04_0));


            //// Альтернативный сценарий
            WorkflowStage("Из этапа 'Отозванные' на этап 'Восстановление срока'", "87CA6953-8D22-4447-B1AF-B3C1A8CB3F49")
                .WhenCurrentStageCode(RouteStageCodes.I_04_0)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForRestoreTime }))
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r=>r.Eval(new[] { DicDocumentTypeCodes .OfficeWorkRestartNotification}))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r=>r.Eval(new [] {DicTariffCodes.BRestoreAndProlongationTerm }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1_1_1));

            //// Альтернативный сценарий
            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Отозванные'", "FB69FE85-F2C0-4C65-AB39-247578122616")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_1)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_04_0))
                .Then(SendRequestToNextStage(RouteStageCodes.I_04_0));

            //// Альтернативный сценарий
            WorkflowStage("Из этапа 'Отозванные' на этап 'Окончательно отозванные'", "09ABB5B3-845A-448B-A16D-B4E11B7A63AE")
                .WhenCurrentStageCode(RouteStageCodes.I_04_0)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_04_0_0))
                .Then(SendRequestToNextStage(RouteStageCodes.I_04_0_0));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Формальная экспертиза изобретения' на этап 'Признаны неподанными или ошибочно зарегистрированные'", "A9111BE5-7150-40CF-9B52-8DD3A64050F4")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_2_1)
            //    .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesRule>(r =>
            //        r.Eval(new[]
            //            {
            //                DicDocumentTypeCodes.FE133
            //            }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_04_0_0_1));


            //// TODO ВРЕМЕННО убираем (накопительный этап)
            ////WorkflowStage("Из этапа 'Готовые для передачи на экспертизу по существу' на этап 'Подготовка к экспертизе по существу'", "0482A7C4-C505-424D-ADED-7EBA26681C87")
            ////    .WhenCurrentStageCode(RouteStageCodes.I_03_2_1_1)
            ////    .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_03_2_1_0))
            ////    .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_1_0));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Подготовка к экспертизе по существу' на этап 'Ожидание оплаты за экспертизу по существу '", "61D189BF-6CB0-4106-A9AE-5A6D91C0DDC9")
            //   .WhenCurrentStageCode(RouteStageCodes.I_03_2_1_0)
            //   .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.NotificationForPaymentlessPozitiveFormalExamination }))
            //   .Then(SendRequestToNextStage(RouteStageCodes.I_02_2_0));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Восстановление срока' на этап 'Ожидает оплату за экспертизу по существу'", "41D892C0-0F1B-42E2-9C30-576F82E16C48")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_1)
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicTariff.Codes.NEW_032
            //    }))
            //    .And<IsRequestStageLaterThanOtherStageRule>(r => r.Eval(RouteStageCodes.I_03_2_1_1, RouteStageCodes.I_03_2_1))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_02_2_0));


            //WorkflowStage("Из этапа 'Подготовка к экспертизе по существу' на этап 'Подготовка к поиску'", "0BD0A6FA-229E-480F-9E3B-FABC92F0A4EE")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_2_1_0)
            //    .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.IZ_POISK
            //     }))
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[]
            //    {
            //        DicTariff.Codes.ExaminationOfApplicationForInventionMerits,
            //        DicTariff.Codes.AcceleratedExaminationOfApplicationForInventionMerits,
            //        DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithInternationalReport,
            //        DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithSearchReport
            //    }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_1__1));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Ожидает оплату за экспертизу по существу' на этап 'Подготовка к поиску'", "2534DD65-0619-4A8F-B0F6-7ED0629A21AC")
            //    .WhenCurrentStageCode(RouteStageCodes.I_02_2_0)
            //    .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.IZ_POISK
            //     }))
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[]
            //    {
            //        DicTariff.Codes.ExaminationOfApplicationForInventionMerits,
            //        DicTariff.Codes.AcceleratedExaminationOfApplicationForInventionMerits,
            //        DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithInternationalReport,
            //        DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithSearchReport
            //    }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_1__1));



            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Подготовка к поиску' на этап 'Отозванные'", "42CC0E18-86BC-4708-BDA2-00C5C7F2B796")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_2_1__1)
            //    .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_04_0))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_04_0));


            //// TODO ВРЕМЕННО убираем (накопительный этап)
            ////WorkflowStage("Из этапа 'Подготовка к поиску' на этап 'Распределение эксперту на проведение экспертизы по существу'", "511346A8-B65A-41C6-A315-2D379FABD22D")
            ////    .WhenCurrentStageCode(RouteStageCodes.I_03_2_1__1)
            ////    .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_03_2_3))
            ////    .Then(SendToNextSatgeAndSetCoefficientComplexityRequest(RouteStageCodes.I_03_2_3));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Проведение поиска (Алматы)' на этап 'Распределение эксперту на проведение экспертизы по существу (после поиска)'", "D7DDA598-6955-4712-AA3C-CFBE2F36D06C")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_2_2_0)
            //    .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.IZ_OTCHET_POISK_P
            //     }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_3_0));


            //WorkflowStage("Из этапа 'Проведение поиска (Астана)' на этап 'Экспертиза по существу'", "2F7BAA37-0CA3-4F80-ADD3-B1008F0CA72A")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_2_2_1)
            //    .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.ReportOfSearch
            //     }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_4));

            //// Альтернативный сценарий

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Восстановление срока' на этап 'Экспертиза по существу'", "73A14143-B8CE-4AF6-B824-842F474EAABB")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_1)
            //    .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicTariff.Codes.NEW_032
            //    }))
            //    .And<IsRequestStageLaterThanOtherStageRule>(r => r.Eval(RouteStageCodes.I_03_2_4, RouteStageCodes.I_03_2_1_1))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_4));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Продление срока' на этап 'Экспертиза по существу'", "CF62E76E-0E04-4846-ABF8-694260064AEE")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_0)
            //    .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest }))
            //    .And<IsRequestStageLaterThanOtherStageRule>(r => r.Eval(RouteStageCodes.I_03_2_4, RouteStageCodes.I_03_2_1))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_2_4));



            //WorkflowStage("Из этапа 'Экспертиза по существу' на этап 'Вынесено экспертное заключение'", "B99AB8AE-812B-4DD9-B0D1-31F1F48E39D3")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_2_4)
            //    .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule>(r =>
            //        r.Eval(new[]
            //            {
            //                DicDocumentTypeCodes.ConclusionOfInventionPatentGrant,
            //                DicDocumentTypeCodes.ConclusionOfInventionPatentGrantRefuse,
            //            },
            //            RouteStageCodes.DocumentOutgoing_02_1))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_2));



            //// Альтернативный сценарий
            WorkflowStage("Из этапа 'Экспертиза по существу' на этап 'Запрос экспертизы'", "8C857E16-B6AD-456B-8466-89D5FF78773A")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_4)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                        {
                            DicDocumentTypeCodes.RequestForSubstantiveExamination,
                        }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1_1));

            WorkflowStage("Из этапа 'Внесение изменений' на этап 'Запрос экспертизы'", "9C38FD72-9A74-4AAB-960D-6A955EA74130")
                .WhenCurrentStageCode(RouteStageCodes.I_03_9)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicDocumentTypeCodes.RequestForExaminationOfInventionPatentRequest
                    }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1_1));

            WorkflowStage("Из этапа 'Преобразование заявки на полезную модель' на этап 'Запрос экспертизы'", "040F5D04-DA50-46E1-8500-F7B770A017FE")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_1__1)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicDocumentTypeCodes.RequestForExaminationOfInventionPatentRequest,
                    }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1_1));




            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Продление срока' на этап 'Запрос экспертизы'", "541B79C2-D452-4956-934D-C83B53A5E7DB")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_0)
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest }))
            //    .And<IsRequestHasPreviousStageByCodeRule>(r => r.Eval(RouteStageCodes.I_03_3_1_1))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1_1));


            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Продление срока' на этап 'Отозванные'", "2C483DB4-5FD8-4EA1-9C85-2CF7904583D6")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_0)
            //    .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_04_0))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_04_0));

            //WorkflowStage("Из этапа 'Вынесено экспертное заключение' на этап 'На утверждении руководства'", "4F8C3CE7-1177-4962-946C-C5A043358E57")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_2)
            //    .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule>(r =>
            //        r.Eval(new[]
            //        {
            //            DicDocumentTypeCodes.ConclusionOfInventionPatentGrant,
            //            DicDocumentTypeCodes.ConclusionOfInventionPatentGrantRefuse
            //        },
            //            RouteStageCodes.DocumentOutgoing_02_2))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_2_1));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'На утверждении руководства' на этап 'Утверждено руководством'", "24A063BB-1657-4DFE-8F10-783DE4BAEAF3")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_2_1)
            //    .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeAndLaterCurrentStageRule>(r => r.Eval(new[]
            //        {
            //            DicDocumentTypeCodes.ConclusionOfInventionPatentGrant,
            //            DicDocumentTypeCodes.ConclusionOfInventionPatentGrantRefuse
            //        },
            //        RouteStageCodes.DocumentOutgoing_02_3))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_3));


            //WorkflowStage("Из этапа 'Утверждено руководством' на этап 'Передано в МЮ РК'", "BAC8D903-330A-4D18-87EA-A815F69BB67F")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_3)
            //    .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesRule>(r =>
            //        r.Eval(new[]
            //        {
            //            DicDocumentTypeCodes.ConclusionOfInventionPatentGrant,
            //            DicDocumentTypeCodes.ConclusionOfInventionPatentGrantRefuse
            //        }))
            //        .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
            //        r.Eval(new[]
            //        {
            //            DicDocumentTypeCodes.ReestrExpertConclusionToMJ
            //        }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_4));

            //WorkflowStage("Из этапа 'Передано в МЮ РК' на этап 'Возвращено с МЮ РК'", "424464B8-C9A5-4AB2-B21F-1F5592F66753")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_4)
            //    .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.DecisionOfAuthorizedBody
            //    }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_5));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Отказано в выдаче охранного документа' на этап 'Рассмотрение возражения на Апелляционном совете'", "EACAAD40-43A4-411C-8F29-40E4439750FC")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_4_1)
            //    .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.ConclusionOfInventionPatentGrantRefuse
            //    }))
            //    .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.Objection
            //    }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_4_1_0));

            //WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Подготовка для передачи в Госреестр'", "8BC8828F-1CB3-45D6-A6F5-B4BCC5D5FE9F")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_5)
            //    .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.ConclusionOfInventionPatentGrant
            //    }))
            //    .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r =>
            //     r.Eval(new[]
            //     {
            //        DicDocumentTypeCodes.NotificationOfDecisionPatentGrant
            //     }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_7_1));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Отказано в выдаче охранного документа' на этап 'Делопроизводство прекращено'", "CA42AEFA-7CD3-4768-BA65-E6DAA0A9C58A")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_4_1)
            //    .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_03_3_9))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_9));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Продление срока' на этап 'Делопроизводство прекращено'", "C226E0CA-EA5E-4369-9D91-42BBDF2392AD")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_0)
            //    .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_03_3_9))
            //     .And<IsRequestHasNotDocumentWithCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Objection }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_9));

            //WorkflowStage("Из этапа 'Подготовка для передачи в Госреестр' на этап 'Готовые для передачи в Госреестр'", "A97B916B-C594-4AE1-9EC3-10EB0D184B50")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_7_1)
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent }))
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.StateFee2018 }))
            //    .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.NotificationOfPaymentOfStateDuty }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_8));

            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Отозванные' на этап 'Готовые для передачи в Госреестр'", "1A845C81-AC80-4D1A-8700-E5316CCE57F2")
            //    .WhenCurrentStageCode(RouteStageCodes.I_04_0)
            //    .And<IsRequestHasPayedInvoicesWithTariffCodesAndLaterCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent }))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_8));


            //// Альтернативный сценарий
            //WorkflowStage("Из этапа 'Подготовка для передачи в Госреестр' на этап ' Отозванные'", "8AE3081D-33BD-4DD8-9658-38BBE42D6719")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_7_1)
            //    .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_04_0))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_04_0));



            //WorkflowStage("Из этапа 'Готовые для передачи в Госреестр' на этап 'Создание ОД на патент'", "80B98DDD-C7FE-4FC4-AB16-3610C8A96361")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_8)
            //    .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.I_04_1))
            //    .And<IsTodayIsDayRule>(r => r.Eval(System.DayOfWeek.Monday))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_04_1));

            //WorkflowStage("Из этапа 'Готовые для передачи в Госреестр' на этап 'Создание ОД на патент'", "61EE3FCF-D3EE-4704-93A2-0F9AB02758F1")
            //    .WhenCurrentStageCode(RouteStageCodes.I_03_3_8)
            //    .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.ApplicationForEarlyPublication }))
            //    .And<IsTodayIsDayRule>(r => r.Eval(System.DayOfWeek.Monday))
            //    .Then(SendRequestToNextStage(RouteStageCodes.I_04_1));

            #region Преобразования

            //todo workaround для демо
            /*WorkflowStage("Из любого этапа на этап 'Преобразование заявки на полезную модель'", "9443B7E5-5609-486B-83B4-991C97F4EC3F")
                .UseForAllStages()
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Author))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Correspondence))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Declarant))
                .And<IsRequestNotOfConventionTypeRule>(r => r.Eval(DicConventionTypeCodes.PctConvention))
                .And<IsRequestHasAnyNameRule>(r => r.Eval())
                .And<IsRequestHasReferatRule>(r => r.Eval())
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));*/

            WorkflowStage("Из этапа 'Формирование данных заявки' на этап 'Преобразование заявки на полезную модель'", "9443B7E5-5609-486B-83B4-991C97F4EC3F")
                .WhenCurrentStageCode(RouteStageCodes.I_02_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Author))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Correspondence))
                .And<IsRequestHasCustomerWithRoleCodeRule>(r => r.Eval(DicCustomerRoleCodes.Declarant))
                .And<IsRequestNotOfConventionTypeRule>(r => r.Eval(DicConventionTypeCodes.PctConvention))
                .And<IsRequestHasAnyNameRule>(r => r.Eval())
                .And<IsRequestHasReferatRule>(r => r.Eval())
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Секретка' на этап 'Преобразование заявки на полезную модель'", "65FB1F70-69C2-4943-AC54-4C044A951ABE")
                .WhenCurrentStageCode(RouteStageCodes.I_02_3)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Ввод оплат' на этап 'Преобразование заявки на полезную модель'", "7C572B60-2F12-4976-92CB-9DDE1E2262A5")
                .WhenCurrentStageCode(RouteStageCodes.I_02_2)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Ожидает оплату за экспертизу по существу' на этап 'Преобразование заявки на полезную модель'", "22DD473F-7AE8-4340-80CA-70D56A387ADB")
                .WhenCurrentStageCode(RouteStageCodes.I_02_2_0)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Готовые для передачи на формальную экспертизу' на этап 'Преобразование заявки на полезную модель'", "F1E21851-32FB-4273-9EF4-E4CF4AE15C05")
                .WhenCurrentStageCode(RouteStageCodes.I_02_2_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Ожидает оплату за подачу' на этап 'Преобразование заявки на полезную модель'", "B7FD56CB-344F-40CF-BA03-3975CAC82641")
                .WhenCurrentStageCode(RouteStageCodes.I_02_2_0_0)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Распределение эксперту на проведение экспертизы по существу (после поиска)' на этап 'Преобразование заявки на полезную модель'", "365BBD7B-EC48-4BE4-B9A0-1F981985F60C")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_3_0)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Формальная экспертиза изобретения' на этап 'Преобразование заявки на полезную модель'", "E6F839B6-5153-4DB8-AA8D-09AC0EC2077A")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Вынесено экспертное заключение' на этап 'Преобразование заявки на полезную модель'", "DFC68559-5583-4DBC-AD00-CF2C9107F9D0")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_2)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Утверждено руководством ' на этап 'Преобразование заявки на полезную модель'", "E10ECB5B-1BF1-4957-86DF-3772B7BC4A39")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_3)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Продление срока ' на этап 'Преобразование заявки на полезную модель'", "588D1301-466B-4D37-920A-A6C20FA309C8")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_0)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Экспертиза по существу' на этап 'Преобразование заявки на полезную модель'", "8CE0426B-657B-46FB-8B62-5F2301C674F3")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_4)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Преобразование заявки на полезную модель'", "797A6995-E3F0-40A3-9A40-A171840467EE")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Проведение поиска (Астана)' на этап 'Преобразование заявки на полезную модель'", "D3B5B12C-7579-4D0B-87DB-79D00CDCC02A")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_2_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Проведение поиска (Алматы)' на этап 'Преобразование заявки на полезную модель'", "411D897B-8001-46E3-B349-200694E65245")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_2_0)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Готовые для передачи на экспертизу по существу' на этап 'Преобразование заявки на полезную модель'", "19779E4B-88E4-4982-99AB-60EA18EFFEB6")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_1_1)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Подготовка к экспертизе по существу' на этап 'Преобразование заявки на полезную модель'", "509100A4-406C-473F-B7CF-FC8F570BB2A8")
                .WhenCurrentStageCode(RouteStageCodes.I_03_2_1_0)
                .And<IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes._001_004G_1 }))
                .And<IsRequestHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.NEW_019 }))
                .Then(SendRequestToNextStage(RouteStageCodes.I_03_3_1__1));

            WorkflowStage("Из этапа 'Запрос экспертизы' на этап основного сценария",
                    "09D37DE6-5589-4DB6-9C20-7CBA031B3E9A")
                .WhenCurrentStageCode(RouteStageCodes.I_03_3_1_1)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(ReturnFromSendRequestScenario());
            #endregion
        }
        private Action SendToNextSatgeAndSetCoefficientComplexityRequest(string stageCode)
        {
            return () =>
            {
                SendRequestToNextStage(stageCode)?.Invoke();
                Executor.GetHandler<SetCoefficientComplexityRequestHandler>().Process(h => h.Execute(WorkflowRequest.RequestId));
            };
        }
    }
}
