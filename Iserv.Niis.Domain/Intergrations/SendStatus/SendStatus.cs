using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{ 
    /// <summary>
    /// Информация о заявке
    /// </summary>
    public class SendStatus
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
        /// Идентефикатор внешнего статуса заявка 
        /// </summary>
        [XmlElement(ElementName = "StatusId", Namespace = "")]
        public int StatusId { get; set; }
    }
}