using System;
using System.Web;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Models
{
    [Serializable]
    [XmlType(Namespace = "http://niis-efilling-service.kz")]
    public class Addressee
    {
        public ReferenceInfo CustomerType { get; set; }

        public string Xin { get; set; }

        private string _customerName;
        public string CustomerName
        {
            get
            {
                return _customerName;
            }
            set
            {
                _customerName = HttpUtility.HtmlDecode(value);
            }
        }

        public string Login { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public ReferenceInfo CountryInfo { get; set; }

        public string Region { get; set; }

        public string Street { get; set; }
    }
}
