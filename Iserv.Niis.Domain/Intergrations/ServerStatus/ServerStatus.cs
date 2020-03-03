using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Статус результата
    /// </summary>
    public class ServerStatus
    {
        /// <summary>
        /// Сообщение результата
        /// </summary>
        [XmlElement(ElementName = "Message", Namespace = "")]
        public string Message { get; set; }
        
        /// <summary>
        /// Код результата
        /// </summary>
        [XmlElement(ElementName = "Code", Namespace = "")]
        public int Code { get; set; }
    }
}