using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Models
{
    [Serializable]
    [XmlType(Namespace = "http://niis-efilling-service.kz")]
    public class CorrespondenceAddress
    {
        public string CustomerName { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public ReferenceInfo CountryInfo { get; set; }

        public string Region { get; set; }

        public string Street { get; set; }

        public string Postcode { get; set; }
    }
}
