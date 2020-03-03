using System.Linq;
using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.RulesRequest;

namespace NetCoreWorkflow.WorkFlows.Requests
{
    /// <summary>
    /// Рабочий процесс по заявке "Товарные знаки"
    /// </summary>
    public class RequestTradeMarkWorkflow : BaseRequestWorkflow
    {
        public RequestTradeMarkWorkflow(
            IIntegrationStatusUpdater integrationStatusUpdater,
            IIntegrationDocumentUpdater integrationDocumentUpdater,
            IMapper mapper) : base(integrationStatusUpdater, integrationDocumentUpdater, mapper)
        {
            //InitStages();
        }

        private void InitStages()
        {
            var formalExamRequests = new[]
            {
                DicDocumentTypeCodes.DeclarantAddressMismatchRequest,
                DicDocumentTypeCodes.EcoBioOrganicDesignationRequest,
                DicDocumentTypeCodes.IcgsMismatchRequest,
                DicDocumentTypeCodes.TranslationRequest,
                DicDocumentTypeCodes.PriorityRequest,
                DicDocumentTypeCodes.TZ_ZAP_O_DOV,
                //DicDocumentTypeCodes.OUT_Zap_Pol_dover_v1_19, //TODO

                //DicDocumentTypeCodes.OUT_Zap_Pred_Adres_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_ECO_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_Nesootv_MKTU_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_Perevod_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_Prior_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_dover_v1_19,
                DicDocumentTypeCodes.OUT_Zap_Pred_gos_sim_v1_19,
                DicDocumentTypeCodes.TZ_ZAP_O_IZOBR,
                DicDocumentTypeCodes.IcgsOrImageMissingRequest,
                DicDocumentTypeCodes.DesignationTranslationRequest
            };
            var fullExamRequests = new[]
            {
                //DicDocumentTypeCodes.OUT_Zap_Pol_dover_v1_19, //TODO

                DicDocumentTypeCodes.OUT_Zap_Pol_avtor_v1_19,
                DicDocumentTypeCodes.TZ_ZAP_O_DOV,
                DicDocumentTypeCodes.OUT_Zap_Pol_gos_sim_v1_19,
                DicDocumentTypeCodes.TZ_ZAP_O_IZOBR,
                DicDocumentTypeCodes.OUT_Zap_Pol_kult_dost_v1_19,
                DicDocumentTypeCodes.OUT_Zap_Pol_lich_neimush_v1_19,
                DicDocumentTypeCodes.OUT_Zap_Pol_obyect_sv_v1_19,
                DicDocumentTypeCodes.DesignationTranslationRequest,
                DicDocumentTypeCodes.OUT_Zap_Pol_pismo_sogl_v1_19
            };
            var allRequests = formalExamRequests.Concat(fullExamRequests).ToArray();

            WorkflowStage("Ручной переход этапов", "67EE3330-C6EC-4FDB-907B-945C6B316472")
                .UseForAllStages()
                .And<IsRequestExistsNextRouteStageByCodeRule>(r =>
                    r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code,
                        WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Из любого этапа на этап 'Отозвано'", "A6FF99AE-A149-411F-B3F1-3A7B68B2EB2F")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionOfApplicationRevocation}))
                .Then(SendRequestToNextStage(RouteStageCodes.RequestCanceled));

            WorkflowStage("Из любого этапа на этап 'Делопроизводство приостановлено'",
                    "B1077DD0-BEE2-45D6-954B-899259BF41CD")
                .UseForAllStages()
                .And<IsRequestDocumentHasCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForSuspensionOfOfficeWork}))
                .Then(SendRequestToNextStage(RouteStageCodes.ITZ_03_3_2_1));

            WorkflowStage("Ручной возврат этапов", "9E00D2D3-28FF-426D-8577-BC452C18FE52")
                .UseForAllStages()
                .And<IsRequestExistsPreviousRouteStageByCodeRule>(r =>
                    r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code,
                        WorkflowRequest?.NextStageCode))
                .Then(ReturnRequestToPreviousStage());

            WorkflowStage("Переход по текущему этапу вручную", "20CD808D-3568-4A14-870F-6F103784EDDD")
                .UseForAllStages()
                .And<IsRequestBeingSentToTheSameStageRule>(r => r.Eval())
                .Then(SendRequestToSameStage());

            WorkflowStage("Из этапа 'Ввод оплаты' на этап 'Выбор исполнителя предварительной экспертизы'",
                    "A22DD037-6CAB-4DE6-91C0-8B0628702858")
                .WhenCurrentStageCode(RouteStageCodes.TZ_02_2)
                .And<IsRequestHasNotMoreThanIcgsRule>(r => r.Eval())
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[]
                {
                    DicTariffCodes.CollectiveTmFormalExpertizeDigital,
                    DicTariffCodes.CollectiveTmFormalExpertizePaper,
                    DicTariffCodes.TmNmptFormalExpertizeDigital,
                    DicTariffCodes.TmNmptFormalExpertizePaper
                }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[]
                {
                    DicTariffCodes.CollectiveTmNmptFullExpertizeDigital,
                    DicTariffCodes.CollectiveTmNmptFullExpertizePaper,
                    DicTariffCodes.TmNmptFullExpertizeDigital,
                    DicTariffCodes.TmNmptFullExpertizePaper
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_2_1));

            WorkflowStage("Из этапа 'Ввод оплаты' на этап 'Выбор исполнителя предварительной экспертизы'",
                    "C9D54E99-1A0F-4843-BC19-9D0264BECB7E")
                .WhenCurrentStageCode(RouteStageCodes.TZ_02_2)
                .And<IsRequestHasMoreThanIcgsRule>(r => r.Eval())
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[]
                {
                    DicTariffCodes.CollectiveTmFormalExpertizeDigital,
                    DicTariffCodes.CollectiveTmFormalExpertizePaper,
                    DicTariffCodes.TmNmptFormalExpertizeDigital,
                    DicTariffCodes.TmNmptFormalExpertizePaper
                }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[]
                {
                    DicTariffCodes.CollectiveTmNmptFullExpertizeDigital,
                    DicTariffCodes.CollectiveTmNmptFullExpertizePaper,
                    DicTariffCodes.TmNmptFullExpertizeDigital,
                    DicTariffCodes.TmNmptFullExpertizePaper
                }))
                .And<IsExceededIcgsRequestsPaidForRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariffCodes.CollectiveTmFormalExpertizeMoreThanThreeIcgsClassesDigital,
                        DicTariffCodes.CollectiveTmFormalExpertizeMoreThanThreeIcgsClassesPaper,
                        DicTariffCodes.TmNmptFormalExpertizeMoreThanThreeIcgsClassesDigital,
                        DicTariffCodes.TmNmptFormalExpertizeMoreThanThreeIcgsClassesPaper
                    }))
                .And<IsExceededIcgsRequestsPaidForRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariffCodes.TmNmptFullExpertizeMoreThanThreeIcgsClassesPaper,
                        DicTariffCodes.TmNmptFullExpertizeMoreThanThreeIcgsClassesDigital,
                        DicTariffCodes.CollectiveTmNmptFullExpertizeMoreThanThreeIcgsClassesDigital,
                        DicTariffCodes.CollectiveTmNmptFullExpertizeMoreThanThreeIcgsClassesPaper
                    }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_2_1));

            WorkflowStage(
                    "Из этапа 'Предварительная экспертиза' на этап 'Выбор исполнителя 1ого этапа полной экспертизы'",
                    "DB5FD51E-BEA9-4C9B-89D7-9ACE7063B4EF")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_2_2)
                .And<IsRequestHasNotMoreThanIcgsRule>(r => r.Eval())
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[]
                {
                    DicTariffCodes.CollectiveTmNmptFullExpertizeDigital,
                    DicTariffCodes.CollectiveTmNmptFullExpertizePaper,
                    DicTariffCodes.TmNmptFullExpertizeDigital,
                    DicTariffCodes.TmNmptFullExpertizePaper
                }))
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.TZPRED1 }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZFirstFullExpertizePerformerChoosing));

            WorkflowStage(
                    "Из этапа 'Предварительная экспертиза' на этап 'Выбор исполнителя 1ого этапа полной экспертизы'",
                    "AC9F5F29-8E5A-4450-97D8-9F2CE8973EC1")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_2_2)
                .And<IsRequestHasMoreThanIcgsRule>(r => r.Eval())
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[]
                {
                    DicTariffCodes.CollectiveTmNmptFullExpertizeDigital,
                    DicTariffCodes.CollectiveTmNmptFullExpertizePaper,
                    DicTariffCodes.TmNmptFullExpertizeDigital,
                    DicTariffCodes.TmNmptFullExpertizePaper
                }))
                .And<IsExceededIcgsRequestsPaidForRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariffCodes.TmNmptFullExpertizeMoreThanThreeIcgsClassesPaper,
                        DicTariffCodes.TmNmptFullExpertizeMoreThanThreeIcgsClassesDigital,
                        DicTariffCodes.CollectiveTmNmptFullExpertizeMoreThanThreeIcgsClassesDigital,
                        DicTariffCodes.CollectiveTmNmptFullExpertizeMoreThanThreeIcgsClassesPaper
                    }))
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.TZPRED1}))
                .Then(SendRequestToNextStage(RouteStageCodes.TZFirstFullExpertizePerformerChoosing));

            WorkflowStage("Из этапа 'Ожидание восстановления пропущенного срока' на этап 'Восстановление срока'",
                    "5DED5A4D-E830-46FE-B2E3-54F4AB72D6C8")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_4)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] {DicDocumentTypeCodes.PetitionForRestoreTime}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TimeRestore}))
                .Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));

            WorkflowStage("Из этапа '2ой этап полной экспертизы' на этап 'Передано на подготовку уведомления'",
                    "C8EF36AE-9FC2-478B-8B55-53F83C7CCB2A")
                .WhenCurrentStageCode(RouteStageCodes.TZSecondFullExpertize)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.ExpertTmRegisterOpinion },
                    RouteStageCodes.DocumentInternal_IN01_1_0))
                .Then(SendRequestToNextStage(RouteStageCodes.TZNotificationPreparation));

            WorkflowStage("Из этапа '2ой этап полной экспертизы' на этап 'Ожидания возражения заявителя'",
                    "E6712FF8-CCC4-4E22-8978-D52A1510981F")
                .WhenCurrentStageCode(RouteStageCodes.TZSecondFullExpertize)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicDocumentTypeCodes.ExpertTmRegisterRefusalOpinion,
                        DicDocumentTypeCodes.ExpertTmRegistrationOpinionWithDisclaimer
                    }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZAwaitingDeclarantObjection));

            WorkflowStage("Из этапа 'Ожидания возражения заявителя' на этап 'Рассмотрение возражения'",
                    "B0A6A51F-9E0D-48E8-AAF5-17C161E2CBF1")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingDeclarantObjection)
                .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[] {DicDocumentTypeCodes.Objection}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.PreliminaryRejectionObjectionConsideration}))
                .Then(SendRequestToNextStage(RouteStageCodes.TZObjectionConsideration));

            WorkflowStage(
                    "Из этапа 'Ожидания возражения заявителя' на этап 'Вынесение решения о частичной регистрации'",
                    "F35AC87C-99C3-4348-B3E5-5D516C4FDE3D")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingDeclarantObjection)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionOfApplicantConsent}))
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.ExpertTmRegistrationOpinionWithDisclaimer }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZPartialRegistrationDecision));

            WorkflowStage("Из этапа 'Ожидания возражения заявителя' на этап 'Вынесение решения об отказе'",
                    "BF1665F8-154C-4F40-99C3-827630E4E9D6")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingDeclarantObjection)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionOfApplicantConsent}))
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.ExpertTmRegisterRefusalOpinion }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZRejectionDecision));

            WorkflowStage("Из этапа 'Ожидания возражения заявителя' на этап 'Продление срока'",
                    "C29C0A09-B5CF-4D3A-8F47-D46D6226BAA8")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingDeclarantObjection)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorObjections}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorObjections))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Ожидания возражения заявителя' на этап 'Продление срока'",
                    "A8894882-E35A-4C4F-8D88-1B2B11BB5B26")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingDeclarantObjection)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorObjections}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorObjections))
                    .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Ожидания возражения заявителя' на этап 'Продление срока'",
                    "5C7135B9-FF40-4CF8-BA30-B9368D871382")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingDeclarantObjection)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorObjections}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorObjections))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Ожидания возражения заявителя' на этап 'Продление срока'",
                    "E85A30C0-A89A-4361-A97E-85BE1DBB69FA")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingDeclarantObjection)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorObjections}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorObjections))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Ожидания возражения заявителя' на этап 'Продление срока'",
                    "31809A22-D205-411A-A1BF-047602F9DA8F")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingDeclarantObjection)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorObjections}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorObjections))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Ожидания возражения заявителя' на этап 'Продление срока'",
                    "AFFE219E-9121-48A4-AC46-19A239BE66AB")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingDeclarantObjection)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorObjections}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorObjections))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Рассмотрение возражения'",
                    "2A4882D3-D93B-463A-BDFD-C1B8AB9DE8F5")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestHasDocumentWithCodeRule>(r => r.Eval(new[] {DicDocumentTypeCodes.Objection}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.PreliminaryRejectionObjectionConsideration}))
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TimeRestore}))
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.OfficeWorkRestartNotification,
                    DicDocumentTypeCodes.ResponseDelapFormsExp,
                    DicDocumentTypeCodes.OfficeWorkRestartNotification
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZObjectionConsideration));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Восстановление срока'",
                    "BA1D0E49-D63F-4EB6-8F4F-C16A919525F8")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TimeRestore}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.Petition_001_004G_3))
                .Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));
                //    r.Eval(DicDocumentTypeCodes.ObjectionAwaitingTermProlongationPetition))
                //.Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Восстановление срока'",
                    "93984165-1DB2-41AF-94E3-EC6765FB8F6D")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TimeRestore}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.Petition_001_004G_3))
                .Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));
                //    r.Eval(DicDocumentTypeCodes.ObjectionAwaitingTermProlongationPetition))
                //.Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Восстановление срока'",
                    "B23DEBEB-1307-4AC1-BC8A-D5A49874D93C")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TimeRestore}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.Petition_001_004G_3))
                .Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));
                //    r.Eval(DicDocumentTypeCodes.ObjectionAwaitingTermProlongationPetition))
                //.Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Восстановление срока'",
                    "17DFC6F5-6A64-4210-8F16-04FCA5D4E071")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TimeRestore}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.Petition_001_004G_3))
                .Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));
                //    r.Eval(DicDocumentTypeCodes.ObjectionAwaitingTermProlongationPetition))
                //.Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Восстановление срока'",
                    "358444D7-25BD-4B8E-BE73-8BB39CCF852C")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TimeRestore}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.Petition_001_004G_3))
                .Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));
                //    r.Eval(DicDocumentTypeCodes.ObjectionAwaitingTermProlongationPetition))
                //.Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Восстановление срока'",
                    "0CDA4002-114C-4868-A8E9-8B860855D9C5")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.Petition_001_004G_3 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TimeRestore}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.Petition_001_004G_3))
                .Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));
                //    r.Eval(DicDocumentTypeCodes.ObjectionAwaitingTermProlongationPetition))
                //.Then(SendRequestToNextStage(RouteStageCodes.TZTermRestart));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "59F117C5-8AD0-494A-B966-1A06758919D6")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.ResponseDelapFormsExp }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Рассмотрение возражения' на этап 'Передано на подготовку уведомления'",
                    "04E31687-C477-426D-A6C2-5603C684DC8D")
                .WhenCurrentStageCode(RouteStageCodes.TZObjectionConsideration)
                .And<IsDocumentSignedAtStageRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.TrademarkPartialRegistrationDecision, DicDocumentTypeCodes.TrademarkRegistrationDecision },
                        RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendRequestToNextStage(RouteStageCodes.TZNotificationPreparation));

            WorkflowStage("Из этапа 'Решение Апелляционного Совета' на этап 'Передано на подготовку уведомления'",
                    "3de7e2b3-0f92-4618-b7af-a87ef1081ff9")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_9_2_0)
                .And<IsAnyDocumentHasOutgoingNumberByCodesRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.OUT_UV_Pol_Reg_TZ_AP_sov_v1_19 }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZNotificationPreparation));

            WorkflowStage("Из этапа 'Рассмотрение возражения' на этап 'Вынесение решения об отказе'",
                    "b64949d0-e398-4f55-a84a-b5985c7a67eb")
                .WhenCurrentStageCode(RouteStageCodes.TZObjectionConsideration)
                .And<IsDocumentSignedAtStageRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.TrademarkRegistrationRejectionDecision },
                        RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendRequestToNextStage(RouteStageCodes.TZRejectionDecision));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "D394F9F1-E096-46D9-8978-700EE64E847A")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "8CFB33C8-FFE5-4FD4-BCC5-086D31BAB922")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "12256B99-F476-41F0-9A91-841B5E09D27A")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "05B7FF6F-01DC-4AD3-80CE-1B86592D3C54")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "C2B98B95-96F8-4C0A-AFE3-5BA5716F5762")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "221DB572-5FAC-4B78-BAF3-18E5D91CFB8C")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "E4912B2E-2486-45C0-A050-0E927305007B")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.ResponseTermProlongationNotification }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "1F5EAFC2-AF16-421D-B6B5-B0EA761FAB40")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.ResponseTermProlongationNotification }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "B67F7D0D-6C61-4539-99A2-C82BED6618DB")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                 .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.ResponseTermProlongationNotification }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "0213BFE3-13EC-43F7-A21B-69F465B7A977")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.ResponseTermProlongationNotification }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "D19C4177-52EC-4331-9290-762F5618F631")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.ResponseTermProlongationNotification }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "971C4911-825F-4FE3-8242-7B27E441474D")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.ResponseTermProlongationNotification }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "C136437F-D61D-416F-8887-068153C3DCA8")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "F02BFC4A-F4F5-4604-9C9A-E51BCCF73D9B")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "BF873B56-3759-4149-B610-79BFA4EFD58C")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "AC474A0F-C7C7-4C04-A268-819A10DBFEB2")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "DA31F7FD-9361-4E11-98D2-8B0E5D82B39F")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "FF9993DA-96DD-4C99-95B0-C3F40442FBDD")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "2FC108F0-B741-48F2-9CE8-97190222C77B")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.MTZ_NEW_7 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "988AAEA9-6CF0-4B1A-9D73-62E4A8383121")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.MTZ_NEW_7 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "15B45DFB-4C11-4DB4-B197-2FBE3BDAC27F")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.MTZ_NEW_7 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "584A99D4-E422-4A31-B6B4-CFDE9316A6B4")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.MTZ_NEW_7 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "97CBA49A-E3BF-4AC9-BBB6-83D856A36656")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.MTZ_NEW_7 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "BC801FCA-4931-4A94-B1AC-E89DBE415754")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsDocumentHasOutgoingNumberByCodeRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.MTZ_NEW_7 }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Ожидания возражения заявителя'",
                    "B5010118-4298-4A9A-BB7E-E925BCB28BDD")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_3)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] {DicDocumentTypeCodes.AnswerToRequest }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZAwaitingDeclarantObjection));


            WorkflowStage("Из этапа 'Продление срока' на этап 'Направлен запрос (предварительная)'",
                    "7B7667E0-84AE-4035-BDCF-92FB23E436CD")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_3)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.ResponseTermProlongationNotification }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_1));
                //    r.Eval(new[] {DicDocumentTypeCodes.OUT_UV_Pred_prodl_srok_na_otv_v1_19}))
                //.Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_1));

            WorkflowStage("Из этапа 'Направлен запрос' (истекший) на этап 'Ожидание восстановления пропущенного срока'",
                    "1DC5A285-3562-4FA6-A5E7-147D04B8F44B")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.TZPRED3,
                    //DicDocumentTypeCodes.OUT_Uv_pred_prekr_del_otv_zap_v1_19,
                    DicDocumentTypeCodes.NotificationOfPaymentlessOfficeWorkTerminationFormal
                }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_4));

            WorkflowStage("Из этапа 'Направлен запрос' (истекший) на этап 'Ожидание восстановления пропущенного срока'",
                    "1C065149-CBD8-4C5D-A85A-E305A66D8CAC")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.TZPRED3,
                    //DicDocumentTypeCodes.OUT_Uv_pred_prekr_del_otv_zap_v1_19,

                }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_4));

            WorkflowStage("Из этапа 'Предварительная экспертиза' на этап 'Направлен запрос (предварительная)'",
                    "A42B36BB-D3BE-4BDE-BCC5-FE661ECF6E5E")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_2_2)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new string[]{ DicDocumentTypeCodes.OUT_Zap_Pol_dover_v1_19 }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_1));

            WorkflowStage("Из этапа '1ый этап полной экспертизы' на этап 'Направлен запрос (полная)'",
                    "28CAC22E-46E0-40F4-A14C-C089F416925E")
                .WhenCurrentStageCode(RouteStageCodes.TZFirstFullExpertize)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new string[] { DicDocumentTypeCodes.OUT_Zap_Pol_dover_v1_19 }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_1));

            WorkflowStage("Из этапа 'Передано на подготовку уведомления' на этап 'Направлено уведомление заявителю'",
                    "4D2821E3-3506-4EBB-8453-7B6161A64D24")
                .WhenCurrentStageCode(RouteStageCodes.TZNotificationPreparation)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicDocumentTypeCodes.ResponseTermProlongationNotification,
                        DicDocumentTypeCodes.OUT_UV_Pol_Reg_TZ_AP_sov_v1_19,
                        DicDocumentTypeCodes.NotificationOfRegistrationDecision,
                    }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZNotificationSentToDeclarant));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления  срока оплаты за  регистрацию' на этап 'Готовые для передачи в Госреестр'",
                    "D5A31E63-B6E8-43C3-85E6-6AA79B8FDDAA")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingRegistrationTermRestoration)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] {DicDocumentTypeCodes.PetitionForRestoreTime}))
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] {DicDocumentTypeCodes.StateServicesRequest}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TimeRestore}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.TrademarNmptRegistrationAndPublishing}))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_8));

            WorkflowStage(
                    "Из этапа 'Направлено уведомление заявителю' на этап 'Готовые для передачи в Госреестр'",
                    "b124ce70-21a6-4c30-a012-8b17b8fcd638")
                .WhenCurrentStageCode(RouteStageCodes.TZNotificationSentToDeclarant)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] {DicDocumentTypeCodes.StateServicesRequest}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TrademarNmptRegistrationAndPublishing}))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_8));

            WorkflowStage(
                    "Из этапа 'Направлено решение заявителю' на этап 'Готовые для передачи в Госреестр'",
                    "0BA2B5E9-175B-4A95-9B6B-9492D1B1C1CA")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7)
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[] { DicDocumentTypeCodes.StateServicesRequest }))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] { DicTariffCodes.TrademarNmptRegistrationAndPublishing }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_8));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "2001FD5A-0A47-4EC3-A1F8-A99CD849720F")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "8D3511A3-66B8-4901-941D-E8DF77EF3AFD")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "EBE07713-ECD9-4875-89A4-DB5D95450016")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "5E023F19-56CE-4444-857D-33286B2B9411")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "E958259D-2B0C-47CF-9A9A-9209760B79C1")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "EA9216E7-1F5F-415C-B24B-8BF3F6E3CA2A")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "A1E37943-F9B0-46D8-A46E-2C80F975A5F6")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "AEADFC48-F5BC-4EED-8735-A92FF645FE69")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "FF800B64-01C0-4229-8DB2-3E7A49E80F2E")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "E59784DE-42A6-4D34-B00F-0BA2BDFB9EE1")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "84D649BA-1825-4B84-8F69-B3664E766990")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "CD0F61B6-439C-491E-96D8-BCF9F5752C6E")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "E5BD5DA3-77B7-4CEB-90F5-F7DE14CFFE15")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "DB6614AA-D9AD-420E-B1F3-AB5DA33B0F44")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "DB9C37BD-FD22-4E08-88B2-253871785072")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "2E0CB9DB-D202-475D-A8FD-2742E85B405C")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "5CBFA324-52E9-4035-B7C1-31DD1FD8901F")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "5C030A6A-1E33-4688-B853-D472E654CAB8")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "14A62F41-1857-4736-A2A6-0455C7B8D319")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "A125CE6C-FB3D-4DEC-B6A7-EEE084B11821")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "145358FA-C727-4247-9FC8-26180D47D0F0")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "2D72E597-43B3-4462-AA2D-F9B8AB5FE6D9")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "A5FA5886-A574-4669-8F80-5F6E61E7E664")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "C797C157-406A-45D0-A3EF-087022359A48")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Преобразование заявки' на этап 'Направлен запрос (преобразование)'",
                    "39CD9A61-C7BC-43BD-AC2A-1658FE2F8F09")
                .WhenCurrentStageCode(RouteStageCodes.TZConvert)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>
                    (r => r.Eval(new string[] { DicDocumentTypeCodes.OUT_Zap_Pol_dover_v1_19 }))

                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_1));

            WorkflowStage("Из этапа 'Разделение заявки на ТЗ' на этап 'Направлен запрос (разделение)'",
                    "820B5F20-597F-45AA-8280-46A26DA08F51")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_2_2)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>
                    (r => r.Eval(new string[] { DicDocumentTypeCodes.OUT_Zap_Pol_dover_v1_19 }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_1));

            WorkflowStage("Из этапа 'Внесение изменений' на этап 'Направлен запрос (внесение изменений)'",
                    "8C069196-F0EC-4934-B39E-D8E9F091AE1C")
                .WhenCurrentStageCode(RouteStageCodes.TZ_06)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>
                    (r => r.Eval(new string[] { DicDocumentTypeCodes.OUT_Zap_Pol_dover_v1_19 }))
                .Then(SendRequestOnChangeScenarioStage(RouteStageCodes.TZ_03_3_7_1));



            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "EAF92F2D-8A40-4888-9C2B-78D5E01BF48E")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "4EEA3C24-7A42-46F4-9B79-C729B59F0E30")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "6DE46E39-4EC4-4D5B-8D74-E3BDDB29A8A6")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "4BB2DE57-FDB7-499D-9B0C-5D28553A03CF")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "DABA4E99-3D7B-4569-B3AA-077B58C165EF")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Направлен запрос' на этап 'Продление срока'",
                    "3782F70F-DE28-443B-9141-6D0A689DCEB8")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "37E85602-E1F3-41D3-BC8D-80697B2A9DCF")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSixthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "559D2E1E-9D6A-471A-9675-92F252301DDC")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFifthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "B3A8FED5-51A1-426A-9906-2B7EEAD6A793")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFourthMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "1792A240-852D-4936-880E-96DEC07A0513")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationThirdMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "243F4875-BC31-4293-B59D-E6BFD0CC26DF")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationSecondMonth}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Из этапа 'Восстановление срока' на этап 'Продление срока'",
                    "ED94C889-121C-4E71-AB82-284F143708CA")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForExtendTimeRorResponse}))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.ResponseTimeProlongationFirstMonth}))
                .And<IsRequestHasEnoughProlongationPaymentsRule>(r =>
                    r.Eval(DicDocumentTypeCodes.PetitionForExtendTimeRorResponse))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_7_3));

            WorkflowStage("Возврат из этапа 'Внесение изменений'", "A14AEC03-B882-4B3F-A6B9-D54F6B058C0A")
                .WhenCurrentStageCode(RouteStageCodes.TZ_06)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.OUT_UV_Pred_izmen_adres_v1_19,
                    DicDocumentTypeCodes.OUT_UV_Pred_izmen_naim_adr_v1_19,
                    DicDocumentTypeCodes.UV_TZ_VN_IZM_NAIMEN_ZAYAV,
                    DicDocumentTypeCodes.UV_TZ_VN_IZM,
                    DicDocumentTypeCodes.UV_TZ_VN_IZM_PERECH_TOV,
                    DicDocumentTypeCodes.UV_TZ_VN_IZM_PRED_ZAYAV,
                    DicDocumentTypeCodes.OUT_UV_Pred_izmen_yur_adr_v1_19,
                    DicDocumentTypeCodes.UV_TZ_VN_IZM_YUR_ADR_PER
                }))
                .Then(ReturnRequestToPreviousStage());

            WorkflowStage(
                    "Из этапа 'Вынесение решения о частичной регистрации' на этап 'Передано на подготовку уведомления'",
                    "2224EED4-FA24-43A7-8FDD-C68DFCD2123A")
                .WhenCurrentStageCode(RouteStageCodes.TZPartialRegistrationDecision)
                .And<IsDocumentSignedAtStageRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.TrademarkPartialRegistrationDecision },
                        RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendRequestToNextStage(RouteStageCodes.TZNotificationPreparation));

            WorkflowStage("Возврат с этапа 'Преобразование заявки' на предыдущий этап",
                    "A54B40C6-9D22-4235-BC9D-0E8AC570FFAB")
                .WhenCurrentStageCode(RouteStageCodes.TZConvert)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19,
                }))
                .Then(ReturnRequestToPreviousStage());

            WorkflowStage("Возврат с этапа 'Разделение заявки на ТЗ' на предыдущий этап",
                    "9E46AC0D-0A9F-447B-9642-03D7660A03D9")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_2_2)
                .And<IsAnyDocumentHasOutgoingNumberAndSendDateByCodesAndLaterCurrentStageRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.NotificationReleaseRequestRegistrationNotification }))
                .Then(ReturnRequestToPreviousStage());

            WorkflowStage("Из любого этапа этапа на этап 'Преобразование заявки'",
                    "C8E96EA3-B5E1-466B-806E-723010F51869")
                .UseForAllStages()
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.TzConvertPetition}, RouteStageCodes.DocumentIncoming_2_2))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TmConvert}))
                .Then(SendRequestToNextStageWithExecutorFromPetition(RouteStageCodes.TZConvert,
                    new[] {DicDocumentTypeCodes.TzConvertPetition}));

            WorkflowStage("Из любого этапа этапа на этап 'Разделение заявки на ТЗ'",
                    "44A39017-3129-41F6-946C-889C73EF26B1")
                .UseForAllStages()
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.RequestSplitPetition}, RouteStageCodes.DocumentIncoming_2_2))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r => r.Eval(new[] {DicTariffCodes.TmSplit}))
                .Then(SendRequestToNextStageWithExecutorFromPetition(RouteStageCodes.TZ_03_3_2_2,
                    new[] {DicDocumentTypeCodes.RequestSplitPetition}));

            WorkflowStage("Из любого этапа этапа на этап 'Внесение изменений'",
                    "B12B5D06-3C7A-4C4C-BF54-21F75D50E35F")
                .UseForAllStages()
                .And<IsRequestDocumentHasStageByStageCodeAndDocumentCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.PetitionForChanging}, RouteStageCodes.DocumentIncoming_2_2))
                .And<IsRequestHasUnderpaidInvoicesByCodeRule>(r =>
                    r.Eval(new[] {DicTariffCodes.TmChange, DicTariffCodes.TmSameTypeChange}))
                .Then(SendRequestToNextStageWithExecutorFromPetition(RouteStageCodes.TZ_06,
                    new[] {DicDocumentTypeCodes.PetitionForChanging}));

            //Автоэтапы
            WorkflowStage("Из этапа 'Ожидаемые оплату за полную экспертизу' на этап 'Делопроизводство прекращено'",
                    "08B8B8C7-ABBB-4033-B3A7-803ADE2B6C98")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_2_2_0)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZ_03_3_9))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_9));

            WorkflowStage("Из этапа 'Ожидание восстановления пропущенного срока' на этап 'Делопроизводство прекращено'",
                    "505ED477-3483-421A-BA8B-57E93042D417")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_4)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZ_03_3_9))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_9));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления  срока оплаты за  регистрацию' на этап 'Делопроизводство прекращено'",
                    "7D8D6B1E-6457-452C-AA3F-4F57F2D56B4D")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingRegistrationTermRestoration)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZ_03_3_9))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_9));

            WorkflowStage("Из этапа 'Продление срока (предварительная)' на этап 'Делопроизводство прекращено'",
                    "4D278542-11F2-4EBE-A5BD-A9D8B9B28CDE")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_3)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZ_03_3_9))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_9));

            WorkflowStage("Из этапа 'Продление срока (полная)' на этап 'Делопроизводство прекращено'",
                    "6B0FD0AF-3CA1-4132-B427-EA56EC5AD668")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_3)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZ_03_3_9))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_9));

            WorkflowStage("Из этапа 'Продление срока (возражение)' на этап 'Делопроизводство прекращено'",
                    "946B30CC-89C7-4D88-8E42-AC52FA5D4843")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_3)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZ_03_3_9))
                .Then(SendRequestToNextStage(RouteStageCodes.TZ_03_3_9));

            WorkflowStage(
                    "Из этапа 'Ожидания возражения заявителя' на этап 'Ожидание восстановления срока подачи возражения'",
                    "EB299CF3-74B1-49B8-A99A-C79AA6A7992C")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingDeclarantObjection)
                .And<IsRequestExpiredOnCurrentStageRule>(r =>
                    r.Eval(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration))
                .Then(SendRequestToNextStage(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Вынесение решения о частичной регистрации'",
                    "685BF4FC-1229-4E90-8C63-6B2A9358876A")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZPartialRegistrationDecision))
                .And<IsRequestDocumentHasCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.TrademarkPartialRegistrationDecision }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZPartialRegistrationDecision));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Вынесение решения об отказе'",
                    "00D31A3B-2E83-4456-839C-9D8EBFE17EBA")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZRejectionDecision))
                .And<IsRequestDocumentHasCodeRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.TrademarkRegistrationRejectionDecision

                }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZRejectionDecision));

            WorkflowStage("Из этапа 'Ввод оплаты' на этап 'Выбор исполнителя предварительной экспертизы'",
                    "AC396A7C-DA8D-4850-9C8C-FDF04FF8AF17")
                .WhenCurrentStageCode(RouteStageCodes.TZ_02_2)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZ_03_2_1))
                .And<IsRequestHasNotPayedInvoicesWithTariffCodesRule>(r => r.Eval(new[]
                {
                    DicTariffCodes.CollectiveTmFormalExpertizeDigital,
                    DicTariffCodes.CollectiveTmFormalExpertizePaper,
                    DicTariffCodes.TmNmptFormalExpertizeDigital,
                    DicTariffCodes.TmNmptFormalExpertizePaper
                }))
                .Then(SendToNextStageAndSetFormalExamNotPaidFlag(RouteStageCodes.TZ_03_2_1));

            WorkflowStage("Из этапа 'Готовые для передачи в Госреестр' на этап 'Регистрация ТЗ'",
                    "EC55BE72-D7D6-492E-BB48-A55615652BE4")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_8)
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZ_03_3_8))
                .Then(SendRequestToNextStage(RouteStageCodes.TZRegistration));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Вынесение решения о частичной регистрации' по истечению сроков",
                    "A47253B7-548F-4DCD-B49E-98234125F809")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestExpiredOnCurrentStageRule>(r =>
                    r.Eval(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration))
                .And<IsRequestDocumentHasCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.ExpertTmRegistrationOpinionWithDisclaimer }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZPartialRegistrationDecision));

            WorkflowStage(
                    "Из этапа 'Ожидание восстановления срока подачи возражения' на этап 'Вынесение решения об отказе' по истечению сроков",
                    "655BF2E1-81B0-4264-8B87-135D5E9613D5")
                .WhenCurrentStageCode(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration)
                .And<IsRequestExpiredOnCurrentStageRule>(r =>
                    r.Eval(RouteStageCodes.TZAwaitingObjectionSubmissionTermRestoration))
                .And<IsRequestDocumentHasCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.ExpertTmRegisterRefusalOpinion }))
                .Then(SendRequestToNextStage(RouteStageCodes.TZRejectionDecision));

            WorkflowStage("Из этапа 'Продление срока' на этап 'Ожидания возражения заявителя'",
                    "C9532A25-0A71-4848-9165-337A65EEB92D")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_3)
                .And<IsRequestHasDocumentWithCodeRule>(r =>
                    r.Eval(new[] {DicDocumentTypeCodes.ResponseTermProlongationNotification}))
                .And<IsRequestExpiredOnCurrentStageRule>(r => r.Eval(RouteStageCodes.TZAwaitingDeclarantObjection))
                .Then(SendRequestToNextStage(RouteStageCodes.TZAwaitingDeclarantObjection));

            WorkflowStage("Из этапа 'Продление срока' на этап основного сценария",
                    "0B8D361F-925A-4E27-A65A-8170104EBE85")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_3)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes.ResponseTermProlongationNotification }))
                .Then(ReturnFromSendRequestScenario());

            WorkflowStage("Из этапа 'Направлен запрос' на этап основного сценария",
                    "09D37DE6-5589-4DB6-9C20-7CBA031B3E9A")
                .WhenCurrentStageCode(RouteStageCodes.TZ_03_3_7_1)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(ReturnFromSendRequestScenario());

            WorkflowStage("Из этапа 'Восстановление срока' на этап основного сценария",
                    "324CC59C-1206-46BD-AB01-D57FB3E8846F")
                .WhenCurrentStageCode(RouteStageCodes.TZTermRestart)
                .And<IsRequestHasDocumentWithCodeAndLaterCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.AnswerToRequest }))
                .Then(ReturnFromSendRequestScenario());
        }
    }
}
