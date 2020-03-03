using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Справочник типов режимов внесения изменений в биб. данные
    /// </summary>
    public class DicBiblioChangeType: DictionaryEntity<int>
    {
        public DicBiblioChangeType()
        {
            Stages = new HashSet<DicBiblioChangeTypeDicRouteStageRelation>();
        }

        /// <summary>
        /// Этапы, на котором доступен режим
        /// </summary>
        public ICollection<DicBiblioChangeTypeDicRouteStageRelation> Stages { get; set; }
    }
}
