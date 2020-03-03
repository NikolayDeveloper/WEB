using System;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models
{
    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class GetReferenceArgument
    {
        public string Password { get; set; }
    }
}
