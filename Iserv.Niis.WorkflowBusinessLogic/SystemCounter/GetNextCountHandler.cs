using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.SystemCounter
{
    public class GetNextCountHandler: BaseHandler
    {
        public int Execute(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            var currentCounter = Executor.GetQuery<GetSystemCounterByCodeQuery>().Process(q => q.Execute(code));

            if (currentCounter != null)
            {
                currentCounter.Count = ++currentCounter.Count;
                Executor.GetCommand<UpdateSystemCounterCommand>().Process(c => c.Execute(currentCounter));
            }
            else
            {
                currentCounter = new Domain.Entities.System.SystemCounter {Code = code, Count = 1};
                Executor.GetCommand<CreateSystemCounterCommand>().Process(c => c.Execute(currentCounter));
            }

            return currentCounter.Count;
        }
    }
}
