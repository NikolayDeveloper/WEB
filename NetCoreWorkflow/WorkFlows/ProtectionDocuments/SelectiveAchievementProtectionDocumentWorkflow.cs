using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument;

namespace NetCoreWorkflow.WorkFlows.ProtectionDocuments
{
    public class SelectiveAchievementProtectionDocumentWorkflow: BaseProtectionDocumentWorkflow
    {
        public SelectiveAchievementProtectionDocumentWorkflow()
        {
            //InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "04EDB175-8DE9-438C-8950-4901328A457A")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "7C31F29A-3BBB-4A81-85C9-1DD758D11DFD")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Из этапа 'Присвоение номера охранного документа' паралельные этапы :['Печать охранного документа', 'Подготовка описаний для ОД', 'Поддержание ОД']",
                "957D1634-C8CB-4114-B32E-F585A7BFED12")
                .WhenCurrentStageCode(RouteStageCodes.OD01_1)
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.ODParallel));

            //WorkflowStage("Из этапа 'Подготовка описаний для ОД' на этап 'Подготовка печатного вида ОД'",
            //        "F3BF05AD-C2AD-4BC0-9F5A-7EEE746E2C34")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_2_2)
            //    .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
            //        r.Eval(DicDocumentTypeCodes.SelectiveAchievementFullDescription,
            //            DicDocumentTypeCodes.SelectiveAchievementFullDescriptionKz))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_2_1));
            //
            //WorkflowStage("Из этапа 'Подготовка печатного вида ОД' на этап 'Печать охранного документа'",
            //        "1B5F70C7-AA94-422E-862A-3CAB5F34D2DC")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_2_1)
            //    .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
            //        r.Eval(DicDocumentTypeCodes.AgriculturalSelectiveAchievementAuthorCertificate,
            //            DicDocumentTypeCodes.AnimalHusbandrySelectiveAchievementAuthorCertificate))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3));
            //
            //
            //WorkflowStage("Из этапа 'Печать охранного документа' на этап 'Контроль  начальника управления'",
            //        "E5F02A8F-347B-4844-9B84-5BD655C9B50F")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3)
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));
            //
            //WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "4FB4F920-5726-467E-A05F-F4A99FB68BF1")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));
            //
            //WorkflowStage("Из этапа 'Контроль  директора' на этап 'Отправка  в МЮ'", "419075C5-6290-471A-9E08-3353B91ED861")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
            //    .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Reestr_006_014_3 }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_4));

            //WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Передача патентообладателю'",
            //        "5228E909-F8C3-4CE6-9EEA-3A948CDEC9DC")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_5_1)
            //    .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.SelectiveAchievementsAgriculturalPatent,
            //        DicDocumentTypeCodes.SelectiveAchievementsAnimalHusbandryPatent
            //    }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_5));

            //WorkflowStage("Из этапа 'Передача патентообладателю' на этап 'Действующие ОД'",
            //        "C2D97FE7-1CF3-4A14-9D73-383EA9A3101A")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_5)
            //    .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicTariff.Codes.FirstToThirdSupportYearExpiredSelectiveAchievement,
            //        DicTariff.Codes.FirstToThirdSupportYearSelectiveAchievement
            //    }))
            //    .And<DoesDocumentHaveOutgoingNumberByCodesRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_5 }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Продление срока действия ОД'", "E3E77129-AD25-4CED-9710-5EF0D2093BBF")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForExtendTime))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_1));

            WorkflowStage("Из этапа 'Направлено уведомление формы УВО-4' на этап 'Прекращение срока действия ОД'", "073F8181-DC72-49BC-B9BE-4DE4BB33381A")
                .WhenCurrentStageCode(RouteStageCodes.OD05_03)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PREKRPAT_SA }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_3));

            WorkflowStage("Из этапа 'Направлено уведомление формы УВО-4' на этап 'Действующие ОД'",
                    "97DD5002-A9CA-46E4-BF54-9A5976F328CF")
                .WhenCurrentStageCode(RouteStageCodes.OD05_03)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(DicTariff.Codes.GetProtectionDocSupportCodes().ToArray()))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Подготовка и отправка уведомление формы УВО-4' на этап 'Направлено уведомление формы УВО-4'", "14A2A37F-721F-40CC-BFFF-7681D7F2697D")
                .WhenCurrentStageCode(RouteStageCodes.OD05_02)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_4 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_03));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Внесение изменений в ОД'",
                    "E32E3F03-4AFD-4046-A966-C5E537D0A2CF")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForChanging))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.SelectiveAchevementsProtectionDocChange
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_6));

            WorkflowStage("Из этапа 'Прекращение срока действия ОД' на этап 'Восстановление срока действия  ОД'", "7E4074B4-4A84-424C-93D0-49051B8D2FC9")
                .WhenCurrentStageCode(RouteStageCodes.OD04_3)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForPdRestore))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_2));

            WorkflowStage("Из этапа 'Восстановление срока действия  ОД' на этап 'Действующие ОД'",
                    "8B1FA681-BC38-49FF-9D15-96A0D9385C75")
                .WhenCurrentStageCode(RouteStageCodes.OD04_2)
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.ProtectionDocRestoreSelectiveAchievement
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Подготовка дубликата ОД ' на этап 'Контроль  начальника управления'",
                    "10AA2F71-427A-44E4-8813-280D2AE903DB")
                .WhenCurrentStageCode(RouteStageCodes.OD04_5)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes.SelectiveAchievementsAgriculturalPatentDuplicate,
                        DicDocumentTypeCodes.SelectiveAchievementsAnimalHusbandryPatent))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));

            //todo уведомление о продлении СД
            WorkflowStage("Из этапа 'Продление срока действия ОД' на этап 'Действующие ОД'",
                    "3009DFE1-0346-484B-A34F-DE522BD2423D")
                .WhenCurrentStageCode(RouteStageCodes.OD04_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.NotificationOfIndustrialExtension,
                    DicDocumentTypeCodes.NotificationOfInnovationExtension,
                    DicDocumentTypeCodes.NotificationOfInternationalTrademarkExtension,
                    DicDocumentTypeCodes.NotificationOfInventionExtension,
                    DicDocumentTypeCodes.NotificationOfTrademarkExtension,
                    DicDocumentTypeCodes.NotificationOfUsefulModelExtension
                }))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.SaExtension
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            //todo уведомление о внесении изменений в СД
            WorkflowStage("Из этапа 'Внесение изменений в ОД' на этап 'Действующие ОД'", "45AFB91E-FA69-41FE-8291-8B679D98298C")
                .WhenCurrentStageCode(RouteStageCodes.OD04_6)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] {
                    DicDocumentTypeCodes.NotificationOfIndustrialChanging,
                    DicDocumentTypeCodes.NotificationOfInnovationChanging,
                    DicDocumentTypeCodes.NotificationOfInventionChanging,
                    DicDocumentTypeCodes.NotificationOfTrademarkChanging,
                    DicDocumentTypeCodes.NotificationOfUsefulModelChanging,
                    DicDocumentTypeCodes.IndustrialConcedingAttachment,
                    DicDocumentTypeCodes.InventionConcedingAttachment,
                    DicDocumentTypeCodes.TrademarkConcedingAttachment,
                    DicDocumentTypeCodes.UsefulModelConcedingAttachment
                }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "46729843-8F6A-41E9-8406-70EF66FF9643")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.SelectiveAchievementsAgriculturalPatent,
                        DicDocumentTypeCodes.SelectiveAchievementsAgriculturalPatentDuplicate,
                        DicDocumentTypeCodes.SelectiveAchievementsAnimalHusbandryPatent
                    },
                    RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));

            WorkflowStage("Из этапа 'Контроль  директора' на этап 'Действующие ОД'",
                    "05FFE7ED-7F5B-4DE1-BFA9-0515865D41C2")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.SelectiveAchievementsAgriculturalPatent,
                    DicDocumentTypeCodes.SelectiveAchievementsAgriculturalPatentDuplicate,
                    DicDocumentTypeCodes.SelectiveAchievementsAnimalHusbandryPatent
                }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Передача патентообладателю' на этап 'Прекращение срока действия ОД'",
                    "FE7FD369-9D6C-4704-85A9-49C9937B59A5")
                .WhenCurrentStageCode(RouteStageCodes.OD01_5)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PREKRPAT_SA }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_3));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Срок действия ОД истек'", "2D96AD7E-2C8A-4417-9A0C-66CDA0A7AE60")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocumentValidityExpiredRule>(r => r.Eval())
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD03_3));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Подготовка и отправка уведомление формы УВО-4'", "EF67FA8C-8561-432D-8E6E-927306D922FE")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocumentSupportExpiredRule>(r => r.Eval())
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_02));
        }
    }
}
