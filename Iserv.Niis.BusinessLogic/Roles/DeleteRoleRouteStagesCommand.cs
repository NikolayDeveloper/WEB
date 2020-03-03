using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Collections.Generic;
using System.Linq;
using NetCoreDataAccess.Repository;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class DeleteRoleRouteStagesCommand : BaseCommand
    {
        public void Execute(IEnumerable<RoleRouteStageRelation> roleRouteStages, int[] stagesIds)
        {
            var repository = Uow.GetRepository<RoleRouteStageRelation>();

            var stagesForDelete = roleRouteStages.Where(rrs => !stagesIds.Contains(rrs.StageId)).ToList(); ;
            repository.DeleteRange(stagesForDelete);

            Uow.SaveChanges();
        }
    }
}
