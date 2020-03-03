using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.Request
{
    public class GetColorTzsByRequestIdQuery: BaseQuery
    {
        public async Task<List<DicColorTZRequestRelation>> ExecuteAsync(int requestId)
        {
            var repository = Uow.GetRepository<DicColorTZRequestRelation>();

            return await repository.AsQueryable()
                .Where(ctz => ctz.RequestId == requestId)
                .ToListAsync();
        }
    }
}
