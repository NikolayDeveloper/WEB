using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Common
{
    public class FilterAddIntegerListIdsHandler: BaseHandler
    {
        public List<int> Execute(List<int> idsToFilter, List<int> entityIdList)
        {
            return idsToFilter.Where(id => !entityIdList.Contains(id)).ToList();
        }
    }
}
