using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetAttachedFileMetadataResult : SystemInfoMessage
    {
        public RefKey MainDocumentType { get; set; }
        public bool JurApplicantExists { get; set; }
        public bool IpApplicantExists { get; set; }
        public bool NonResidentApplicantExists { get; set; }

        public AttachedFileMetadata[] Data { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class AttachedFileMetadata
    {
        public RefKey AttachedFileType { get; set; } //тип документа
        public bool Required { get; set; }
        public bool Multiple { get; set; }
        public string[] Extensions { get; set; }
    }
}