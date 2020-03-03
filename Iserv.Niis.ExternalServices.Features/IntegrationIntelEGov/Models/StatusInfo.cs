using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class StatusInfo
    {
        public string Code { get; set; }
        public string MessageRu { get; set; }
        public string MessageKz { get; set; }
    }
}