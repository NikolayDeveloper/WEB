using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.Domain.Enums;
using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Этапы маршрутов
    /// </summary>
    public class DicRouteStage : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public DicRouteStage()
        {
            StageExpirationByDocTypes = new HashSet<DicStageExpirationByDocType>();
            ChangeTypes = new HashSet<DicBiblioChangeTypeDicRouteStageRelation>();
        }

        public bool IsSystem { get; set; }
        public int? Interval { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }
        public bool IsMultiUser { get; set; }
        public bool? IsReturnable { get; set; }

        /// <summary>
        /// Пожписывающий этап
        /// </summary>
        public bool? IsSign { get; set; }

        /// <summary>
        /// Признак автоматического этапа, который создается только автоматически, недоступен для ручного выбора
        /// </summary>
        public bool IsAuto { get; set; }

        /// <summary>
        /// Признак этапа из основного сценария
        /// </summary>
        public bool IsMain { get; set; }

        public ExpirationType ExpirationType { get; set; }
        public short? ExpirationValue { get; set; }

        /// <summary>
        /// Срок исполнения по типа документа
        /// </summary>
        public ICollection<DicStageExpirationByDocType> StageExpirationByDocTypes { get; set; }
        /// <summary>
        /// Режимы внесения изменений
        /// </summary>
        public ICollection<DicBiblioChangeTypeDicRouteStageRelation> ChangeTypes { get; set; }

        #region Relationships

        public int? OnlineRequisitionStatusId { get; set; }
        public DicOnlineRequisitionStatus OnlineRequisitionStatus { get; set; }
        public int? RouteId { get; set; }
        public DicRoute Route { get; set; }
        public int? StartConServiceStatusId { get; set; }
        public IntegrationConServiceStatus StartConServiceStatus { get; set; }
        public IntegrationConServiceStatus FinishConServiceStatus { get; set; }
        public int? RequestStatusId { get; set; }
        public DicRequestStatus RequestStatus { get; set; }
        public int? ProtectionDocStatusId { get; set; }
        public DicProtectionDocStatus ProtectionDocStatus { get; set; }
        public int? ContractStatusId { get; set; }
        public DicContractStatus ContractStatus { get; set; }
        public DicDocumentStatus DicDocumentStatus { get; set; }

        #endregion

        #region Public codes
        
        public static class Codes
        {
            /// <summary>
            /// Предварительная экспертиза (ТЗ)
            /// </summary>
            public const string TmPreExamination = "TM03.2.2";

            /// <summary>
            /// Предварительная экспертиза (СА) SA03.2.0
            /// </summary>
            public const string SaPreExamination = "SA03.2.0";

            /// <summary>
            /// Формальная экспертиза изобретения
            /// </summary>
            public const string BFormalExamination = "B03.2.1";

            /// <summary>
            /// Формальная экспертиза (ПО)
            /// </summary>
            public const string PoFormalExamination = "PO03.2";

            /// <summary>
            /// Отправка
            /// </summary>
            public const string SentStage = "OUT03.1";

            /// <summary>
            /// Формирование данных заявки (НМПТ)
            /// </summary>
            public const string TmiRequestShaping = "TMI02.1";

            /// <summary>
            /// Формирование данных заявки (ТЗ)
            /// </summary>
            public const string TmRequestShaping = "TM02.1";

            /// <summary>
            /// Формирование данных заявки (СД)
            /// </summary>
            public const string SaRequestShaping = "SA02.1";

            /// <summary>
            /// Формирование данных заявки (ПО)
            /// </summary>
            public const string PoRequestShaping = "PO02.1";

            /// <summary>
            /// Формирование данных заявки (ИЗ)
            /// </summary>
            public const string BRequestShaping = "B02.1";

            /// <summary>
            /// Формирование данных заявки (МТЗ)
            /// </summary>
            public const string NmptRequestShaping = "NMPT02.1";

            /// <summary>
            /// Формирование данных заявления (ДК)
            /// </summary>
            public const string DkStatementShaping = "DK02.1";

            /// <summary>
            /// Создание Заявки (ИЗ)
            /// </summary>
            public const string URequestShaping = "U02.1";

            /// <summary>
            /// Создание Заявки (АП)
            /// </summary>
            public const string ApRequestShaping = "AP01.1";

            /// <summary>
            /// Ожидает оплату за подачу (ИЗ)
            /// </summary>
            public const string InventionsWaitingPayment = "B02.2.0.0";

            /// <summary>
            /// Ожидает оплату за подачу ПМ
            /// </summary>
            public const string UsefulModelsWaitingPayment = "U02.2.7";

            /// <summary>
            /// Ожидает оплату за подачу ПМ
            /// </summary>
            public const string IndustrialDesignsWaitingPayment = "PO02.2.0";

            /// <summary>
            /// Ожидает оплату за подачу ПМ
            /// </summary>
            public const string SelectiveAchievsWaitingPayment = "SA02.2.1";

            /// <summary>
            /// Ожидает оплату за экспертизу по существу (ИЗ)
            /// </summary>
            public const string  BWaitingExamination = "B02.2.0";

            /// <summary>
            /// Ожидает оплату за экспертизу по существу (ПО)
            /// </summary>
            public const string PoWaitingExamination = "PO03.2.02";

            /// <summary>
            /// Ожидает оплату за экспертизу по существу (ТЗ)
            /// </summary>
            public const string TmWaitingExamination = "TM03.2.2.1";

            /// <summary>
            /// Полная экспертиза (ТЗ)
            /// </summary>
            public const string TmFullExamination = "TM03.3.2";

            /// <summary>
            /// Полная экспертиза МТЗ
            /// </summary>
            public const string TmiFullExamination = "TMI03.3.2";

            /// <summary>
            /// Подготовка для передачи в Госреестр (ПО)
            /// </summary>
            public const string IndustrialDesignsGosRegTransfer = "PO03.8";

            /// <summary>
            /// Подготовка для передачи в Госреестр (СД)
            /// </summary>
            public const string SelectiveAchievsGosRegTransfer = "SA03.4";

            /// <summary>
            /// Подготовка для передачи в Госреестр (ПМ)
            /// </summary>
            public const string UsefulGosRegTransfer = "U03.7.1";

            /// <summary>
            /// Подготовка для передачи в Госреестр (ТЗ)
            /// </summary>
            public const string TrademarkGosRegTransfer = "TM03.3.7";

            /// <summary>
            /// Подготовка для передачи в Госреестр (ИЗ)
            /// </summary>
            public const string InventionsGosRegTransfer = "B03.3.7.1";

            /// <summary>
            /// Ожидание восстановления пропущенного срока (ТМ)
            /// </summary>
            public const string TmWaitingRestorationMissedDeadline = "TM03.3.7.0";

            /// <summary>
            /// Восстановление срока (НМПТ)
            /// </summary>
            public const string NmptWaitingRestorationMissedDeadline = "NMPT03.2.3";

            /// <summary>
            /// Восстановление срока (ИЗ)
            /// </summary>
            public const string BRecoveryPeriod = "B03.3.1.1.1";

            /// <summary>
            /// Восстановление срока (ПМ)
            /// </summary>
            public const string URecoveryPeriod = "U03.3.1";

            /// <summary>
            /// Запрос экспертизы  (ИЗ)
            /// </summary>
            public const string BExaminationRequest = "B03.3.1.1";

            /// <summary>
            /// Запрос экспертизы  (ПМ)
            /// </summary>
            public const string UExaminationRequest = "U03.2.1";

            /// <summary>
            /// Запрос экспертизы  (СД) SA03.3.1
            /// </summary>
            public const string SaExaminationRequest = "SA03.3.1";

            /// <summary>
            /// Запрос экспертизы  (ПО)
            /// </summary>
            public const string PoExaminationRequest = "PO03.2.1";

            /// <summary>
            /// Секретка PO02.1.0
            /// </summary>
            public const string PoSecret = "PO02.1.0";
		}

        #endregion
    }
}