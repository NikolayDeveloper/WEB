using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetRequisitionInfoResult : SystemInfoMessage
    {
        public int DocumentID { get; set; }
        public File Image { get; set; }
        public Applicant[] ApplicantList { get; set; }
    }


    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class Applicant
    {
        public int CustomerId { get; set; }
        public string XIN { get; set; }
        public string Name { get; set; }
    }
}