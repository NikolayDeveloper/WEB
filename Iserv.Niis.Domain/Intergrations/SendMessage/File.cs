using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Файл
    /// </summary>
    public class File
    {
        /// <summary>
        /// Наименование
        /// </summary>
        [XmlElement(ElementName = "Name", Namespace = "")]
        public string Name { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        [XmlElement(ElementName = "Content", Namespace = "")]
        public string Content { get; set; }
    }
}
