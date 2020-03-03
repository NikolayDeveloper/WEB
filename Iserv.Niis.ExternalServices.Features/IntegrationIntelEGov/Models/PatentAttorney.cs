using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class PatentAttorney
    {
        public int Id { get; set; }
        public int? AttorneyId { get; set; }
        public string Xin { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string CertificateNumber { get; set; }
        public DateTime? CertificateDate { get; set; }
        public int Status { get; set; }
        public string RevalidNote { get; set; }
        public string Activites { get; set; }
        public string Knowledge { get; set; }
        public string PlaceOfWork { get; set; }
        public string Languages { get; set; }
        public string PartOfTheWorld { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string AddressEn { get; set; }
        public string AddressRu { get; set; }
        public string AddressKz { get; set; }
        public string Postcode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string ExternalId { get; set; }
    }
}
