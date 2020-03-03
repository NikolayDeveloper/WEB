using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class CreateRoleRouteStagesCommand : BaseCommand
    {
        public void Execute(IEnumerable<RoleRouteStageRelation> roleRouteStages, int[] stagesIds, int roleId)
        {
            var repository = Uow.GetRepository<RoleRouteStageRelation>();

            var stagesForCreate = stagesIds
                .Where(id => !roleRouteStages.Select(rrs => rrs.StageId).Contains(id))
                .Select(stageId => new RoleRouteStageRelation
                {
                    RoleId = roleId,
                    StageId = stageId
                })
                .ToList();

            repository.CreateRange(stagesForCreate);

            Uow.SaveChanges();
        }
    }
}
