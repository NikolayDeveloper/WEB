using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations.SendRegNumber
{ 
    /// <summary>
    /// Тело отправки номера заявки
    /// </summary>
    public class SendRegNumberBody
    {
        /// <summary>
        /// Информация о заявке
        /// </summary>
        [XmlElement(ElementName = "input", Namespace = "lktest.kazpatent.kz/18kz_niip/sendRegNumber")]
        public SendRegNumber Input { get; set; }
    }
}