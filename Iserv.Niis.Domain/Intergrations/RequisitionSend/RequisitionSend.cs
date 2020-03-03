using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations.RequisitionSend
{
    public class RequisitionSend
    {
        [XmlElement(ElementName = "argument", Namespace = "")]
        public Argument Argument { get; set; }
    }
}