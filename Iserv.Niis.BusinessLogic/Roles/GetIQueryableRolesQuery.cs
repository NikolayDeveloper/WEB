using System.Linq;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class GetIQueryableRolesQuery : BaseQuery
    {
        public IQueryable<ApplicationRole> Execute()
        {
            var roleRepo = Uow.GetRepository<ApplicationRole>();
            return roleRepo
                .AsQueryable()
                .AsNoTracking()
                .Include(r => r.Stages);
        }
    }
}
