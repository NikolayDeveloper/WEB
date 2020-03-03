using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icfem.Request
{
    public class GetIcfemByRequestIdQuery: BaseQuery
    {
        public async Task<List<DicIcfemRequestRelation>> ExecuteAsync(int requestId)
        {
            var repository = Uow.GetRepository<DicIcfemRequestRelation>();

            return await repository.AsQueryable()
                .Where(pdci => pdci.RequestId == requestId)
                .ToListAsync();
        }
    }
}
