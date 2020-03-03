using System;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models
{
    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public abstract class SystemInfoMessage
    {
        public SystemInfo SystemInfo { get; set; }
    }
}
