using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class File
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public ShepFile ShepFile { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class ShepFile
    {
        public string ID;
        public string Md5hash;
        public string Name;
    }
}