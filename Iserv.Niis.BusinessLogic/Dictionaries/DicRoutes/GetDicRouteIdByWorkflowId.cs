using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRoutes
{
    public class GetDicRouteIdByWorkflowId : BaseQuery
    {
        public int? Execute(int workflowId)
        {
            var routeRepo = Uow.GetRepository<DocumentWorkflow>();
            return routeRepo
                .GetById(workflowId).RouteId;
        }
    }
}