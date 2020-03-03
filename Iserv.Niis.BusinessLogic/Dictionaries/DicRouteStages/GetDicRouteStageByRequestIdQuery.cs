using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetDicRouteStageByRequestIdQuery: BaseQuery
    {
        public async Task<List<DicRouteStage>> ExecuteAsync(int requestId)
        {
            var requestRepo = Uow.GetRepository<Request>();
            var request = await requestRepo.AsQueryable()
                .Include(r => r.ProtectionDocType)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new DataNotFoundException(nameof(Request), DataNotFoundException.OperationType.Read, requestId);

            var routeStagesRepo = Uow.GetRepository<DicRouteStage>();
            var routeStages = await routeStagesRepo.AsQueryable()
                .Where(rs => rs.RouteId == request.ProtectionDocType.RouteId && rs.IsDeleted == false)
                .ToListAsync();

            return routeStages;
        }
    }
}
