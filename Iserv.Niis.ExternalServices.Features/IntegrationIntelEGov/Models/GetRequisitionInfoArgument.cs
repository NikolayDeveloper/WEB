using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetRequisitionInfoArgument : SystemInfoMessage
    {
        public int DocumentID { get; set; }
    }
}