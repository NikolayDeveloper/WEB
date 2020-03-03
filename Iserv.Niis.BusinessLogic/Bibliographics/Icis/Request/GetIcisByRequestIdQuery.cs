using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icis.Request
{
    public class GetIcisByRequestIdQuery: BaseQuery
    {
        public async Task<List<ICISRequest>> ExecuteAsync(int requestId)
        {
            var repository = Uow.GetRepository<ICISRequest>();

            return await repository.AsQueryable()
                .Where(i => i.RequestId == requestId)
                .ToListAsync();
        }
    }
}
