using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;

namespace Iserv.Niis.Domain.Entities.Request
{
    /// <summary>
    /// Дополнительные документы
    /// </summary>
    public class AdditionalDoc : Entity<int>
    {
        public string Code { get; set; }

        /// <summary>
        /// Gazette reference of publication
        /// </summary>
        public string GazetteReference { get; set; }

        /// <summary>
        /// Date of publication. 
        /// </summary>
        public DateTimeOffset? PublicationDate { get; set; }

        /// <summary>
        /// Date of recordal in the International Register
        /// </summary>
        public DateTimeOffset? IntRegisterRegnDate { get; set; }

        /// <summary>
        /// Effective date of modification The date that a transaction recorded in the international register in respect of a given international registration has effect.
        /// </summary>
        public DateTimeOffset? IntRegisterEffectiveDate { get; set; }

        /// <summary>
        /// Notification Date  
        /// </summary>
        public DateTimeOffset? NotificationDate { get; set; }
        
        #region Relationships

        /// <summary>
        /// Office of Origin Code The two letter country code (WIPO ST3.) that is used to identify the Office of Origin. 
        /// </summary>
        public int? OfficeOfOriginCountryId { get; set; }

        public DicCountry OfficeOfOriginCountry { get; set; }

        /// <summary>
        /// Охранный документ
        /// </summary>
        public int RequestId { get; set; }

        public Entities.Request.Request Request { get; set; }

        #endregion
    }

    public class AdditionalDocType
    {
        /// <summary>
        /// 17.1 Registration
        /// </summary>
        public const string Registration = "ENN";
        /// <summary>
        /// 17.2 Subsequent designation
        /// </summary>
        public const string SubsequentDesignation = "EXN";
        /// <summary>
        /// 17.3 Continuation of effect
        /// </summary>
        public const string ContinuationOfEffect = "CEN";
        /// <summary>
        /// 17.4 Total provisional refusal of protection
        /// </summary>
        public const string TotalProvisionalRefusalOfProtection = "RFNT";
        /// <summary>
        /// 17.5 Statement indicating that the mark is protected for all the goods and services requested
        /// </summary>
        public const string StatementIndicatingThatTheMarkIsProtected = "FINV";
        /// <summary>
        /// 17.6 Renewal
        /// </summary>
        public const string Renewal = "REN";
        /// <summary>
        /// 17.7 Limitation
        /// </summary>
        public const string Limitation = "LIN";
        /// <summary>
        /// 17.8 Statement of grant of protection made under Rule 18ter(1)
        /// </summary>
        public const string StatementOfGrantOfProtectionMadeUnderRule = "GP18N";
        /// <summary>
        /// 17.9 Ex Officio examination completed but opposition or observations by third parties still possible, under Rule 18bis(1)
        /// </summary>
        public const string ExOfficioExaminationCompleted = "ISN";
        /// <summary>
        /// 17.10 Confirmation of total provisional refusal under Rule 18ter(3)
        /// </summary>
        public const string ConfirmationOfTotalProvisionalRefusal = "R18NT";
        /// <summary>
        /// 17.11 Partial invalidation
        /// </summary>
        public const string PartialInvalidation = "INNP";
        /// <summary>
        /// 17.12 Statement indicating the goods and services for which protection of the mark is granted under Rule 18ter(2)(ii)
        /// </summary>
        public const string StatementIndicatingTheGoodsAndServices = "R18NP";
        /// <summary>
        /// 17.13 Further statement under Rule 18ter(4) indicating that protection of the mark is granted for all the goods and services requested
        /// </summary>
        public const string FurtherStatementUnderRule = "FDNV";
        /// <summary>
        /// 17.14 Opposition possible after the 18 months time limit
        /// </summary>
        public const string OppositionPossibleAfterTimeLimit = "OPN";
        /// <summary>
        /// 17.15 Statement of grant of protection following a provisional refusal under Rule 18ter(2)(i)
        /// </summary>
        public const string StatementOfGrantOfProtectionFollowingProvisionalRefusal = "R18NV";
        /// <summary>
        /// 17.16 Partial provisional refusal of protection
        /// </summary>
        public const string PartialProvisionalRefusalOfProtection = "RFNP";

        public static Dictionary<string, string> Descriptions => new Dictionary<string, string>()
        {
            {Registration, "Registration"},
            {SubsequentDesignation, "Subsequent designation"},
            {ContinuationOfEffect, "Continuation of effect"},
            {TotalProvisionalRefusalOfProtection, "Total provisional refusal of protection"},
            {StatementIndicatingThatTheMarkIsProtected, "Statement indicating that the mark is protected for all the goods and services requested"},
            {Renewal, "Renewal"},
            {Limitation, "Limitation"},
            {StatementOfGrantOfProtectionMadeUnderRule, "Statement of grant of protection made under Rule 18ter(1)"},
            {ExOfficioExaminationCompleted, "Ex Officio examination completed but opposition or observations by third parties still possible, under Rule 18bis(1)"},
            {ConfirmationOfTotalProvisionalRefusal, "Confirmation of total provisional refusal under Rule 18ter(3)"},
            {PartialInvalidation, "Partial invalidation"},
            {StatementIndicatingTheGoodsAndServices, "Statement indicating the goods and services for which protection of the mark is granted under Rule 18ter(2)(ii)"},
            {FurtherStatementUnderRule, "Further statement under Rule 18ter(4) indicating that protection of the mark is granted for all the goods and services requested"},
            {OppositionPossibleAfterTimeLimit, "Opposition possible after the 18 months time limit"},
            {StatementOfGrantOfProtectionFollowingProvisionalRefusal, "Statement of grant of protection following a provisional refusal under Rule 18ter(2)(i)"},
            {PartialProvisionalRefusalOfProtection, "Partial provisional refusal of protection"},
        };
    }
}