using System;
using System.Web;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Models
{
    [Serializable]
    [XmlType(Namespace = "http://niis-efilling-service.kz")]
    public class Customer
    {
        public string ShortDocContent { get; set; }

        public string NotaryName { get; set; }

        public DateTime? PowerAttorneyDateIssue { get; set; }

        public string PowerAttorneyFullNum { get; set; }

        public string ApplicantsInfo { get; set; }

        public DateTime? AttorneyCertDate { get; set; }

        public string IndustrialProperty { get; set; }

        public string AttorneyCertNum { get; set; }

        public ReferenceInfo CustomerRole { get; set; }

        public bool Mention { get; set; }

        public ReferenceInfo CustomerType { get; set; }

        public string Xin { get; set; }

        public string Opf { get; set; }

        private string _nameEn;
        public string NameEn
        {
            get
            {
                return _nameEn;
            }
            set
            {
                _nameEn = HttpUtility.HtmlDecode(value);
            }
        }
        private string _nameRu;
        public string NameRu
        {
            get
            {
                return _nameRu;
            }
            set
            {
                _nameRu = HttpUtility.HtmlDecode(value);
            }
        }
        private string _nameKz;
        public string NameKz
        {
            get
            {
                return _nameKz;
            }
            set
            {
                _nameKz = HttpUtility.HtmlDecode(value);
            }
        }

        public string CertificateSeries { get; set; }

        public string CertificateNumber { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public ReferenceInfo AddressCountry { get; set; }

        public string Region { get; set; }

        public string Street { get; set; }

        public string Index { get; set; }

        public ReferenceInfo Country { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }
    }
}
