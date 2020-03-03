using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetPayTarifArgument : SystemInfoMessage
    {
        public int PatentType { get; set; }
        public string PatentN { get; set; }
    }
}