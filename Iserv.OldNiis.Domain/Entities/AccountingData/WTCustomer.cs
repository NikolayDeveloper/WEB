using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.AccountingData {
    [Table("WT_CUSTOMER")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class WTCustomer {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("date_create")]
        public DateTime? DateCreate { get; set; }
        [Column("stamp")]
        public DateTime? DateUpdate { get; set; }
        [Column("TYPE_ID")]
        public int TypeId { get; set; }
        [Column("flXIN")]
        public string flXIN { get; set; }
        [Column("flIsSMB")]
        public string flIsSmb { get; set; }
        [Column("CUS_NAME_ML_EN")]
        public string CusNameMlEn { get; set; }
        [Column("CUS_NAME_ML_RU")]
        public string CusNameMlRu { get; set; }
        [Column("CUS_NAME_ML_KZ")]
        public string CusNameMlKz { get; set; }
        [Column("RTN")]
        public string RTN { get; set; }
        [Column("PHONE")]
        public string Phone { get; set; }
        [Column("FAX")]
        public string Fax { get; set; }
        [Column("EMAIL")]
        public string EMail { get; set; }
        [Column("JUR_REG_NUMBER")]
        public string JurRegNumber { get; set; }
        [Column("CONTACT_FACE")]
        public string ContactFace { get; set; }
        [Column("ATT_CODE")]
        public string AttCode { get; set; }
        [Column("ATT_STAT_REG")]
        public string AttStatReg { get; set; }
        [Column("ATT_STAT_REG_DATE")]
        public DateTime? AttStatRegDate { get; set; }
        [Column("ATT_SPHERE_WORK")]
        public string AttSphereWork { get; set; }
        [Column("ATT_SPHERE_KNOW")]
        public string AttSphereKnow { get; set; }
        [Column("ATT_WORK_PLACE")]
        public string AttWorkPlace { get; set; }
        [Column("ATT_LANG")]
        public string AttLang { get; set; }
        [Column("ATT_DATE_CARD")]
        public DateTime? AttDateCard { get; set; }
        [Column("ATT_DATE_DISCARD")]
        public DateTime? AttDateDiscard { get; set; }
        [Column("ATT_DATE_BEGIN_STOP")]
        public DateTime? AttDateBeginStop { get; set; }
        [Column("ATT_DATE_END_STOP")]
        public DateTime? AttDateEndStop { get; set; }
        [Column("ATT_SOME_DATE")]
        public string AttSomeDate { get; set; }
        [Column("ATT_PLATPOR")]
        public string AttPlatpor { get; set; }
        [Column("ATT_PUBLIC")]
        public DateTime? AttPublic { get; set; }
        [Column("ATT_PUBLIC_REDEFINE")]
        public string AttPublicRedefine { get; set; }
        [Column("ATT_REDEFINE")]
        public string AttRedefine { get; set; }
        [Column("ATT_EDUCATION")]
        public string AttEducation { get; set; }
        [Column("COUNTRY_ID")]
        public int? CountryId { get; set; }
        [Column("ADDRESS_ID")]
        public int? AddressId { get; set; }
        [Column("LOGIN")]
        public string Login { get; set; }
        [Column("PASSWORD_")]
        public string Password_ { get; set; }
        [Column("SUBSCRIPT")]
        public string Subscript { get; set; }
        [Column("flApplicantsInfo")]
        public string flApplicantsInfo { get; set; }
        [Column("flCertificateSeries")]
        public string flCertificateSeries { get; set; }
        [Column("flCertificateNumber")]
        public string flCertificateNumber { get; set; }
        [Column("flRegDate")]
        public DateTime? flRegDate { get; set; }
        [Column("flOpf")]
        public string flOpf { get; set; }
        [Column("flPowerAttorneyFullNum")]
        public string flPowerAttorneyFullNum { get; set; }
        [Column("flPowerAttorneyDateIssue")]
        public DateTime? flPowerAttorneyDateIssue { get; set; }
        [Column("flNotaryName")]
        public string flNotaryName { get; set; }
        [Column("flShortDocContent")]
        public string flShortDocContent { get; set; }
        [Column("CUS_NAME_ML_EN_long")]
        public string CusNameMlEnLong { get; set; }
        [Column("CUS_NAME_ML_RU_long")]
        public string CusNameMlRuLong { get; set; }
        [Column("CUS_NAME_ML_KZ_long")]
        public string CusNameMlKzLong { get; set; }
    }
}
