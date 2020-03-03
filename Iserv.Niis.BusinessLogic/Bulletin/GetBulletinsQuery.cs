using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class GetBulletinsQuery: BaseQuery
    {
        public async Task<List<Domain.Entities.Bulletin.Bulletin>> ExecuteAsync()
        {
            var repo = Uow.GetRepository<Domain.Entities.Bulletin.Bulletin>();
            var result = repo.AsQueryable();

            return await result.ToListAsync();
        }
    }
}
