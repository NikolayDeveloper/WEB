using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class EGovPay
    {
        //GG1351HLHND7
        public string PayCode { get; set; }

        public decimal Sum { get; set; }
        public DateTime Date { get; set; }
        public string XIN { get; set; }

        public string XML { get; set; }
    }
}