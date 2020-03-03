using Iserv.Niis.Model.Models.Subject;

namespace Iserv.Niis.Model.Models
{
    public class CustomerShortInfoDto
    {
        public int Id { get; set; }
        public string Xin { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameKz { get; set; }
        public string Address { get; set; }
        public string Republic { get; set; }
        public string Region { get; set; }
        public string Oblast { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int TypeId { get; set; }
        public string TypeCode { get; set; }
        public bool IsBeneficiary { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string PhoneFax { get; set; }
        public string Email { get; set; }
        public string JurRegNumber { get; set; }
        public string GovReg { get; set; }
        public string PowerAttorneyFullNum { get; set; }
        public int? CountryId { get; set; }
        public bool IsNotResident { get; set; }
        public bool IsNotMention { get; set; }
        public ContactInfoDto[] ContactInfos { get; set; }
        public string OwnerAddresseeAddress { get;set;}
        public string Apartment { get; set; }
        public int? BeneficiaryTypeId { get; set; }
    }
}