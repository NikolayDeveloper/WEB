using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class GetRoleRouteStageByRoleIdQyery : BaseQuery
    {
        public List<RoleRouteStageRelation> Execute(int roleId)
        {
            var repository = Uow.GetRepository<RoleRouteStageRelation>();

            return repository.AsQueryable()
                .Where(x => x.RoleId == roleId).ToList();
        }
    }
}
