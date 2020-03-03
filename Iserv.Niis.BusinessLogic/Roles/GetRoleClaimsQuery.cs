using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class GetRoleClaimsQuery : BaseQuery
    {
        public async Task<List<IdentityRoleClaim<int>>> ExecuteAsync()
        {
            var roleClaimRepo = Uow.GetRepository<IdentityRoleClaim<int>>();
            return await roleClaimRepo
                .AsQueryable()
                .ToListAsync();
        }
    }
}