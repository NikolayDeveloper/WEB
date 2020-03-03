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
    public class EgovPay
    {
        public string PayCode { get; set; }

        public decimal? Sum { get; set; }

        public DateTime? PayDate { get; set; }

        public string Xin { get; set; }

        public string Xml { get; set; }
    }
}
