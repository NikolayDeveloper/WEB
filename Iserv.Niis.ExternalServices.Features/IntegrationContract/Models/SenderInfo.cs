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
    public class SenderInfo
    {
        public string AdditionalInfo { get; set; }

        public string ChainId { get; set; }

        public DateTime MessageDate { get; set; }

        public string MessageId { get; set; }

        public string Sender { get; set; }
    }
}
