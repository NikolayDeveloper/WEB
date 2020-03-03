using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Базовый ответ на запрос ЛК
    /// </summary>
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class EnvelopeResponce
    {
        /// <summary>
        /// Пустой объект шапки ответа
        /// </summary>
        [XmlElement(ElementName = "Header")]
        public object Header { get; set; }

        /// <summary>
        /// Тело ответа
        /// </summary>
        [XmlElement(ElementName = "Body")]
        public ServerStatusBody Body { get; set; }
    }
}