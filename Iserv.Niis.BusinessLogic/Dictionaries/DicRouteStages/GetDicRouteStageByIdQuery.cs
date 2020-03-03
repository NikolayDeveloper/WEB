using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetDicRouteStageByIdQuery : BaseQuery
    {
        public DicRouteStage Execute(int routeStateId)
        {
            var dicRouteStageRepository = Uow.GetRepository<DicRouteStage>();
            return dicRouteStageRepository.GetById(routeStateId);
        }
    }
}
