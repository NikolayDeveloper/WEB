using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations.RequisitionSend
{
    /// <summary>
    /// Тело отправки  заявки
    /// </summary>
    public class RequisitionSendBody
    {
        /// <summary>
        /// Информация о заявке
        /// </summary>
        [XmlElement(ElementName = "input", Namespace = "lktest.kazpatent.kz/18kz_niip/RequisitionSend")]
        public RequisitionSend Input { get; set; }
    }
}