using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Other
{
    /// <summary>
    /// Связка этапов (порядок этапов)
    /// </summary>
    public class RouteStageOrder : Entity<int>
    {
        public int CurrentStageId { get; set; }
        public DicRouteStage CurrentStage { get; set; }
        public int NextStageId { get; set; }
        public DicRouteStage NextStage { get; set; }
        public bool IsAutomatic { get; set; }
        public bool IsParallel { get; set; }
        public bool IsReturn { get; set; }

        /// <summary>
        /// Класификация документов
        /// </summary>
        public int? ClassificationId { get; set; }
        public DicDocumentClassification Classification { get; set; }
    }
}