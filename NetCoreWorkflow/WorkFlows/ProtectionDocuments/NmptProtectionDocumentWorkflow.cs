using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument;

namespace NetCoreWorkflow.WorkFlows.ProtectionDocuments
{
    public class NmptProtectionDocumentWorkflow: BaseProtectionDocumentWorkflow
    {
        public NmptProtectionDocumentWorkflow()
        {
            //InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "2421CAD8-4445-4B49-98C4-C57C3F28CB83")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "D70147CF-4362-4387-BEF5-AC25CBEC237E")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Из этапа 'Печать охранного документа' на этап 'Действующие ТЗ'",
                    "DBE043D3-D91C-4B1B-A32D-BE6B1E1AD500")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.NmptCertificate }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_01));

            //Старое
            //WorkflowStage("Из этапа 'Контроль  директора' на этап 'Отправка  в МЮ'", "D5F29390-FB9B-43F1-9A83-B7EC493D4333")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
            //    .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Reestr_006_014_3 }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_4));

            //WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Передача патентообладателю'",
            //        "843E0C57-8E51-4253-9B46-FACEFF5B515F")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_5_1)
            //    .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.NmptCertificate },
            //        RouteStageCodes.DocumentOutgoing_02_3))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_5));
            

            WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "FAC80BA8-2757-41E5-9FEB-476948A88B0A")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));
            
            WorkflowStage("Из этапа 'Печать охранного документа' на этап 'Контроль  начальника управления'",
                    "16133A3D-B857-4928-A14E-A218472E2436")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3)
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));

            WorkflowStage("Из этапа 'Передача патентообладателю' на этап 'Действующие ОД'",
                    "DCE2998D-F87B-4705-B61A-5D52611E6869")
                .WhenCurrentStageCode(RouteStageCodes.OD01_5)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.NmptCertificate
                }))
                .And<DoesDocumentHaveOutgoingNumberByCodesRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_5 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Продление срока действия ОД'", "EF03DA10-844D-456A-B495-ABAB09504ADF")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForExtendTime))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_1));

            WorkflowStage("Из этапа 'Направлено уведомление формы УВО-4' на этап 'Продление срока действия ОД'", "7F973B59-F251-4223-A421-2CF40B5F1AE6")
                .WhenCurrentStageCode(RouteStageCodes.OD05_03)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForExtendTime))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_1));

            WorkflowStage("Из этапа 'Подготовка и отправка уведомление формы УВО-4' на этап 'Направлено уведомление формы УВО-4'", "F25C20DC-7D93-4F66-A3C5-F5487C9CAA93")
                .WhenCurrentStageCode(RouteStageCodes.OD05_02)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_4 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_03));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Внесение изменений в ОД'",
                    "6C374F40-8F21-4012-8FF4-F7B9AA0A68FF")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
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

            WorkflowStage("Из этапа 'Прекращение срока действия ОД' на этап 'Восстановление срока действия  ОД'", "DA97AD48-FC4A-4698-8C3F-0EA40D725F49")
                .WhenCurrentStageCode(RouteStageCodes.OD04_3)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.PetitionForPdRestore))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_2));

            WorkflowStage("Из этапа 'Восстановление срока действия  ОД' на этап 'Действующие ОД'",
                    "745F62BB-F0D6-41EF-B4A7-56E84FC07299")
                .WhenCurrentStageCode(RouteStageCodes.OD04_2)
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.ProtectionDocRestore
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Подготовка дубликата ОД ' на этап 'Контроль  начальника управления'",
                    "00874AC9-5F1E-4A64-9287-7C3C711F6527")
                .WhenCurrentStageCode(RouteStageCodes.OD04_5)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes.NmptCertificate))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));
            
            WorkflowStage("Из этапа 'Продление срока действия ОД' на этап 'Действующие ОД'",
                    "14D03665-D1EE-4051-B82D-2234519CE887")
                .WhenCurrentStageCode(RouteStageCodes.OD04_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.NmptExtensionAppendix
                }))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.NmptTzExtension
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            //todo уведомление о внесении изменений в нмпт
            WorkflowStage("Из этапа 'Внесение изменений в ОД' на этап 'Действующие ОД'", "1B82EC2A-464B-450E-87DA-5AA5F1CF1B0F")
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

            WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "1605C27B-4189-4489-83A7-45484EBCBD90")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.NmptCertificate },
                    RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));

            WorkflowStage("Из этапа 'Контроль  директора' на этап 'Действующие ОД'", "1362EBB7-03A6-4EA1-A6D2-18BDBDA2412E")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.NmptCertificate }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Срок действия ОД истек'", "F72628FE-3302-4CD8-B4B5-EF01530747BE")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocumentValidityExpiredRule>(r => r.Eval())
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD03_3));
        }
    }
}
