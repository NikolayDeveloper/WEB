using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.BibliographicData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Domain.EntitiesFile;
using Iserv.Niis.Domain.EntitiesHistory.Document;

namespace Iserv.Niis.Domain.Entities.Request
{
    public class Request : Entity<int>, IHaveBarcode, IHistorySupport, IHaveConcurrencyToken, IHaveImageAttachment,
        ISoftDeletable
    {
        public Request()
        {
            Documents = new HashSet<RequestDocument>();
            ParentRequests = new HashSet<RequestRequestRelation>();
            ChildsRequests = new HashSet<RequestRequestRelation>();
            Icfems = new HashSet<DicIcfemRequestRelation>();
            ColorTzs = new HashSet<DicColorTZRequestRelation>();
            ICGSRequests = new HashSet<ICGSRequest>();
            ICISRequests = new HashSet<ICISRequest>();
            IPCRequests = new HashSet<IPCRequest>();
            Workflows = new HashSet<RequestWorkflow>();
            PaymentInvoices = new HashSet<PaymentInvoice>();
            RequestCustomers = new HashSet<RequestCustomer>();
            Contracts = new HashSet<ContractRequestRelation>();
            EarlyRegs = new HashSet<RequestEarlyReg>();
            RequestProtectionDocSimilarities = new HashSet<RequestProtectionDocSimilar>();
            ExpertSearchSimilarities = new HashSet<ExpertSearchSimilar>();
            NotificationStatuses = new HashSet<RequestNotificationStatus>();
            RequestConventionInfos = new HashSet<RequestConventionInfo>();
            MediaFiles = new HashSet<Attachment>();
            PaymentExecutors = new HashSet<PaymentExecutor>();
            MadeChanges = new HashSet<MadeChange>();
        }
        //public DateTimeOffset DateUpdate { get; set; }
        public int Barcode { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }

        public int? StatusId { get; set; }
        public DicRequestStatus Status { get; set; }
        public int ProtectionDocTypeId { get; set; }
        public DicProtectionDocType ProtectionDocType { get; set; }
        public int? RequestTypeId { get; set; }
        public DicProtectionDocSubType RequestType { get; set; }
        public string IncomingNumber { get; set; }
        public string StatusSending { get; set; }
        public bool? IsDocSendToEmail { get; set; }
        public string IncomingNumberFilial { get; set; }
        public int? ReceiveTypeId { get; set; }
        public DicReceiveType ReceiveType { get; set; }
        public int? AddresseeId { get; set; }
        public DicCustomer Addressee { get; set; }
        public string AddresseeAddress { get; set; }
        public int? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int? DivisionId { get; set; }
        public DicDivision Division { get; set; }
        public int? FlDivisionId { get; set; }
        public DicDivision FlDivision { get; set; }
        public string RequestNum { get; set; }
        public DateTimeOffset? RequestDate { get; set; }
        public int? DepartmentId { get; set; }
        public DicDepartment Department { get; set; }
        public int? ConventionTypeId { get; set; }
        public DicConventionType ConventionType { get; set; }
        public string SelectionFamily { get; set; }
        public string ProductPlace { get; set; }
        public string Referat { get; set; }
        public int? CopyCount { get; set; }
        public int? PageCount { get; set; }
        public bool IsComplete { get; set; }
        public int? TemplateDataFileId { get; set; }
        public string OutgoingNumber { get; set; }
        public DateTimeOffset? OutgoingDate { get; set; }

        public int? ScanFileId { get; set; }
        public int? ApplicantTypeId { get; set; } //APPLICANTTYPEID
        public DicApplicantType ApplicantType { get; set; }
        public int? RequestInfoId { get; set; }
        public RequestInfo RequestInfo { get; set; }
        public bool IsRead { get; set; }
        public int? CurrentWorkflowId { get; set; }
        public RequestWorkflow CurrentWorkflow { get; set; }
        public string Transliteration { get; set; }
        public string NumberBulletin { get; set; }
        public DateTimeOffset? PublicDate { get; set; }
        public string DisclaimerRu { get; set; }
        public string DisclaimerKz { get; set; }
        public string DisclaimerEn { get; set; }
        public DateTimeOffset? TransferDate { get; set; }
        public DateTimeOffset? DateRecognizedKnown { get; set; }
        public string InfoDecisionToRecognizedKnown { get; set; }
        public string InfoConfirmKnownTrademark { get; set; }
        public int? SpeciesTradeMarkId { get; set; }
        public DicProtectionDocSubType SpeciesTradeMark { get; set; }
        public int? TypeTrademarkId { get; set; }
        public DicTypeTrademark TypeTrademark { get; set; }
        public int? SelectionAchieveTypeId { get; set; }
        public DicSelectionAchieveType SelectionAchieveType { get; set; }
        public int? MainAttachmentId { get; set; }
        public Attachment MainAttachment { get; set; }
        public int? BeneficiaryTypeId { get; set; }
        public DicBeneficiaryType BeneficiaryType { get; set; }
        public int? CountIndependentItems { get; set; }
        public double? CoefficientComplexity { get; set; }
        public string Translation { get; set; }
        public bool? IsRejected { get; set; }
        public string RejectionReason { get; set; }
        public string ColectiveTrademarkParticipantsInfo { get; set; }
        public string OutgoingNumberFilial { get; set; }
        public DateTimeOffset? PublishDate { get; set; }

        public Type GetHistoryEntity()
        {
            return typeof(DocumentHistory);
        }

        public ICollection<RequestDocument> Documents { get; set; }
        public ICollection<RequestRequestRelation> ParentRequests { get; set; }
        public ICollection<RequestRequestRelation> ChildsRequests { get; set; }
        public ICollection<ICGSRequest> ICGSRequests { get; set; }
        public ICollection<ICISRequest> ICISRequests { get; set; }
        public ICollection<IPCRequest> IPCRequests { get; set; }
        public ICollection<DicIcfemRequestRelation> Icfems { get; set; }
        public ICollection<DicColorTZRequestRelation> ColorTzs { get; set; }
        public ICollection<RequestWorkflow> Workflows { get; set; }
        public ICollection<PaymentInvoice> PaymentInvoices { get; set; }
        public ICollection<RequestCustomer> RequestCustomers { get; set; }
        public ICollection<ContractRequestRelation> Contracts { get; set; }
        public ICollection<RequestEarlyReg> EarlyRegs { get; set; }
        public ICollection<RequestProtectionDocSimilar> RequestProtectionDocSimilarities { get; set; }
        public ICollection<ExpertSearchSimilar> ExpertSearchSimilarities { get; set; }
        public ICollection<RequestNotificationStatus> NotificationStatuses { get; set; }
        public ICollection<RequestConventionInfo> RequestConventionInfos { get; set; }
        public ICollection<Attachment> MediaFiles { get; set; }
        public ICollection<PaymentExecutor> PaymentExecutors { get; set; }
        public ICollection<MadeChange> MadeChanges { get; set; }

        /// <summary>
        /// Дополнительные документы
        /// </summary>
        public ICollection<AdditionalDoc> AdditionalDocs { get; set; }

        public override string ToString()
        {
            return NameRu;
        }

        public void MarkAsUnRead()
        {
            IsRead = false;
        }

        [NonSerialized] [NotMapped] public DocumentTemplateDataFile DocumentTemplateDataFile = null;

        public bool IsImageFromName { get; set; }
        public byte[] Image { get; set; }
        public byte[] PreviewImage { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public string ExpertSearchKeywords { get; set; }
        /// <summary>
        /// (891) Дата распространения на РК
        /// </summary>
        public DateTimeOffset? DistributionDate { get; set; }
        public string RomarinColor { get; set; }
        public bool? IsOnChangeScenario { get; set; }
        public bool? IsFormalExamFeeNotPaidInTime { get; set; }
        public bool? RequiresPaymentExecutor { get; set; }
        
        /// <summary>
        /// Постледний статус отправленный в ЛК
        /// </summary>
        public int? LastOnlineRequisitionStatusId { get; set; }
        public DicOnlineRequisitionStatus LastOnlineRequisitionStatus { get; set; }

        /// <summary>
        /// Отметка о том, что заявка пришла с ЛК
        /// </summary>
        public bool? IsFromLk { get; set; }
        /// <summary>
        /// Отмета успешной отправки рег. номера в ЛК
        /// </summary>
        public bool? IsSyncRequestNum { get; set; }

        /// <summary>
        /// Тип Рассмотрения
        /// </summary>
        public int? ConsiderationTypeId { get; set; }
        public DicConsiderationType ConsiderationType { get; set; }

        /// <summary>
        /// Дата регистрации охранного документа
        /// </summary>
        public DateTime? RegisterDateProtectionDoc { get; set; }

        /// <summary>
        /// Ожидаемая дата окончания регистрации / продления
        /// </summary>
        public DateTime? ExpectedValidDateProtectionDoc { get; set; }
    }
}