using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetPayTarifResult : SystemInfoMessage
    {
        public PayTarifResult Result { get; set; }
        public string Message { get; set; }
        public int PatentUID { get; set; }
        public string PatentName { get; set; }
        public DateTime? Validity { get; set; }
        public DateTime? RegDate { get; set; }
        public DateTime? DateTerminate { get; set; }
        public string Status { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public enum PayTarifResult
    {
        TarifFound,
        Info,
        Error
    }
}