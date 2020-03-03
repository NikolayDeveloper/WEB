using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class EarlyReg
    {
        public RefKey EarlyTypeId { get; set; }
        public string Num { get; set; }
        public DateTime? Date { get; set; }
        public RefKey Country { get; set; }
        public string StageReview { get; set; }
        public string Name { get; set; }
    }
}