using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class UserRouteStageRelation
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int StageId { get; set; }
        public DicRouteStage Stage { get; set; }
    }
}