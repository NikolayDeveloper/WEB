using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class RoleRouteStageRelation
    {
        public int RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        public int StageId { get; set; }
        public DicRouteStage Stage { get; set; }
    }
}