using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetRouteStagesByCodesQuery: BaseQuery
    {
        public async Task<List<DicRouteStage>> ExecuteAsync(string[] codes)
        {
            var repo = Uow.GetRepository<DicRouteStage>();
            var stages = await repo.AsQueryable()
                .Where(s => codes.Contains(s.Code))
                .ToListAsync();

            return stages;
        }
    }
}
