using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.Patent
{
    /// <summary>
    /// Заявки и ОД История(BT_BASE_PATENT_HISTORY)
    /// </summary>
    public class PatentHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public int? StatusId { get; set; } //STATUS_ID
        public string GosNumber { get; set; } //GOS_NUMBER_11
        public DateTimeOffset? GosDate { get; set; } //GOS_DATE_11
        public string RegNumber { get; set; } //REQ_NUMBER_21
        public DateTimeOffset? RegDate { get; set; } //REQ_DATE_22
        public int? ConsiderationId { get; set; } //CONSID_ID
        public int? RequestionFeedId { get; set; } //TYPE_REQUEST
        public string NumberBulletin { get; set; } //NBY
        public DateTimeOffset? BulletinDate { get; set; } //DBY
        public DateTimeOffset? ValidDate { get; set; } //STZ17
        public DateTimeOffset? ExtensionDate { get; set; } //STZ176
        public int? PatentSubTypeId { get; set; } //SUBTYPE_ID
        public DateTimeOffset? CreatBeginDate { get; set; } //DATAZAKP
        public DateTimeOffset? CreateEndDate { get; set; } //DATAZAKPP
        public DateTimeOffset? ProxyWithDate { get; set; } //DPOS
        public DateTimeOffset? ProxyForDate { get; set; } //DPOP
        public int? AddressId { get; set; } //flAddress
        public int? Storona1 { get; set; } //Storona1
        public string DisclaimerRu { get; set; } //DISCLAM_RU
        public string DisclaimerKz { get; set; } //DISCLAM_KZ
        public int? CCCustomerLinkTypeId { get; set; } //PRIZA 
        public int? ConventionTypeId { get; set; } //TYPEII_ID
        public int? ImageId { get; set; }
        public DateTimeOffset? TransferDate { get; set; } //DATE_85
        public bool? DeclarantEmployer { get; set; } //DECLARANT_EMPLOYER
        public bool? CopyrightEmployer { get; set; } //COPYRIGHT_EMPLOYER
        public bool? CopyrightAuthor { get; set; } //COPYRIGHT_AUTOR
        public string SelectionNumber { get; set; } //SELECTION_NUMBER
        public bool? Inherit { get; set; } //INHERIT
        public string Referat { get; set; } //REF_57
        public string SelectionFamily { get; set; } //SELECTION_FAMILY
        public string SelectionNameOffer { get; set; } //SELECTION_NAME_OFFER
        public bool? Otkaz { get; set; } //OTKAZ int
        public int? CodePatentType { get; set; } //KODTIP
        public int? CodeStage { get; set; } //KODETAP
        public bool? ReqNeedTranslation { get; set; } //JZ int
        public int? RegExchange { get; set; } //IZ3PER
        public string AdditionalInfo { get; set; } //PROCH
        public string OtherDocuments { get; set; } //DDOK
        public int? TypePatent { get; set; } //TIP_PAT
        public string DataInitialPublication { get; set; } //PARO
        public int? CodeStageExpertise { get; set; } //KODETAP1
        public DateTimeOffset? TransferOrgDate { get; set; } //GO_ORG
        public int? RKSCpf { get; set; } //CPF
        public DateTimeOffset? TransferOeiDate { get; set; } //GO_OEI
        public string NumberApxWork { get; set; } //KOD_OEI
        public string CodeExpert { get; set; } //KOD_EXP
        public DateTimeOffset? IssuePatentDate { get; set; } //DVIDOD
        public DateTimeOffset? SendingDate { get; set; } //IN_DATE
        public string OldDecisionReqInfo { get; set; } //OLD_RESH
        public int? MarkWaitAnswer { get; set; } //WAIT_ANSW
        public DateTimeOffset? IssuePrepatentDate { get; set; } //DVIDODPP
        public int? DirectionDate { get; set; } //MOSKOW
        public DateTimeOffset? SendPatentDate { get; set; } //OUT_DATE
        public DateTimeOffset? RegisStateRegisterDate { get; set; } //DATGRP
        public DateTimeOffset? SendExpertiseDate { get; set; } //DATGRE
        public DateTimeOffset? EarlyTerminationDate { get; set; } //DATPDPAT
        public int? CodeTermination { get; set; } //KODPDPAT
        public string LicenseInfo { get; set; } //LICENZ
        public string LicenseInfoStateRegister { get; set; } //LICENZ1
        public int? PresenceOpenLicense { get; set; } //LICOPEN
        public string NumberCopyrightCertificate { get; set; } //NAC
        public int? SEIWaitFe { get; set; } //WAIT_FE
        public string PaperworkStateRegister { get; set; } //DVPP
        public DateTimeOffset? IssueConsentDate { get; set; } //DPRINT
        public DateTimeOffset? IssueFinalDate { get; set; } //DFIRST
        public DateTimeOffset? BoardAppealDate { get; set; } //DQUEST
        public DateTimeOffset? SEIReviewDate { get; set; } //DOTZIV
        public DateTimeOffset? SEISdakDate { get; set; } //DSDAK
        public DateTimeOffset? SEIInEfDate { get; set; } //IN_DATEF
        public DateTimeOffset? SEIAnswerDate { get; set; } //DOTVETDO
        public int? SEIOsn { get; set; } //OSN4
        public int? SEIDospyb { get; set; } //DOSPYB
        public DateTimeOffset? DateSendSearch { get; set; } //DMOSKOW
        public DateTimeOffset? ReceivedDocDate { get; set; } //DMOSKOW2
        public DateTimeOffset? RequisitionReceiptDate { get; set; } //DPVZ
        public int? YearMaintain { get; set; } //GOD_PROD
        public DateTimeOffset? RecoveryPetitionDate { get; set; } //DHODVOST
        public DateTimeOffset? HODDate { get; set; } //DATHOD
        public DateTimeOffset? EndDate2 { get; set; } //DATA_END2
        public DateTimeOffset? GRPADate { get; set; } //DATGRPA
        public DateTimeOffset? DateExpiryPaymentTerm { get; set; } //DATUVO4
        public DateTimeOffset? DateRegExtension { get; set; } //DHODPROD
        public string ClassGrafElement { get; set; } //GOSP
        public string NumberEarlyReg { get; set; } //PROM
        public DateTimeOffset? ExtensionDateTz { get; set; } //STZ156
        public string Code60 { get; set; } //NM60
        public int? ToPm { get; set; } //TO_PM
        public int? InfP { get; set; } //INF_P
        public DateTimeOffset? DIzyat { get; set; } //DIzyat
        public int? Kppp { get; set; } //Kppp
        public int? ReturnDateFips { get; set; } //RESHFIPS
        public DateTimeOffset? DviDpi { get; set; } //DVIDPI
        public string Preds { get; set; } //PREDS
        public DateTimeOffset? DQUESTF { get; set; } //DQUESTF
        public DateTimeOffset? OutDatef { get; set; } //OUT_DATEF
        public DateTimeOffset? DOTZIVF { get; set; } //DOTZIVF
        public string Transliteration { get; set; } //TRASLITERATION
        public DateTimeOffset? Des { get; set; } //DES
        public DateTimeOffset? PreliminaryFailureDate { get; set; } //DATA_POL
        public DateTimeOffset? FillingObjectionDate { get; set; } //DATA_END
        public int? SENPCTIPEA { get; set; } //NPCTIPEA
        public int? SENPSTISA { get; set; } //NPSTISA
        public int? SENKPPYB { get; set; } //NKPPYB
        public int? SmallImageId { get; set; }
        public string CustomerApplication { get; set; } //PatentCustomerAppWithAddress
        public string CustomerAttorney { get; set; } //PatentCustomerAttorney
        public string CustomerAttorneyWithAddress { get; set; } //PatentCustomerAttorneyWithAddress
        public string CustomerAuthor { get; set; } //PatentCustomerAuthor
        public string CustomerContact { get; set; } //PatentCustomerContact
        public string ContactParceFull { get; set; } //GetContactParceFull
        public string CustomerConfidant { get; set; } //PatentCustomerConfidant
        public string CustomerOwner { get; set; } //PatentCustomerOwnerWithAddress
        public string Icfem { get; set; } //PatentICFEM
        public string Icgs { get; set; } //PatentICGS
        public string Icis { get; set; } //PatentICIS
        public string Mpk { set; get; } //PatentIPC
        public string PatentStorona1WithAddress { get; set; } //PatentStorona1WithAddress
        public string PatentStorona2WithAddress { get; set; } //PatentStorona2WithAddress
    }
}