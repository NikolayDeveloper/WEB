using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument;

namespace NetCoreWorkflow.WorkFlows.ProtectionDocuments
{
    public class IndustrialDesignProtectionDocumentWorkflow: BaseProtectionDocumentWorkflow
    {
        public IndustrialDesignProtectionDocumentWorkflow()
        {
           // InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "409CA096-7C13-4FCD-84A0-B20A4B560161")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "29794AF9-21FC-4208-8206-818988928673")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Из этапа 'Присвоение номера охранного документа' паралельные этапы :['Печать охранного документа', 'Подготовка описаний для ОД', 'Поддержание ОД']",
                "957D1634-C8CB-4114-B32E-F585A7BFED12")
                .WhenCurrentStageCode(RouteStageCodes.OD01_1)
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.ODParallel));

            //todo описание и удостоверение автора ПО
            //WorkflowStage("Из этапа 'Подготовка описаний для ОД' на этап 'Подготовка печатного вида ОД'",
            //        "01E139B6-3D42-4133-B6B6-3B1F32582950")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_2_2)
            //    .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
            //        r.Eval(DicDocumentTypeCodes.IndustrialDesignFullDescription,
            //            DicDocumentTypeCodes.IndustrialDesignFullDescriptionKz))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_2_1));
            //
            //WorkflowStage("Из этапа 'Подготовка печатного вида ОД' на этап 'Печать охранного документа'",
            //        "4B70AD6B-C68A-4CBD-9FDF-37DC71D6B971")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_2_1)
            //    .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.IndustrialDesignAuthorCertificate))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3));
            //
            //WorkflowStage("Из этапа 'Печать охранного документа' на этап 'Контроль  начальника управления'",
            //        "C5C90F94-DC16-43C8-B55B-DC0CC1CD830E")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3)
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));
            //
            //WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "D3DEE721-B4A1-40E7-A862-4741018F6038")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));
            //
            //WorkflowStage("Из этапа 'Контроль  директора' на этап 'Отправка  в МЮ'", "46D4574E-4DA3-446E-B93A-6E848CDC9944")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
            //    .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Reestr_006_014_3 }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_4));

            //WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Передача патентообладателю'",
            //        "B40C308D-DC65-40C6-9DC9-E0561DCC53AB")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_5_1)
            //    .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.IndustrialDesignsPatent
            //    }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_5));

            //WorkflowStage("Из этапа 'Передача патентообладателю' на этап 'Действующие ОД'",
            //        "FEC3E71C-250B-48C7-8DD9-1C27FDC8C8CD")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_5)
            //    .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicTariff.Codes.FirstToThirdSupportYearExpiredIndustrialDesign,
            //        DicTariff.Codes.FirstToThirdSupportYearIndustrialDesign
            //    }))
            //    .And<DoesDocumentHaveOutgoingNumberByCodesRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_5 }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Продление срока действия ОД'", "98CB4195-BBD0-4E93-BFDE-BEA037F0F8BE")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForExtendTime))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes._001_002_1,DicDocumentTypeCodes.IN001_032))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_1));

            WorkflowStage("Из этапа 'Направлено уведомление формы УВО-4' на этап 'Прекращение срока действия ОД'", "661C7D41-AF2D-4DB0-B29F-F3D3CBF28685")
                .WhenCurrentStageCode(RouteStageCodes.OD05_03)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new []{DicDocumentTypeCodes.PREKR_PO}))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_3));

            WorkflowStage("Из этапа 'Направлено уведомление формы УВО-4' на этап 'Действующие ОД'",
                    "C222EB55-296C-4E60-B748-596C018A9CC4")
                .WhenCurrentStageCode(RouteStageCodes.OD05_03)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes._001_002_1, DicDocumentTypeCodes.IN001_032))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(DicTariff.Codes.GetProtectionDocSupportCodes().ToArray()))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Подготовка и отправка уведомление формы УВО-4' на этап 'Направлено уведомление формы УВО-4'", "9E64F5CE-B4B7-4ECF-9ADB-6B742B6A1E9D")
                .WhenCurrentStageCode(RouteStageCodes.OD05_02)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_4 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_03));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Внесение изменений в ОД'",
                    "FE735817-145B-493A-B782-F2C6EAFD2A15")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForChanging))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes._001_002_1, DicDocumentTypeCodes.IN001_032))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.UmImIzProtectionDocChange,
                        DicTariff.Codes.UmImIzProtectionDocSameTypeChange
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_6));

            WorkflowStage("Из этапа 'Прекращение срока действия ОД' на этап 'Восстановление срока действия  ОД'", "21E2EEC6-22AA-44DF-B9C6-CD6260A3C122")
                .WhenCurrentStageCode(RouteStageCodes.OD04_3)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForPdRestore))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes._001_002_1,DicDocumentTypeCodes.IN001_032))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_2));

            WorkflowStage("Из этапа 'Восстановление срока действия  ОД' на этап 'Действующие ОД'",
                    "91AF5C03-55FA-4259-B986-E7D54764AAB0")
                .WhenCurrentStageCode(RouteStageCodes.OD04_2)
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.ProtectionDocRestore
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Подготовка дубликата ОД '", "1BF67856-21FD-422F-B7A1-BE2DAA1C2A6D")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForCopy))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_5));
            
            WorkflowStage("Из этапа 'Подготовка дубликата ОД ' на этап 'Контроль  начальника управления'",
                    "43E63B29-5C3B-49A2-97F2-91ED48CCFB9A")
                .WhenCurrentStageCode(RouteStageCodes.OD04_5)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes.IndustrialDesignsPatentDuplicate))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));

            WorkflowStage("Из этапа 'Продление срока действия ОД' на этап 'Действующие ОД'",
                    "CFA55DEB-797A-4017-B9F4-8FDB4BA6E0EC")
                .WhenCurrentStageCode(RouteStageCodes.OD04_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.IndustrialDesignExtensionAppendix
                }))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.ImExtension
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Внесение изменений в ОД' на этап 'Действующие ОД'", "F39B5465-F0AD-447D-B1DC-1A7B866BF9BC")
                .WhenCurrentStageCode(RouteStageCodes.OD04_6)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] {
                    DicDocumentTypeCodes.NotificationOfIndustrialChanging,
                    DicDocumentTypeCodes.IndustrialConcedingAttachment
                }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "C0DBF1E7-BADF-4A9A-B1E5-4ABF8699212A")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.IndustrialDesignsPatent,
                        DicDocumentTypeCodes.IndustrialDesignsPatentDuplicate
                    },
                    RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));

            WorkflowStage("Из этапа 'Контроль  директора' на этап 'Действующие ОД'", "8047E1EE-C2A5-4A92-8F61-A5205B9BF4DE")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.IndustrialDesignsPatent,
                    DicDocumentTypeCodes.IndustrialDesignsPatentDuplicate
                }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Передача патентообладателю' на этап 'Прекращение срока действия ОД'",
                    "2F072B73-A318-46EF-B2A8-3DBCC5310CA2")
                .WhenCurrentStageCode(RouteStageCodes.OD01_5)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new []{DicDocumentTypeCodes.PREKR_PO}))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_3));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Срок действия ОД истек'", "7A416B41-34DA-403D-A623-BF145698EE35")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocumentValidityExpiredRule>(r => r.Eval())
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD03_3));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Подготовка и отправка уведомление формы УВО-4'", "E7D67F2D-F259-4334-AE70-0ECA72626894")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocumentSupportExpiredRule>(r => r.Eval())
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_02));
        }
    }
}
