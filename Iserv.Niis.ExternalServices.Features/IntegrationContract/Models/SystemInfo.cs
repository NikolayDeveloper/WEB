using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Models
{
    [Serializable]
    [XmlType(Namespace = "http://niis-efilling-service.kz")]
    public class SystemInfo
    {
        public SenderInfo SenderInfo { get; set; }
        public StatusInfo StatusInfo { get; set; }
    }
}
