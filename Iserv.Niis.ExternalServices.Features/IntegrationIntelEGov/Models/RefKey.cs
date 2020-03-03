using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class RefKey
    {
        public int UID { get; set; }
        public string Note { get; set; }


        public override bool Equals(object obj)
        {
            return UID == (obj as RefKey).UID;
        }
    }
}