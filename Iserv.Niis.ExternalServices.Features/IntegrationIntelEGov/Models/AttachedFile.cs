using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class AttachedFile
    {
        public RefKey Type { get; set; }
        public int PageCount { get; set; }
        public File File { get; set; }
    }
}