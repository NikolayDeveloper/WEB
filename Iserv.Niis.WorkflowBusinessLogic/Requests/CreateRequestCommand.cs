using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Requests
{
    public class CreateRequestCommand: BaseCommand
    {
        public int Execute(Request request)
        {
            var repo = Uow.GetRepository<Request>();
            repo.Create(request);

            Uow.SaveChanges();

            return request.Id;
        }
    }
}
