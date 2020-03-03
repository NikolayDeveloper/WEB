using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Common
{
    public class FilterRemoveEntityCollectionIdsHandler: BaseHandler
    {
        public List<int> Execute<T>(List<int> idsToFilter, List<T> entities) where T: Entity<int>
        {
            return entities.Where(c => !idsToFilter.Contains(c.Id)).Select(c => c.Id).ToList();
        }
    }
}
