using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.AutoRouteStages
{
    public class RequestAutoRouteStageExecutor: Entity<int>
    {
        public int StageId { get; set; }
        public DicRouteStage Stage { get; set; }
        public int PositionId { get; set; }
        public DicPosition Position { get; set; }
    }
}
