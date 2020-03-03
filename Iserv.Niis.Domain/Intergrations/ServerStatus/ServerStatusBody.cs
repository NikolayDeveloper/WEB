using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Тело ответа
    /// </summary>
    public class ServerStatusBody
    {
        /// <summary>
        /// Статус результата
        /// </summary>
        [XmlElement(ElementName = "ServerStatus", Namespace = "")]
        public ServerStatus ServerStatus { get; set; }
    }
}