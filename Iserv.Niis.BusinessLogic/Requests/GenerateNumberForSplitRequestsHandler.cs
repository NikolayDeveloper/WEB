using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GenerateNumberForSplitRequestsHandler: BaseHandler
    {
        public async Task ExecuteAsync(Request oldRequest, Request newRequest)
        {
            if (newRequest == null || oldRequest == null)
            {
                throw new ArgumentNullException(nameof(newRequest));
            }

            var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(oldRequest.RequestNum));
            newRequest.RequestNum = $"{oldRequest.RequestNum}{Convert.ToChar(count + 64)}";
        }
    }
}
