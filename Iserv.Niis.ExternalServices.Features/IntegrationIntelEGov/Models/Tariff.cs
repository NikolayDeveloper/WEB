using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class Tariff
    {
        public int Id { get; set; }
        public int? TypeId { get; set; }
        public string TypeDescription { get; set; }
        public string Code { get; set; }
        public string NameEn { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public double Price { get; set; }
        public string Descript { get; set; }
        public string Limit { get; set; }
        public string ExternalId { get; set; }
    }
}
