using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetAttachedFileMetadataArgument : SystemInfoMessage
    {
        public RefKey MainDocumentType { get; set; }
        public bool JurApplicantExists { get; set; }
        public bool IpApplicantExists { get; set; }
        public bool NonResidentApplicantExists { get; set; }
    }
}