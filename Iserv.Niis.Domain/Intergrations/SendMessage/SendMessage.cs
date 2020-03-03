using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Переписка
    /// </summary>
    public class SendMessage
    {
        /// <summary>
        /// Индентефикатор заявки
        /// </summary>
        [XmlElement(ElementName = "DocumentID", Namespace = "")]
        public int? DocumentId { get; set; }

        /// <summary>
        /// Идентефикатор типа заявки
        /// </summary>
        [XmlElement(ElementName = "PatentTypeId", Namespace = "")]
        public int? PatentTypeId { get; set; }

        /// <summary>
        /// Штрихкод ходатайства на получение выписки
        /// </summary>
        [XmlElement(ElementName = "AnswerDocId", Namespace = "")]
        public int? AnswerDocId { get; set; }

        /// <summary>
        /// Информация по документу переписки
        /// </summary>
        [XmlElement(ElementName = "MessageInfo", Namespace = "")]
        public MessageInfo MessageInfo { get; set; }

        /// <summary>
        /// Информация об эксперте
        /// </summary>
        [XmlElement(ElementName = "ExpertInfo", Namespace = "")]
        public ExpertInfo ExpertInfo { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        [XmlElement(ElementName = "File", Namespace = "")]
        public File File { get; set; }
    }
}
