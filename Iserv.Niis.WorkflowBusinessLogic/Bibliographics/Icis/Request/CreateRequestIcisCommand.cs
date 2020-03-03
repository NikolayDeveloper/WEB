using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icis.Request
{
    public class CreateRequestIcisCommand: BaseCommand
    {
        public int Execute(ICISRequest icisRequest)
        {
            var repo = Uow.GetRepository<ICISRequest>();
            repo.Create(icisRequest);
            Uow.SaveChanges();
            return icisRequest.Id;
        }
    }
}
