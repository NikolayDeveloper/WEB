using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Базовый запрос ЛК
    /// </summary>
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    { 
        /// <summary>
        /// Пустой объект шапки запроса
        /// </summary>
        [XmlElement(ElementName = "Header")]
        public object Header { get; set; }

        /// <summary>
        /// Тело запроса(Динамика)
        /// </summary>
        [XmlElement(ElementName = "Body")]
        public object Body { get; set; }
    }
}