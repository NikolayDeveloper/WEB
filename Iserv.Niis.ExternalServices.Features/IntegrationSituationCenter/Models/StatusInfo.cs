using System;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models
{
    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class StatusInfo
    {
        public string Code { get; set; }
        public string MessageRu { get; set; }
        public string MessageKz { get; set; }
    }
}