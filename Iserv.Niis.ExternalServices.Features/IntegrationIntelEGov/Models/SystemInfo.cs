using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class SystemInfo
    {
        public string MessageId { get; set; }
        public string ChainId { get; set; }
        public DateTime MessageDate { get; set; }
        public string Sender { get; set; }
        public StatusInfo Status { get; set; }
        public string AdditionalInfo { get; set; }
    }
}