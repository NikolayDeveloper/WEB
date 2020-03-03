using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRoutes
{
    public class GetDicRouteIdByStageId : BaseQuery
    {
        public int? Execute(int stageId)
        {
            var routeRepo = Uow.GetRepository<DicRouteStage>();
            return routeRepo
                .GetById(stageId).RouteId;
        }
    }
}