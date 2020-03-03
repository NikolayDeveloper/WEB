using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Тело отправки документа переписки
    /// </summary>
    public class SendMessageBody
    {
        /// <summary>
        /// Информация о заявке
        /// </summary>
        [XmlElement(ElementName = "input", Namespace = "lktest.kazpatent.kz/18kz_niip/sendMessage")]
        public SendMessage Input { get; set; }
    }
}
