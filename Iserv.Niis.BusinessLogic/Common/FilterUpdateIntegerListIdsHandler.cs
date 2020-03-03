using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Common
{
    public class FilterUpdateIntegerListIdsHandler: BaseHandler
    {
        public List<int> Execute(List<int> idsToFilter, List<int> entityIdList)
        {
            var addIds = Executor.GetHandler<FilterAddIntegerListIdsHandler>().Process(h => h.Execute(idsToFilter, entityIdList));
            var removeIds = Executor.GetHandler<FilterRemoveIntegerListIdsHandler>()
                .Process(h => h.Execute(idsToFilter, entityIdList));

            return entityIdList.Where(c => !addIds.Contains(c) && !removeIds.Contains(c)).ToList();
        }
    }
}
