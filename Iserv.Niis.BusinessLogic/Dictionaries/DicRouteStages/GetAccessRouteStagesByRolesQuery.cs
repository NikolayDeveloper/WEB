using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetAccessRouteStagesByRolesQuery : BaseQuery
    {
        public List<DicRouteStage> Execute(IEnumerable<string> roleNames)
        {
            var repository = Uow.GetRepository<RoleRouteStageRelation>();
            var roles = Uow.GetRepository<ApplicationRole>().AsQueryable().Where(r => roleNames.Contains(r.Name)).ToList();

            var result = repository.AsQueryable()
                .Where(rr => roles.Select(r => r.Id).Contains(rr.RoleId))
                .Select(rr => rr.Stage)
                .Include(s => s.Route)
                .Distinct()
                .ToList();

            return result;
        }
    }
}
