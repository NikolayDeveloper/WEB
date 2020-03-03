using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument;

namespace NetCoreWorkflow.WorkFlows.ProtectionDocuments
{
    public class UsefulModelProtectionDocumentWorkflow: BaseProtectionDocumentWorkflow
    {
        public UsefulModelProtectionDocumentWorkflow()
        {
           // InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "177D1634-C8CB-4114-B52E-F585A7BFED89")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "7D328F2A-6A09-48DB-B23C-6EB3A13B09E4")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Из этапа 'Присвоение номера охранного документа' паралельные этапы :['Печать охранного документа', 'Подготовка описаний для ОД', 'Поддержание ОД']",
                "957D1634-C8CB-4114-B32E-F585A7BFED12")
                .WhenCurrentStageCode(RouteStageCodes.OD01_1)
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.ODParallel));

            //WorkflowStage("Из этапа 'Подготовка описаний для ОД' на этап 'Подготовка печатного вида ОД'",
            //        "ED96E724-1939-407C-83BA-CDFBB3F92275")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_2_2)
            //    .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
            //        r.Eval(DicDocumentTypeCodes.UsefulModelFullDescription,
            //            DicDocumentTypeCodes.UsefulModelFullDescriptionKz))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_2_1));
            //
            //WorkflowStage("Из этапа 'Подготовка печатного вида ОД' на этап 'Печать охранного документа'",
            //        "5F85E740-D36E-4580-824C-85BE4516341F")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_2_1)
            //    .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.UsefulModelAuthorCertificate))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3));
            //
            //WorkflowStage("Из этапа 'Печать охранного документа' на этап 'Контроль  начальника управления'",
            //        "878D0CFF-2E8C-408D-B5BD-6C839B8B164B")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3)
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));
            //
            //WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "69A14444-3761-40E6-AA21-426875B6C075")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));
            //
            //WorkflowStage("Из этапа 'Контроль  директора' на этап 'Отправка  в МЮ'", "B76AD516-034A-4644-95A8-039DB98A1AF0")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
            //    .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.Reestr_006_014_3 }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_4));

            //WorkflowStage("Из этапа 'Возвращено с МЮ РК' на этап 'Передача патентообладателю'",
            //        "662A4EA0-51FD-47DE-A026-B4C4A099F867")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_5_1)
            //    .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicDocumentTypeCodes.UsefulModelPatent
            //    }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_5));

            //WorkflowStage("Из этапа 'Передача патентообладателю' на этап 'Действующие ОД'",
            //        "52A58CF5-0450-484D-8908-6AAC9FF90421")
            //    .WhenCurrentStageCode(RouteStageCodes.OD01_5)
            //    .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r => r.Eval(new[]
            //    {
            //        DicTariff.Codes.FirstToThirdSupportYearExpiredUsefulModel,
            //        DicTariff.Codes.FirstToThirdSupportYearUsefulModel,
            //    }))
            //    .And<DoesDocumentHaveOutgoingNumberByCodesRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_5 }))
            //    .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Продление срока действия ОД'", "559A4739-7FD1-4964-BA92-6A0C4592CCAE")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForExtendTime }))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_1));

            WorkflowStage("Из этапа 'Направлено уведомление формы УВО-4' на этап 'Прекращение срока действия ОД'", "80740148-C00A-41E6-B48E-984A6998A003")
                .WhenCurrentStageCode(RouteStageCodes.OD05_03)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PREKRPM }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_3));

            WorkflowStage("Из этапа 'Направлено уведомление формы УВО-4' на этап 'Действующие ОД'",
                    "D524C77A-45BC-43B6-9322-F46ADD63CAD3")
                .WhenCurrentStageCode(RouteStageCodes.OD05_03)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(DicTariff.Codes.GetProtectionDocSupportCodes().ToArray()))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Подготовка и отправка уведомление формы УВО-4' на этап 'Направлено уведомление формы УВО-4'", "C15FF4E5-3826-408F-869E-0E200F4FEE13")
                .WhenCurrentStageCode(RouteStageCodes.OD05_02)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.UVO_4 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_03));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Внесение изменений в ОД'",
                    "811FA724-F34D-4BC1-8CD5-2E34DDD808B2")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForChanging }))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1 }))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.UmImIzProtectionDocChange,
                        DicTariff.Codes.UmImIzProtectionDocSameTypeChange
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_6));

            WorkflowStage("Из этапа 'Прекращение срока действия ОД' на этап 'Восстановление срока действия  ОД'", "8CDA262C-2D13-4906-9F7C-305278DAD22B")
                .WhenCurrentStageCode(RouteStageCodes.OD04_3)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PetitionForPdRestore }))
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r =>
                    r.Eval(new[] { DicDocumentTypeCodes._001_002, DicDocumentTypeCodes.IN001_032, DicDocumentTypeCodes._001_002_1 }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_2));

            WorkflowStage("Из этапа 'Восстановление срока действия  ОД' на этап 'Действующие ОД'",
                    "AE66FF36-9DE6-42AD-82B1-77761E47AE22")
                .WhenCurrentStageCode(RouteStageCodes.OD04_2)
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.ProtectionDocRestore
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Подготовка дубликата ОД ' на этап 'Контроль  начальника управления'",
                    "D19746DF-DE35-488C-AAEA-4767A60696F7")
                .WhenCurrentStageCode(RouteStageCodes.OD04_5)
                .And<IsProtectionDocHasDocumentWithCodeCreatedOnCurrentStageRule>(r => r.Eval(DicDocumentTypeCodes.UsefulModelPatentDuplicate))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_0));

            //todo уведомление о продлении ПМ
            WorkflowStage("Из этапа 'Продление срока действия ОД' на этап 'Действующие ОД'",
                    "15B25BAC-7DA0-4854-923F-74FD1A959265")
                .WhenCurrentStageCode(RouteStageCodes.OD04_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.UsefulModelExtensionAppendix
                }))
                .And<IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule>(r =>
                    r.Eval(new[]
                    {
                        DicTariff.Codes.ImExtension, DicTariff.Codes.InvExtension, DicTariff.Codes.NmptTzExtension,
                        DicTariff.Codes.SaExtension
                    }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));
            
            WorkflowStage("Из этапа 'Внесение изменений в ОД' на этап 'Действующие ОД'", "BE3D5E33-1B50-47C6-90DE-AEF3D6CCDD76")
                .WhenCurrentStageCode(RouteStageCodes.OD04_6)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] {
                    DicDocumentTypeCodes.NotificationOfUsefulModelChanging,
                    DicDocumentTypeCodes.UsefulModelConcedingAttachment
                }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Контроль  начальника управления' на этап 'Контроль  директора'", "E3FB960A-4C49-4446-9DD7-F3B63ACDA0EE")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_0)
                .And<IsDocumentSignedAtStageRule>(r => r.Eval(new[]
                    {
                        DicDocumentTypeCodes.UsefulModelPatent,
                        DicDocumentTypeCodes.UsefulModelPatentDuplicate
                    },
                    RouteStageCodes.DocumentOutgoing_02_2))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD01_3_1));

            WorkflowStage("Из этапа 'Контроль  директора' на этап 'Действующие ОД'", "0B8D962C-D623-4F57-8C60-72BE718EE04F")
                .WhenCurrentStageCode(RouteStageCodes.OD01_3_1)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[]
                {
                    DicDocumentTypeCodes.UsefulModelPatent,
                    DicDocumentTypeCodes.UsefulModelPatentDuplicate
                }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05));

            WorkflowStage("Из этапа 'Передача патентообладателю' на этап 'Прекращение срока действия ОД'",
                    "3734CBA7-1424-47CB-BB80-78A058895048")
                .WhenCurrentStageCode(RouteStageCodes.OD01_5)
                .And<IsDocumentHasOutgoingNumberByCodesOnCurrentStageRule>(r => r.Eval(new[] { DicDocumentTypeCodes.PREKRPM }))
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD04_3));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Срок действия ОД истек'", "4C30C1FE-47FC-443C-86EF-064AA4DD7353")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocumentValidityExpiredRule>(r => r.Eval())
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD03_3));

            WorkflowStage("Из этапа 'Действующие ОД' на этап 'Подготовка и отправка уведомление формы УВО-4'", "D5E7459D-2877-4514-8C0F-47E67DCE2204")
                .WhenCurrentStageCode(RouteStageCodes.OD05)
                .And<IsProtectionDocumentSupportExpiredRule>(r => r.Eval())
                .Then(SendProtectionDocumentToNextStage(RouteStageCodes.OD05_02));
        }
    }
}
