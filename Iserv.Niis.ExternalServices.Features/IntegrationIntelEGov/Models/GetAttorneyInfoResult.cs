using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetAttorneyInfoResult : SystemInfoMessage
    {
        public string IIN { get; set; }
        public string NameLast { get; set; }
        public string NameFirst { get; set; }
        public string NameMiddle { get; set; }
        public string CertNum { get; set; }
        public DateTime CertDate { get; set; }
        public bool Active { get; set; }
        public string RevalidNote { get; set; }
        public string Ops { get; set; }
        public RefKey CountryId { get; set; }
        public RefKey LocationId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }
}