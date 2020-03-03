using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Тело отправки статуса заявки
    /// </summary>
    public class SendStatusBody
    {
        /// <summary>
        /// Информация о заявке
        /// </summary>
        [XmlElement(ElementName = "input", Namespace = "lktest.kazpatent.kz/18kz_niip/sendStatus")]
        public SendStatus Input { get; set; }
    }
}