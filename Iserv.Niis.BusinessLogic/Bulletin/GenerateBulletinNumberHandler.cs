using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class GenerateBulletinNumberHandler: BaseHandler
    {
        public void Execute(Domain.Entities.Bulletin.Bulletin bulletin)
        {
            var counter = Executor.GetHandler<GetNextCountHandler>()
                .Process(h => h.Execute(NumberGenerator.BulletinCode));
            bulletin.Number = counter.ToString();
        }
    }
}
