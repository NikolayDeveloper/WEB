using System;

namespace Iserv.Niis.Model.Models.Subject
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? OwnerId { get; set; }
        public int? RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleNameRu { get; set; }
        public int? CountryId { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameKz { get; set; }
        public int TypeId { get; set; }
        public string TypeNameRu { get; set; }
        public string Xin { get; set; }
        public string Phone { get; set; }
        public string PhoneFax { get; set; }
        public string Email { get; set; }
        public string CommonAddress { get; set; }
        public string ShortAddress { get; set; }
        public string Address { get; set; }
        public string AddressKz { get; set; }
        public string AddressEn { get; set; }
        public string Republic { get; set; }
        public string Region { get; set; }
        public string Oblast { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public bool? IsBeneficiary { get; set; }
        public string JurRegNumber { get; set; }
        public string GovReg { get; set; }
        public string MobilePhone { get; set; }
        public DateTimeOffset? DateBegin { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        public string PowerAttorneyFullNum { get; set; }
        public bool? IsNotResident { get; set; }
        public bool? IsNotMention { get; set; }
        public string Apartment { get; set; }
        public int? BeneficiaryTypeId { get; set; }
        public ContactInfoDto[] ContactInfos { get; set; }
        public int DisplayOrder { get; set; }
    }
}