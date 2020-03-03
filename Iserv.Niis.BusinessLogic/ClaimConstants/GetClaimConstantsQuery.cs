using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using Iserv.Niis.DataBridge.Implementations;

namespace Iserv.Niis.BusinessLogic.ClaimConstants
{
    public class GetClaimConstantsQuery : BaseQuery
    {
        public async Task<List<ClaimConstant>> ExecuteAsync()
        {
            var claimConstRepo = Uow.GetRepository<ClaimConstant>();

            return await claimConstRepo
                .AsQueryable()
                .OrderBy(x => x.NameRu)
                .ToListAsync();
        }
    }
}