using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.RequestWorkflows
{
    public class CreateRequestWorkflowCommand: BaseCommand
    {
        public int Execute(RequestWorkflow requestWorkflow)
        {
            var requestWorkflowRepository = Uow.GetRepository<RequestWorkflow>();
            requestWorkflowRepository.Create(requestWorkflow);
            Uow.SaveChanges();
            return requestWorkflow.Id;
        }
    }
}
