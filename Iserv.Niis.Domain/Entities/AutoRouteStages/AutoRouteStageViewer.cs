using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.AutoRouteStages
{
    /// <summary>
    /// Сотрудник который может просматривать документ на этапе
    /// </summary>
    public class AutoRouteStageViewer : Entity<int>, IHaveConcurrencyToken
    {
        /// <summary>
        /// Объекто автомаршрутизации этапов по тригеру
        /// </summary>
        public int AutoRouteStageId { get; set; }
        public AutoRouteStage AutoRouteStage { get; set; }

        /// <summary>
        /// Должность сотрудника
        /// </summary>
        public int PositionId { get; set; }
        public DicPosition Position { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public int TypeId { get; set; }
        public DicProtectionDocType Type { get; set; }
    }
}
