using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.Request
{
    public class GetColorTzsByRequestIdAndColorIdsQuery : BaseQuery
    {
        public async Task<List<DicColorTZRequestRelation>> ExecuteAsync(int requestId, List<int> colorTzIds)
        {
            var colorTzRequestRelationRepo = Uow.GetRepository<DicColorTZRequestRelation>();

            return await colorTzRequestRelationRepo.AsQueryable()
                .Where(ctz => ctz.RequestId == requestId && colorTzIds.Contains(ctz.ColorTzId))
                .ToListAsync();
        }
    }
}