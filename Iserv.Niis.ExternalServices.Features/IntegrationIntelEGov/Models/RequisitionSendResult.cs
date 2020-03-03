using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class RequisitionSendResult : SystemInfoMessage
    {
        public int DocumentID { get; set; }
        public string DocumentNumber { get; set; }
        public int RequisitionStatus { get; set; }
    }
}