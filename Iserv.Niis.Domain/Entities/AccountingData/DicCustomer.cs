using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.EntitiesHistory.AccountingData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iserv.Niis.Domain.Entities.AccountingData
{
    public class DicCustomer : DictionaryEntity<int>, IHistorySupport
    {
        public DicCustomer()
        {
            CustomerAttorneyInfos = new HashSet<CustomerAttorneyInfo>();
            ContactInfos = new HashSet<ContactInfo>();
        }
        public int TypeId { get; set; }
        public DicCustomerType Type { get; set; }
        public bool? IsSMB { get; set; }
        public string Xin { get; set; }
        public string Rnn { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string PhoneFax { get; set; }
        public string Email { get; set; }
        public string JurRegNumber { get; set; }
        public string ContactName { get; set; }
        public string ShortAddress { get; set; }
        public string Address { get; set; }
        public string AddressKz { get; set; }
        public string AddressEn { get; set; }
        public string Republic { get; set; }
        public string Region { get; set; }
        public string Oblast { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Subscript { get; set; }
        public string ApplicantsInfo { get; set; }
        public string CertificateSeries { get; set; }
        public string CertificateNumber { get; set; }
        public DateTimeOffset? RegDate { get; set; }
        public string Opf { get; set; }
        public string PowerAttorneyFullNum { get; set; }
        public DateTimeOffset? PowerAttorneyDateIssue { get; set; }
        public string NotaryName { get; set; }
        public string ShortDocContent { get; set; }
        public string NameKzLong { get; set; }
        public string NameRuLong { get; set; }
        public string NameEnLong { get; set; }
        public bool IsBeneficiary { get; set; }
        [NotMapped]
        public bool IsNotResident => Country?.Code != DicCountryCodes.Kazakhstan;
        public bool IsNotMention { get; set; }
        public string Apartment { get; set; }
        public int? BeneficiaryTypeId { get; set; }
        public DicBeneficiaryType BeneficiaryType { get; set; }
        public ICollection<CustomerAttorneyInfo> CustomerAttorneyInfos { get; set; }
        public ICollection<ContactInfo> ContactInfos { get; set; }

        public override string ToString()
        {
            return NameRu;
        }

        #region Relationships

        public int? CountryId { get; set; }
        public DicCountry Country { get; set; }

        #endregion

        public Type GetHistoryEntity()
        {
            return typeof(CustomerHistory);
        }
    }
}