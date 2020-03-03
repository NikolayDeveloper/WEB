using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetCountTextForPaySumArgument : SystemInfoMessage
    {
        public RefKey MainDocumentType { get; set; }
        public RefKey DocumentType { get; set; }
    }
}