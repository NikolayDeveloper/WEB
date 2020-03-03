using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icis.Request
{
    public class GetICISRequestsByRequestIdAndIcisIdsQuery : BaseQuery
    {
        public async Task<List<ICISRequest>> ExecuteAsync(int requestId, List<int> icisIds)
        {
            var icisRequestRepo = Uow.GetRepository<ICISRequest>();

            return await icisRequestRepo
                .AsQueryable()
                .Where(ir => ir.RequestId == requestId && icisIds.Contains(ir.IcisId))
                .ToListAsync();
        }
    }
}