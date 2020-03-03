using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.AutoRouteStages
{
    /// <summary>
    /// Условия типа документа и выбора должности исполнителя
    /// </summary>
    public class AutoRouteStageEvent : Entity<int>, IHaveConcurrencyToken
    {
        /// <summary>
        /// Тип документа
        /// </summary>
        public int TypeId { get; set; }
        public DicDocumentType Type { get; set; }
        
        /// <summary>
        /// Должность исполнителя
        /// </summary>
        public int PositionId { get; set; }
        public DicPosition Position { get; set; }  
    }
}
