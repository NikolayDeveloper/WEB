using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Domain.Constants
{
    public static class KeyFor
    {
        public static class Policy
        {
            public const string
                HasAccessToAdministration = nameof(HasAccessToAdministration),
                HasAccessToJournal = nameof(HasAccessToJournal),
                HasAccessToViewStaffTasks = nameof(HasAccessToViewStaffTasks);



        }

        public static class Role
        {
            public const string
                Admin = "Admin",
                Director = "Director",
                DeputyDirector = "Deputy Director",
                Supervisor = "Supervisor",
                IncomingMailEmployee = "Incoming mail employee",
                OutgoingMailEmployee = "Outgoing mail employee",
                Clerk = "Clerk",
                ExpertOnPayments = "Expert on payments",
                ExpertOfPreliminary_FormalExpertise = "Expert of preliminary / formal expertise",
                ExpertOfFull_InEssenceExpertise = "Expert of full / in essence expertise",
                ExpertOfTheStateRegister = "Expert of the State Register",
                ContractExpert = "Contract expert",
                BallotExpert = "Ballot expert";

        }

        public static class JwtClaimIdentifier
        {
            public const string
                Role = "rol",
                Id = "id",
                Permission = "prm",
                XIN = "xin";

        }

        public static class Permission
        {
            #region Common

            [Display(Name = "Доступ к модулю \"Отчеты\"")]
            public const string ReportsModule = "reports.module";

            [Display(Name = "Доступ к модулю \"Администрирование\"")]
            public const string AdministrationModule = "administration.module";

            [Display(Name = "Доступ к модулю \"Международные поисковые системы\"")]
            public const string InternationalSearchEnginesModule = "internationalSearchEngines.module";



            #endregion

            #region Actions 

            [Display(Name = "Генерация производственного номера")]
            public const string PoductNumberGeneration = "action.productNumberGeneration";

            [Display(Name = "Пометка на удаление документа или материала документа на своем этапе")]
            public const string MarkDocumentAsDeleted = "action.markDeleted";

            [Display(Name = "Назначение исполнителя")]
            public const string AssignExecutor = "action.assignExecutor";

            [Display(Name = "Редактирование Заявки/ОД")]
            public const string EditRequestOrProtectionDocument = "action.requestProtectionDoc.edit";

            [Display(Name = "Зачтение и списание оплат")]
            public const string PaymentAcctreptanceAndWriteOff = "action.payment.acceptanceAndWriteOff";

            [Display(Name = "Согласование документов (Подписание с ЭЦП)")]
            // ReSharper disable once InconsistentNaming
            public const string SigningWithDS = "action.signingWithDS";

            [Display(Name = "Несогласование документов (отправка на доработку)")]
            public const string SendingForRevision = "action.sendingForRevision";

            #endregion

            #region Journal 

            [Display(Name = "Доступ к модулю \"Дневник\"")]
            public const string JournalModule = "journal.module";

            [Display(Name = "Просмотр дневника (Задачи сотрудников)")]
            public const string JournalViewStaffTasks = "journal.staffTasks.view";

            [Display(Name = "Автораспределение")]
            public const string JournalViewAutoAllocation = "journal.autoAllocation.view";

            #endregion

            #region Correspondence 

            [Display(Name = "Доступ к модулю \"Материалы\" (корреспонденция)")]
            public const string MaterialsModule = "correspondence.module";

            [Display(Name = "Обработка входящей корреспонденции")]
            public const string ProcessingIncomingCorrespondence = "correspondence.incoming.processing";

            [Display(Name = "Обработка исходящей корреспонденции")]
            public const string ProcessingOutgoingCorrespondence = "correspondence.outgoing.processing";

            [Display(Name = "Обработка внутренней корреспонденции")]
            public const string ProcessingInternalCorrespondence = "correspondence.internal.processing";

            [Display(Name = "Создание входящей корреспонденции")]
            public const string CreatingIncomingCorrespondence = "correspondence.incoming.create";

            [Display(Name = "Создание исходящей корреспонденции")]
            public const string CreatingOutgoingCorrespondence = "correspondence.outgoing.create";

            [Display(Name = "Создание внутренней корреспонденции")]
            public const string CreatingInternalCorrespondence = "correspondence.internal.create";

            #endregion

            #region Search

            [Display(Name = "Доступ к модулю \"Поиск\"")]
            public const string SearchModule = "search.module";

            [Display(Name = "Простой поиск")]
            public const string SimpleSearch = "search.simple";

            [Display(Name = "Расширенный поиск")]
            public const string AdvancedSearch = "search.advanced";

            [Display(Name = "Экспертный поиск")]
            public const string ExpertSearch = "search.expert";

            #endregion
        }
    }
}
