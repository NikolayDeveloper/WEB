using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocSubTypes
{
    public class GetDicProtectionDocSubtypeByIdQuery: BaseQuery
    {
        public DicProtectionDocSubType Execute(int id)
        {
            var repo = Uow.GetRepository<DicProtectionDocSubType>();

            return repo.AsQueryable()
                .FirstOrDefault(d => d.Id == id);
        }
    }
}
