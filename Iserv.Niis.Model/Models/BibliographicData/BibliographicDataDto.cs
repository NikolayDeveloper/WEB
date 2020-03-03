using System;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.EarlyReg;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Model.Models.Subject;

namespace Iserv.Niis.Model.Models.BibliographicData
{
    public class BibliographicDataDto
    {
        public int Id { get; set; }
        public Owner.Type OwnerType { get; set; }
        public int? CurrentWorkflowId { get; set; }
        public WorkflowDto CurrentWorkflow { get; set; }
        public int ProtectionDocTypeId { get; set; }
        public string ProtectionDocTypeCode { get; set; }
        public PaymentInvoiceDto[] InvoiceDtos { get; set; }
        public SubjectDto[] Subjects { get; set; }
        public string ImageUrl { get; set; }
        //imageFile: File;
        public string Transliteration { get; set; }
        public int[] ColorTzIds { get; set; }
        public string DisclaimerRu { get; set; }
        public string DisclaimerKz { get; set; }
        public string DisclaimerEn { get; set; }
        public int? TypeTrademarkId { get; set; }
        public DateTimeOffset? DateRecognizedKnown { get; set; }
        public string InfoDecisionToRecognizedKnown { get; set; }
        public string InfoConfirmKnownTrademark { get; set; }
        public string SelectionFamily { get; set; }
        public string ProductSpecialProp { get; set; }
        public string ProductPlace { get; set; }
        public string Referat { get; set; }
        public int? ConventionTypeId { get; set; }
        public string Genus { get; set; }
        public string BreedingNumber { get; set; }
        public int? BreedCountryId { get; set; }
        public string BreedCountryNameRu { get; set; }
        public bool IsImageFromName { get; set; }
        public int[] IcfemIds { get; set; }
        public int[] IpcIds { get; set; }
        public int? MainIpcId { get; set; }
        public int[] IcisRequestIds { get; set; }
        public IcgsDto[] IcgsRequestDtos { get; set; }
        public RequestEarlyRegDto[] RequestEarlyRegDtos { get; set; }
        public ConventionInfoDto[] RequestConventionInfoDtos { get; set; }
        public string Translation { get; set; }
        public bool? IsColorPerformance { get; set; }
        public string Priority { get; set; }
        public bool? IsExhibitPriority { get; set; }
        public string RequestNum { get; set; }
        public int? RequestTypeId { get; set; }
        public DateTimeOffset? RequestDate { get; set; }
        public bool? HasProxy { get; set; }
        public int? BeneficiaryTypeId { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameKz { get; set; }
        public int? CountIndependentItems { get; set; }
        public string GosNumber { get; set; }
        public int? SelectionAchieveTypeId { get; set; }
        public DateTimeOffset? GosDate { get; set; }
        public int? StatusId { get; set; }
        public DateTimeOffset? ExtensionDate { get; set; }
        public DateTimeOffset? ValidDate { get; set; }
        public string YearMaintain { get; set; }
        public int? BulletinId { get; set; }
        public int? SpeciesTradeMarkId { get; set; }
        public string RejectionReason { get; set; }
        public string ColectiveTrademarkParticipantsInfo { get; set; }
        public string OutgoingNumberFilial { get; set; }
        public DateTimeOffset? PublishDate { get; set; }

        /// <summary>
        /// Дата регистрации охранного документа
        /// </summary>
        public DateTime? RegisterDateProtectionDoc { get; set; }

        /// <summary>
        /// Ожидаемая дата окончания регистрации / продления
        /// </summary>
        public DateTime? ExpectedValidDateProtectionDoc { get; set; }

        /// <summary>
        /// Постледний статус отправленный в ЛК
        /// </summary>
        public int? LastOnlineRequisitionStatusId { get; set; }
    }
}
