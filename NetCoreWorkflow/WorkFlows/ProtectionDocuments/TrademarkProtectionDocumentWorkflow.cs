using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage;
using Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using System;

namespace NetCoreWorkflow.WorkFlows.ProtectionDocuments
{
    public class TrademarkProtectionDocumentWorkflow : BaseProtectionDocumentWorkflow
    {
        public TrademarkProtectionDocumentWorkflow()
        {
            //InitStages();
        }

        private void InitStages()
        {
            #region ОД

            WorkflowStage("Ручной переход этапов", "CD782C2A-427D-42FC-A8F8-6C64A8D85CF9")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "9B3A496D-8085-4EBF-AD99-F982EAFE0385")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());


            WorkflowStage("Из этапа 'Печать охранного документа' на этап 'Действующие ТЗ'",
                    "DBE043D3-D91C-4B1B-A32D-BE6B1E1AD500")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.TrademarkCertificate }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_01));

            WorkflowStage("Из этапа 'Печать свидетельства ТЗ' на этап 'Действующие ТЗ'",
                    "d619566b-2476-456b-a1ee-d1efeb279575")
                .WhenCurrentStageCode(RouteStageCodes.PD_TM_PrintingCertificate)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.TrademarkCertificate }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_01));

            //Старое
            
            WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "3D5187DF-5ED3-4D32-BAAE-A3F5790D9F3B")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));

            WorkflowStage("Из этапа 'Передача патентообладателю' на этап 'Действующие ОД'",
                    "D4FA4D76-F993-4E0F-930F-27CF0DB86B92")
                .WhenCurrentStageCode(RouteStageCodes.OD01_5)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.TrademarkCertificate
                }))
                .And<DoesDocumentHaveOutgoingNumberByCodesRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_5 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_01));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Оформление на межд.регистрацию'", "7655D6F2-292D-461F-ACBC-AF36645FD475")
                .WhenCurrentStageCode(RouteStageCodes.OD05_01)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.OthersIncoming))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD08));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Продление срока действия ОД'", "D989BF74-FB12-4343-85DC-AB952E2AA60C")
                .WhenCurrentStageCode(RouteStageCodes.OD05_01)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForExtendTime))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_1));

            WorkflowStage("Из этапа 'Направлено уведомление формы УВО-4' на этап 'Продление срока действия ОД'", "D0927A41-1ADD-40B2-BCC1-0D4D9BDCDD9B")
                .WhenCurrentStageCode(RouteStageCodes.OD05_03)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForExtendTime))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_1));

            WorkflowStage("Из этапа 'Подготовка и отправка уведомление формы УВО-4' на этап 'Направлено уведомление формы УВО-4'", "E19C500F-2BBB-4785-A0E1-246300B33532")
                .WhenCurrentStageCode(RouteStageCodes.OD05_02)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_4 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_03));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Внесение изменений в ОД'",
                    "5C3BCA18-5407-45E8-8729-3D50C7F1329A")
                .WhenCurrentStageCode(RouteStageCodes.OD05_01)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForChanging))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.TmNmptProtectionDocChange,
                        DicTariff.Codes.TmNmptProtectionDocSameTypeChange
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_6));

            WorkflowStage("Из этапа 'Прекращение срока действия ОД' на этап 'Восстановление срока действия  ОД'", "46D61994-2583-49B3-9142-F64527FFAC8F")
                .WhenCurrentStageCode(RouteStageCodes.OD04_3)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForPdRestore))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_2));

            WorkflowStage("Из этапа 'Восстановление срока действия  ОД' на этап 'Действующие ОД'",
                    "1DD92126-76C4-4688-8BEE-A4FADAC2C35B")
                .WhenCurrentStageCode(RouteStageCodes.OD04_2)
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.ProtectionDocRestore
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_01));

            WorkflowStage("Из этапа 'Подготовка дубликата ОД ' на этап 'Контроль  начальника управления'",
                    "444D509C-47A5-4AB3-9AD2-31E095E93BF5")
                .WhenCurrentStageCode(RouteStageCodes.OD04_5)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes.TrademarkCertificateDuplicate))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));

            WorkflowStage("Из этапа 'Продление срока действия ОД' на этап 'Действующие ОД'",
                    "DC9320F3-7669-4914-9DC6-1B618C99684B")
                .WhenCurrentStageCode(RouteStageCodes.OD04_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.TrademarkExtensionAppendix
                }))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.NmptTzExtension
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_01));

            WorkflowStage("Из этапа 'Внесение изменений в ОД' на этап 'Действующие ОД'", "2CAFC3D8-A915-4341-991E-6B3097A4894D")
                .WhenCurrentStageCode(RouteStageCodes.OD04_6)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] {
                    DicDocumentTypeCodes.NotificationOfTrademarkChanging,
                    DicDocumentTypeCodes.TrademarkConcedingAttachment
                }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_01));

            WorkflowStage("Из этапа 'Оформление на межд.регистрацию' на этап 'Действующие ОД'", "DE28AFF6-CECB-4528-90D8-B839ACC267BE")
                .WhenCurrentStageCode(RouteStageCodes.OD08)
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[] { DicTariff.Codes.InternationalServiceListPreparation }))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.POL_1))
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.POL_2 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_01));

            WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "9B7339E5-C87A-4207-9ADA-5B2D0603ADB3")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.TrademarkCertificate,
                        DicDocumentTypeCodes.TrademarkCertificateDuplicate
                    },
                    RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));

            WorkflowStage("Из этапа 'Контроль  директора' на этап 'Действующие ОД'", "A917A5C4-46CC-4888-9AD6-0045211F6F0D")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.TrademarkCertificate,
                    DicDocumentTypeCodes.TrademarkCertificateDuplicate
                }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_01));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Срок действия ОД истек'", "2774CF1D-A93E-4118-8E66-3C5B3DD1E65F")
                .WhenCurrentStageCode(RouteStageCodes.OD05_01)
                .And<IsProtectionDocumentValidityExpiredRule>(r => r.Eval())
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD03_3));

            #endregion

            #region Свидетельства

            WorkflowStage("Из этапа 'Присвоение номера регистрации ТЗ' на этап 'Печать свидетельства ТЗ'", "6B10519D-B00F-4582-B40F-A8BA119AAE51")
                .WhenCurrentStageCode(RouteStageCodes.PD_TM_AssignmentRegistrationNumber)
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.PD_TM_PrintingCertificate));

            #endregion
        }
    }
}
