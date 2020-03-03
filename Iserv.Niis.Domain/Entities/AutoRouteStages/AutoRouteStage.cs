using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using System.Collections.Generic;

namespace Iserv.Niis.Domain.Entities.AutoRouteStages
{
    /// <summary>
    /// Объекто автомаршрутизации этапов по тригеру
    /// </summary>
    public class AutoRouteStage : Entity<int>, IHaveConcurrencyToken
    {
        public AutoRouteStage()
        {
            AutoRouteStageEvents = new HashSet<AutoRouteStageEvent>();
            AutoRouteStageViewers = new HashSet<AutoRouteStageViewer>();
        }

        public string Code { get; set; }

        /// <summary>
        /// Текуший этап
        /// </summary>
        public int? CurrentStageId { get; set; }
        public DicRouteStage CurrentStage { get; set; }

        /// <summary>
        /// Следующий этап
        /// </summary>
        public int NextStageId { get; set; }
        public DicRouteStage NextStage { get; set; }

        /// <summary>
        /// Условия типа документа и выбора должности исполнителя
        /// </summary>
        public ICollection<AutoRouteStageEvent> AutoRouteStageEvents { get; set; }

        /// <summary>
        /// Список сотрудников с доступом на просмотр документа
        /// </summary>
        public ICollection<AutoRouteStageViewer> AutoRouteStageViewers { get; set; }
    }
}
