using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Информация об эксперте
    /// </summary>
    public class ExpertInfo
    {
        /// <summary>
        /// Имя
        /// </summary>
        [XmlElement(ElementName = "Name", Namespace = "")]
        public string Name { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        [XmlElement(ElementName = "Phone", Namespace = "")]
        public string Phone { get; set; }
    }
}
