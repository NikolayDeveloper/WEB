using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class CheckPatentStatementResult : SystemInfoMessage
    {
        public bool IsSuccess { get; set; }
        public string StatusRu { get; set; }
        public string StatusKz { get; set; }
        public File ResultFile { get; set; }
    }
}