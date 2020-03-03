using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.SystemCounter
{
    public class GetSystemCounterByCodeQuery: BaseQuery
    {
        public Domain.Entities.System.SystemCounter Execute(string code)
        {
            var repo = Uow.GetRepository<Domain.Entities.System.SystemCounter>();
            //Код счетчика должен быть уникальным
            return repo.AsQueryable().FirstOrDefault(c => c.Code == code);
        }
    }
}
