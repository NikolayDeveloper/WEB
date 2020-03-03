using System;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.EarlyReg;
using Iserv.Niis.Model.Models.Journal;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.Model.Models.Subject;

namespace Iserv.Niis.Model.Models.Request
{
    public class RequestDetailDto : IntellectualPropertyDto
    {
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string DescriptionRu { get; set; }
        public string DescriptionKz { get; set; }
        public string DescriptionEn { get; set; }
        public string IncomingNumber { get; set; }
        public string StatusSending { get; set; }
        public bool? IsDocSendToEmail { get; set; }
        public string IncomingNumberFilial { get; set; }
        public string RequestNum { get; set; }
        public DateTimeOffset? RequestDate { get; set; }
        public int? CopyCount { get; set; }
        public int? PageCount { get; set; }
        public bool? WasScanned { get; set; }
        public int? TemplateDataFileId { get; set; }
        public int? ScanFileId { get; set; }
        public int? CurrentWorkflowId { get; set; }
        public string DisclaimerRu { get; set; }
        public string DisclaimerKz { get; set; }
        public string DisclaimerEn { get; set; }
        public WorkflowDto CurrentWorkflow { get; set; }
        public PaymentInvoiceDto[] InvoiceDtos { get; set; }
        public SubjectDto[] Subjects { get; set; }
        public IcgsDto[] IcgsRequestDtos { get; set; }
        public RequestEarlyRegDto[] RequestEarlyRegDtos { get; set; }
        public ConventionInfoDto[] RequestConventionInfoDtos { get; set; }
        public int[] ParentRequestIds { get; set; }
        public int[] ChildsRequestIds { get; set; }
        public int[] IcisRequestIds { get; set; }
        public int[] IpcIds { get; set; }
        public int? MainIpcId { get; set; }
        public int[] IcfemIds { get; set; }
        public bool? HasRequiredOnCreate { get; set; }

        public bool IsImageFromName { get; set; }
        public string ImageUrl { get; set; }
        public string OutgoingNumber { get; set; }
        public DateTimeOffset? OutgoingDate { get; set; }
        public string ExpertSearchKeywords { get; set; }
        public bool? HasProxy { get; set; }
        public int? BeneficiaryTypeId { get; set; }
        public bool? IsRejected { get; set; }
        public string RejectionReason { get; set; }
        public string ColectiveTrademarkParticipantsInfo { get; set; }
        public string OutgoingNumberFilial { get; set; }
        public DateTimeOffset? PublicDate { get; set; }
        public bool? IsSyncRequestNum { get; set; }

        public bool? IsFromLk { get; set; }

        #region Referenced Keys

        public int? StatusId { get; set; }
        public int ProtectionDocTypeId { get; set; }
        public int? RequestTypeId { get; set; }
        public int? ReceiveTypeId { get; set; }
        public int? ProtectionDocId { get; set; }
        public int? AddresseeId { get; set; }
        public ContactInfoDto[] ContactInfos { get; set; }
        public int? UserId { get; set; }
        public int? DivisionId { get; set; }
        public int? FlDivisionId { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentNameRu { get; set; }
        public int? ApplicantTypeId { get; set; }
        public int? RequestInfoId { get; set; }
        public int? ConventionTypeId { get; set; }

        //Специфические для типов заявки поля
        //ТЗ
        public string Priority { get; set; }
        public bool? IsExhibitPriority { get; set; }
        public string Transliteration { get; set; }
        public string Translation { get; set; }
        public bool? IsColorPerformance { get; set; }
        public int[] ColorTzIds { get; set; }
        public DateTimeOffset? DateRecognizedKnown { get; set; }
        public string InfoDecisionToRecognizedKnown { get; set; }
        public string InfoConfirmKnownTrademark { get; set; }
        public int? TypeTrademarkId { get; set; }
        //НМПТ
        public string SelectionFamily { get; set; }
        public string ProductSpecialProp { get; set; }
        public string ProductPlace { get; set; }
        public bool IsHasMaterialExpertOpinionWithOugoingNumber { get; set; }

        //Изобретения, полезные модели, промышленные образцы
        public string Referat { get; set; }
        //Селекционные достижения
        public string Genus { get; set; }
        public string BreedingNumber { get; set; }
        public int? BreedCountryId { get; set; }
        public string BreedCountryNameRu { get; set; }
        public int? SelectionAchieveTypeId { get; set; }

        /// <summary>
        /// Постледний статус отправленный в ЛК
        /// </summary>
        public int? LastOnlineRequisitionStatusId { get; set; }
        /// <summary>
        /// Дата регистрации охранного документа
        /// </summary>
        public DateTime? RegisterDateProtectionDoc { get; set; }

        /// <summary>
        /// Ожидаемая дата окончания регистрации / продления
        /// </summary>
        public DateTime? ExpectedValidDateProtectionDoc { get; set; }
        #endregion

        #region Referenced Values

        public string ReceiveTypeValue { get; set; }
        public string ProtectionDocTypeCode { get; set; }
        public string RequestTypeCode { get; set; }
        public string ProtectionDocValue { get; set; }
        public string AddresseeXin { get; set; }
        public string AddresseeNameRu { get; set; }
        public string AddresseeShortAddress { get; set; }
        public string AddresseeAddress { get; set; }
        public string Republic { get; set; }
        public string Oblast { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Apartment { get; set; }
        public string AddresseeEmail { get; set; }
        public string UserValue { get; set; }
        public string DivisionValue { get; set; }
        public string FlDivisionValue { get; set; }
        public string DepartmentValue { get; set; }
        public string ApplicantTypeValue { get; set; }
        public string RequestInfoValue { get; set; }
        #endregion
    }
}