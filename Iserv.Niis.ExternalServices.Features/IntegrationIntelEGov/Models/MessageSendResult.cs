using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class MessageSendResult : SystemInfoMessage
    {
        public int DocumentUID { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public int? MessageStatus { get; set; }

        public int PaymentDocumentUID { get; set; }
        public string PaymentDocumentNumber { get; set; }
        public DateTime? PaymentDocumentDate { get; set; }
    }
}