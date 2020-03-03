using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class DicBiblioChangeTypeDicRouteStageRelation: Entity<int>
    {
        /// <summary>
        /// Идентификатор режима внесения изменений
        /// </summary>
        public int ChangeTypeId { get; set; }
        /// <summary>
        /// Режим внесения изменений
        /// </summary>
        public DicBiblioChangeType ChangeType { get; set; }
        /// <summary>
        /// Идентификатор этапа, на котором доступен режим
        /// </summary>
        public int StageId { get; set; }
        /// <summary>
        /// Этап, на котором доступен режим
        /// </summary>
        public DicRouteStage Stage { get; set; }
    }
}
