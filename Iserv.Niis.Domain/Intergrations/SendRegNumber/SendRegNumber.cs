using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations.SendRegNumber
{
    /// <summary>
    /// Информация о заявке
    /// </summary>
    public class SendRegNumber
    {
        /// <summary>
        /// Индентефикатор заявки
        /// </summary>
        [XmlElement(ElementName = "DocumentID", Namespace = "")]
        public int DocumentId { get; set; }

        /// <summary>
        /// Идентефикатор типа заявки
        /// </summary>
        [XmlElement(ElementName = "PatentTypeId", Namespace = "")]
        public int PatentTypeId { get; set; }

        /// <summary>
        /// Рег номер заявки
        /// </summary>
        [XmlElement(ElementName = "DocumentRegNumber", Namespace = "")]
        public string DocumentRegNumber { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        [XmlElement(ElementName = "ApplicationDate", Namespace = "")]
        public string ApplicationDate { get; set; }
    }
}