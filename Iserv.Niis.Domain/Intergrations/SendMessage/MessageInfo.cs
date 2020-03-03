using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Информация по документу переписки
    /// </summary>
    public class MessageInfo
    {
        /// <summary>
        /// Идентификатор типа документа переписки
        /// </summary>
        [XmlElement(ElementName = "Id", Namespace = "")]
        public int Id { get; set; }

        /// <summary>
        /// Штрихкод документа переписки
        /// </summary>
        [XmlElement(ElementName = "DocumentID", Namespace = "")]
        public int DocumentId { get; set; }

        /// <summary>
        /// Идентефикатор типа заявки
        /// </summary>
        [XmlElement(ElementName = "DocNumber", Namespace = "")]
        public string DocNumber { get; set; }

        /// <summary>
        /// Штрихкод ходатайства на получение выписки
        /// </summary>
        [XmlElement(ElementName = "DocDate", Namespace = "")]
        public string DocDate { get; set; }
    }
}
