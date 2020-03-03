using System;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models
{
    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class SystemInfo
    {
        public string MessageId { get; set; }
        public string ChainId { get; set; }
        public DateTime MessageDate { get; set; }
        public string Sender { get; set; }
        public StatusInfo Status { get; set; }
        public string AdditionalInfo { get; set; }
    }
}