using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Common
{
    public class FilterRemoveIntegerListIdsHandler: BaseHandler
    {
        public List<int> Execute(List<int> idsToFilter, List<int> entityIdList)
        {
            return entityIdList.Where(c => !idsToFilter.Contains(c)).ToList();
        }
    }
}
