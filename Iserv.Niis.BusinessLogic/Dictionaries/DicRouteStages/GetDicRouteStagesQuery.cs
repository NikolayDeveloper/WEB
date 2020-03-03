using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetDicRouteStagesQuery : BaseQuery
    {
        public async Task<List<DicRouteStage>> ExecuteAsync()
        {
            var routeStageRepo = Uow.GetRepository<DicRouteStage>();
            return await routeStageRepo
                .AsQueryable()
                .ToListAsync();
        }
    }
}