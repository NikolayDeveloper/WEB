using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetMessageFileResult : SystemInfoMessage
    {
        public int MainDocumentID { get; set; }
        public int DocumentID { get; set; }
        public RefKey CorrespondenceType { get; set; }
        public string DocNumber { get; set; }
        public DateTime? DocDate { get; set; }
        public int PageCount { get; set; }
        public File File { get; set; }
    }
}