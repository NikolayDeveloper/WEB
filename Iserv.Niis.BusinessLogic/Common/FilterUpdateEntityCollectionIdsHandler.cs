using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Common
{
    public class FilterUpdateEntityCollectionIdsHandler: BaseHandler
    {
        public List<int> Execute<T>(List<int> idsToFilter, List<T> entities) where T: Entity<int>
        {
            var addIds = Executor.GetHandler<FilterAddEntityCollectionIdsHandler>().Process(h => h.Execute(idsToFilter, entities));
            var removeIds = Executor.GetHandler<FilterRemoveEntityCollectionIdsHandler>()
                .Process(h => h.Execute(idsToFilter, entities));

            return entities.Where(c => !addIds.Contains(c.Id) && !removeIds.Contains(c.Id)).Select(c => c.Id).ToList();
        }
    }
}
