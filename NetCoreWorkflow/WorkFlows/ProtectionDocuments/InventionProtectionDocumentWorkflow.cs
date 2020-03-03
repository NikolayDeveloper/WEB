using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument;

namespace NetCoreWorkflow.WorkFlows.ProtectionDocuments
{
    public class InventionProtectionDocumentWorkflow: BaseProtectionDocumentWorkflow
    {
        public InventionProtectionDocumentWorkflow()
        {
            //InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "09ED15DC-140D-4F7C-A935-E56C598DF70D")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "58ACBC87-F89D-4450-8BA5-30993375579C")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Из этапа 'Присвоение номера охранного документа' паралельные этапы :['Печать охранного документа', 'Подготовка описаний для ОД', 'Поддержание ОД']",
                "957D1634-C8CB-4114-B32E-F585A7BFED12")
                .WhenCurrentStageCode(RouteStageCodes.OD01_1)
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.ODParallel));

            //WorkflowStage("Из этапа 'Подготовка описаний для ОД' на этап 'Подготовка печатного вида ОД'",
            //        "79D26D82-3B0A-453D-97FB-54325C60D932")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_2_2)
            //    .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
            //        r.Eval(DicDocumentTypeCodes.InventionFullDescription,
            //            DicDocumentTypeCodes.InventionFullDescriptionKz))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_2_1));
            //
            //WorkflowStage("Из этапа 'Подготовка печатного вида ОД' на этап 'Печать охранного документа'",
            //        "DBCF157B-340E-4421-97FE-4FA2A8E6F486")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_2_1)
            //    .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.InventionAuthorCertificate))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3));
            //
            //WorkflowStage("Из этапа 'Печать охранного документа' на этап 'Контроль  начальника управления'",
            //        "853D5F50-843B-49FF-9C95-C1477B5B2724")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3)
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));
            //
            //WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "6499B7C6-E53C-4A7D-9722-03D32F112CC7")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));
            //
            //WorkflowStage("Из этапа 'Контроль  директора' на этап 'Отправка  в МЮ'", "FD634434-6FAD-47CD-B095-34D19EB2F284")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
            //    .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Reestr_006_014_3 }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_4));

            //WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Передача патентообладателю'",
            //        "9921AE8C-6239-4F44-AFD6-AA875251F981")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_5_1)
            //    .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.InventionPatent
            //    }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_5));

            //WorkflowStage("Из этапа 'Передача патентообладателю' на этап 'Действующие ОД'",
            //        "918B1728-1EEC-4CCA-9708-B17CB878A4EC")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_5)
            //    .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicTariff.Codes.FirstToThirdSupportYearInvention,
            //        DicTariff.Codes.FirstToThirdSupportYearExpiredInvention
            //    }))
            //    .And<DoesDocumentHaveOutgoingNumberByCodesRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_5 }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Продление срока действия ОД'", "1541316B-EC24-493A-A0A2-8857C0670A69")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForExtendTime))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032 , DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_1));

            WorkflowStage("Из этапа 'Направлено уведомление формы УВО-4' на этап 'Прекращение срока действия ОД'", "E7FA3FB9-825A-4C30-9397-68B9572C952A")
                .WhenCurrentStageCode(RouteStageCodes.OD05_03)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PREKRPAT }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_3));

            WorkflowStage("Из этапа 'Направлено уведомление формы УВО-4' на этап 'Действующие ОД'",
                    "38A481FF-F49B-4DFB-B796-7881DF3FCE85")
                .WhenCurrentStageCode(RouteStageCodes.OD05_03)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032 , DicDocumentTypeCodes._001_002_1))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(DicTariff.Codes.GetProtectionDocSupportCodes().ToArray()))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Подготовка и отправка уведомление формы УВО-4' на этап 'Направлено уведомление формы УВО-4'", "54D57C80-E292-4136-9F31-3D26D20BE6AE")
                .WhenCurrentStageCode(RouteStageCodes.OD05_02)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_4 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_03));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Внесение изменений в ОД'",
                    "9170BF69-0E84-42A9-BC02-75AD83E61769")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForChanging))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032 , DicDocumentTypeCodes._001_002_1))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.UmImIzProtectionDocChange,
                        DicTariff.Codes.UmImIzProtectionDocSameTypeChange
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_6));

            WorkflowStage("Из этапа 'Прекращение срока действия ОД' на этап 'Восстановление срока действия  ОД'", "8E0E142C-4AF2-40DF-A2B7-327F692C6B6B")
                .WhenCurrentStageCode(RouteStageCodes.OD04_3)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForPdRestore))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032 , DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_2));

            WorkflowStage("Из этапа 'Восстановление срока действия  ОД' на этап 'Действующие ОД'",
                    "CBD2C4B4-BCC6-4B2C-AEAD-3B8FD565B312")
                .WhenCurrentStageCode(RouteStageCodes.OD04_2)
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.ProtectionDocRestore
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Подготовка дубликата ОД ' на этап 'Контроль  начальника управления'",
                    "7F45482A-786A-40C0-B69C-C01CC70B4EDA")
                .WhenCurrentStageCode(RouteStageCodes.OD04_5)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes.InventionPatentDuplicate))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));

            WorkflowStage("Из этапа 'Продление срока действия ОД' на этап 'Действующие ОД'",
                    "AAC8D142-8120-406C-BC1E-D9A173637F36")
                .WhenCurrentStageCode(RouteStageCodes.OD04_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.InventionExtensionAppendix
                }))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.InvExtension
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Внесение изменений в ОД' на этап 'Действующие ОД'", "F1EF3EED-0B84-4A1F-BCCD-A735ECC87E02")
                .WhenCurrentStageCode(RouteStageCodes.OD04_6)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] {
                    DicDocumentTypeCodes.NotificationOfInventionChanging,
                    DicDocumentTypeCodes.InventionConcedingAttachment
                }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "6BB9BB6F-8234-4B4E-9BBE-570B05F36751")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.InventionPatent,
                        DicDocumentTypeCodes.InventionPatentDuplicate
                    },
                    RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));

            WorkflowStage("Из этапа 'Контроль  директора' на этап 'Действующие ОД'", "F15D30AE-743A-4EB0-9CB6-B878BDD7B63E")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.InventionPatent,
                    DicDocumentTypeCodes.InventionPatentDuplicate
                }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Передача патентообладателю' на этап 'Прекращение срока действия ОД'",
                    "C96A03BB-3E0F-4C66-B945-ABBDB61FFBCD")
                .WhenCurrentStageCode(RouteStageCodes.OD01_5)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PREKRPAT }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_3));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Срок действия ОД истек'", "16B14B53-013C-4E94-99EE-10CA68ADD865")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocumentValidityExpiredRule>(r => r.Eval())
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD03_3));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Подготовка и отправка уведомление формы УВО-4'", "26CCDE0F-3696-4EC4-B115-3C0581F04A0C")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocumentSupportExpiredRule>(r => r.Eval())
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_02));
        }
    }
}
