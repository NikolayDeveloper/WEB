using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage
{
    public class GetDicRouteStageByCodeQuery : BaseQuery
    {
        public Domain.Entities.Dictionaries.DicRouteStage Execute(string code)
        {
            var dicRouteStageRepository = Uow.GetRepository<Domain.Entities.Dictionaries.DicRouteStage>();
            var stage = dicRouteStageRepository.AsQueryable().FirstOrDefault(r => r.IsDeleted == false && r.Code == code);

            return stage;
        }
    }
}