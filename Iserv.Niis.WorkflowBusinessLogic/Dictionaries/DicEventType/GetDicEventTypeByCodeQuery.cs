using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicEventType
{
    public class GetDicEventTypeByCodeQuery: BaseQuery
    {
        public Domain.Entities.Dictionaries.DicEventType Execute(string code)
        {
            var repo = Uow.GetRepository<Domain.Entities.Dictionaries.DicEventType>();
            var type = repo.AsQueryable().FirstOrDefault(t => t.Code == code);

            return type;
        }
    }
}
