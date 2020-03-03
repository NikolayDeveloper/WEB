using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetRequisitionListForPaymentArgument : SystemInfoMessage
    {
        public string XIN { get; set; }
        public RefKey PatentType { get; set; }
        public RefKey DocumentType { get; set; }
    }
}