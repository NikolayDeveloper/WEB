using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class Customer
    {
        public string ShortDocContent { get; set; }
        public string NotaryName { get; set; }
        public DateTime PowerAttorneyDateIssue { get; set; }
        public string PowerAttorneyFullNum { get; set; }
        public string ApplicantsInfo { get; set; }
        public DateTime AttorneyCertDate { get; set; }
        public string IndustrialProperty { get; set; }
        public string AttorneyCertNum { get; set; }

        public RefKey PatentLinkType { get; set; }
        public bool Mention { get; set; }

        public RefKey Type { get; set; }
        public string Xin { get; set; }
        public string Opf { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string CertificateSeries { get; set; }
        public string CertificateNumber { get; set; }
        public DateTime RegDate { get; set; }

        public RefKey AdrCountry { get; set; }
        public string AdrObl { get; set; }
        public string AdrStreet { get; set; }
        public string AdrPostCode { get; set; }

        public RefKey Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }
}