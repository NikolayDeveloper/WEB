using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Model.BusinessLogic
{
    public class GetDicIpcsByParentIdQuery: BaseQuery
    {
        public List<DicIPC> Execute(int parentId)
        {
            var repo = Uow.GetRepository<DicIPC>();
            var ipcChildren = repo.AsQueryable()
                .Where(r => r.ParentId == parentId);

            return ipcChildren.ToList();
        }
    }
}
