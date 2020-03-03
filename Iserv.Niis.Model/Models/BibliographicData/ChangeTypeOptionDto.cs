using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Model.Models.BibliographicData
{
    public class ChangeTypeOptionDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string NameRu { get; set; }
        /// <summary>
        /// Типы внесений изменений
        /// </summary>
        public ChangeType[] Types { get; set; }
        /// <summary>
        /// Код этапа, начиная с которого изменение не доступно
        /// </summary>
        public string StageCode { get; set; }
    }
}
