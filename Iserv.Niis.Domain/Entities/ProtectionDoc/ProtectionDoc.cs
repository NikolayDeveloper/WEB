using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Domain.EntitiesHistory.Patent;

namespace Iserv.Niis.Domain.Entities.ProtectionDoc
{
    /// <summary>
    /// Охранный документ
    /// </summary>
    public class ProtectionDoc : Entity<int>, IHaveBarcode, IHistorySupport, IHaveConcurrencyToken
    {


        public ProtectionDoc()
        {
            Icfems = new HashSet<DicIcfemProtectionDocRelation>();
            ColorTzs = new HashSet<DicColorTZProtectionDocRelation>();
            IpcProtectionDocs = new HashSet<IPCProtectionDoc>();
            IcisProtectionDocs = new HashSet<ICISProtectionDoc>();
            IcgsProtectionDocs = new HashSet<ICGSProtectionDoc>();
            Contracts = new HashSet<ContractProtectionDocRelation>();
            Workflows = new HashSet<ProtectionDocWorkflow>();
            PaymentInvoices = new HashSet<PaymentInvoice>();
            ProtectionDocCustomers = new HashSet<ProtectionDocCustomer>();
            Documents = new HashSet<ProtectionDocDocument>();
            EarlyRegs = new HashSet<ProtectionDocEarlyReg>();
            ExpertSearchSimilarities = new HashSet<ExpertSearchSimilar>();
            ProtectionDocConventionInfos = new HashSet<ProtectionDocConventionInfo>();
            Bulletins = new HashSet<ProtectionDocBulletinRelation>();
            RequestProtectionDocSimilarities = new HashSet<RequestProtectionDocSimilar>();
            MediaFiles = new HashSet<Attachment>();
        }
        public int? SupportUserId { get; set; }
        public ApplicationUser SupportUser { get; set; }
        public int? BulletinUserId { get; set; }
        public ApplicationUser BulletinUser { get; set; }
        public int Barcode { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string GosNumber { get; set; } //GOS_NUMBER_11
        public DateTimeOffset? GosDate { get; set; } //GOS_DATE_11
        public string RegNumber { get; set; } //REQ_NUMBER_21
        public DateTimeOffset? RegDate { get; set; } //REQ_DATE_22
        public DateTimeOffset? ValidDate { get; set; } //STZ17
        public DateTimeOffset? ExtensionDate { get; set; } //STZ176
        public DateTimeOffset? ProxyWithDate { get; set; } //DPOS
        public DateTimeOffset? ProxyForDate { get; set; } //DPOP
        public string DisclaimerRu { get; set; } //DISCLAM_RU
        public string DisclaimerKz { get; set; } //DISCLAM_KZ
        public int? ImageId { get; set; }
        public DateTimeOffset? TransferDate { get; set; } //DATE_85
        public bool? DeclarantEmployer { get; set; } //DECLARANT_EMPLOYER
        public bool? CopyrightEmployer { get; set; } //COPYRIGHT_EMPLOYER
        public bool? CopyrightAuthor { get; set; } //COPYRIGHT_AUTOR
        public string Referat { get; set; } //REF_57
        public string SelectionNameOffer { get; set; } //SELECTION_NAME_OFFER
        public bool? Otkaz { get; set; } //OTKAZ int
        public string AdditionalInfo { get; set; } //PROCH
        public string OtherDocuments { get; set; } //DDOK
        public string DataInitialPublication { get; set; } //PARO
        public string NumberApxWork { get; set; } //KOD_OEI
        public DateTimeOffset? EarlyTerminationDate { get; set; } //DATPDPAT
        public string LicenseInfo { get; set; } //LICENZ
        public string LicenseInfoStateRegister { get; set; } //LICENZ1
        public string NumberCopyrightCertificate { get; set; } //NAC
        public string PaperworkStateRegister { get; set; } //DVPP
        public DateTimeOffset? MaintainDate { get; set; }
        public DateTimeOffset? RecoveryPetitionDate { get; set; } //DHODVOST
        public DateTimeOffset? ExtensionDateTz { get; set; } //STZ156
        public string Code60 { get; set; } //NM60
        public int? ToPm { get; set; } //TO_PM
        public string Transliteration { get; set; } //TRASLITERATION
        public bool IsImageFromName { get; set; }
        public byte[] Image { get; set; }
        public byte[] PreviewImage { get; set; }
        public int? SmallImageId { get; set; }
        public string SelectionFamily { get; set; }
        public int? SelectionAchieveTypeId { get; set; }
        public DicSelectionAchieveType SelectionAchieveType { get; set; }
        public DateTimeOffset? PublicDate { get; set; }
        public ICollection<ExpertSearchSimilar> ExpertSearchSimilarities { get; set; }
        public ICollection<ProtectionDocConventionInfo> ProtectionDocConventionInfos { get; set; }
        public int? TypeTrademarkId { get; set; }
        public DicTypeTrademark TypeTrademark { get; set; }
        public int? SpeciesTradeMarkId { get; set; }
        public DicProtectionDocSubType SpeciesTradeMark { get; set; }
        public int? BeneficiaryTypeId { get; set; }
        public DicBeneficiaryType BeneficiaryType { get; set; }
        public string OutgoingNumber { get; set; }
        public DateTimeOffset? OutgoingDate { get; set; }
        public string Translation { get; set; }
        public string RejectionReason { get; set; }
        public string ColectiveTrademarkParticipantsInfo { get; set; }
        public string OutgoingNumberFilial { get; set; }
        public DateTimeOffset? PublishDate { get; set; }

        /// <summary>
        /// (891) Дата распространения на РК
        /// </summary>
        public DateTimeOffset? DistributionDate;
        public int? PageCount { get; set; }


        #region Relationships

        public int? StatusId { get; set; }
        public DicProtectionDocStatus Status { get; set; }
        public int TypeId { get; set; }
        public DicProtectionDocType Type { get; set; }
        public int? SubTypeId { get; set; } //SUBTYPE_ID
        public DicProtectionDocSubType SubType { get; set; }
        public int? ApplicantTypeId { get; set; }
        public DicApplicantType ApplicantType { get; set; }
        public int? ConsiderationTypeId { get; set; }
        public DicConsiderationType ConsiderationType { get; set; }
        public int? ConventionTypeId { get; set; }
        public DicConventionType ConventionType { get; set; }
        public int? CurrentWorkflowId { get; set; }
        public ProtectionDocWorkflow CurrentWorkflow { get; set; }
        public int? ProtectionDocInfoId { get; set; }
        public ProtectionDocInfo ProtectionDocInfo { get; set; }
        public int? RequestId { get; set; }
        public Request.Request Request { get; set; }
        public int? AddresseeId { get; set; }
        public DicCustomer Addressee { get; set; }
        public int? SendTypeId { get; set; }
        public DicSendType SendType { get; set; }

        /// <summary>
        /// Дата создания охранного документа.
        /// </summary>
        public DateTimeOffset DateOfCreation { get; set; }

        /// <summary>
        /// Основной МПК в охранных документах
        /// </summary>
        public string IsMainIpcs { get; set; }
        public ICollection<ProtectionDocProtectionDocRelation> Parents { get; set; }
        public ICollection<ProtectionDocProtectionDocRelation> Childs { get; set; }
        public ICollection<IPCProtectionDoc> IpcProtectionDocs { get; set; }
        public ICollection<ICISProtectionDoc> IcisProtectionDocs { get; set; }
        public ICollection<ICGSProtectionDoc> IcgsProtectionDocs { get; set; }
        public ICollection<DicIcfemProtectionDocRelation> Icfems { get; set; }
        public ICollection<DicColorTZProtectionDocRelation> ColorTzs { get; set; }
        public ICollection<ContractProtectionDocRelation> Contracts { get; set; }
        public ICollection<ProtectionDocWorkflow> Workflows { get; set; }
        public ICollection<PaymentInvoice> PaymentInvoices { get; set; }
        public ICollection<ProtectionDocCustomer> ProtectionDocCustomers { get; set; }
        public ICollection<ProtectionDocDocument> Documents { get; set; }
        public ICollection<ProtectionDocEarlyReg> EarlyRegs { get; set; }
        public ICollection<ProtectionDocBulletinRelation> Bulletins { get; set; }
        public ICollection<RequestProtectionDocSimilar> RequestProtectionDocSimilarities { get; set; }
        public ICollection<Attachment> MediaFiles { get; set; }

        public int? MainAttachmentId { get; set; }
        public Attachment MainAttachment { get; set; }

        public int? IntellectualPropertyId { get; set; }
        public IntellectualProperty.IntellectualProperty IntellectualProperty { get; set; }

        #endregion

        public Type GetHistoryEntity()
        {
            return typeof(PatentHistory);
        }

        public override string ToString()
        {
            var result = string.Empty;
            if (DateCreate != null)
                result += DateCreate.ToString("dd.MM.yyyy");
            var name = string.Empty;
            if (!string.IsNullOrEmpty(NameRu))
                name = NameRu;
            else if (!string.IsNullOrEmpty(NameKz))
                name = NameKz;
            else if (!string.IsNullOrEmpty(NameEn))
                name = NameEn;
            if (!string.IsNullOrEmpty(RegNumber))
                result = (string.IsNullOrEmpty(result) ? string.Empty : result + " - ") + RegNumber;
            if (!string.IsNullOrEmpty(name))
                result = (string.IsNullOrEmpty(result) ? string.Empty : result + " - ") + name;
            if (string.IsNullOrEmpty(result))
                return IntellectualPropertyId + string.Empty;
            return result;
        }
        public string Gosreestr { get; set; }
    }
}