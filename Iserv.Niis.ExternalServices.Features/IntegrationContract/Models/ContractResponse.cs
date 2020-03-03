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
    public class ContractResponse : SystemInfo
    {
        public int DocumentId { get; set; }

        public string DocumentNumber { get; set; }

        public int RequisitionStatus { get; set; }
    }
}
