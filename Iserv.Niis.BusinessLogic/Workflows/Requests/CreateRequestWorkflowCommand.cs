using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Workflows.Requests
{
    public class CreateRequestWorkflowCommand : BaseCommand
    {
        public async Task<int> ExecuteAsync(RequestWorkflow requestWorkflow)
        {
            var requestWorkflowRepository = Uow.GetRepository<RequestWorkflow>();
            await requestWorkflowRepository.CreateAsync(requestWorkflow);
            await Uow.SaveChangesAsync();
            return requestWorkflow.Id;
        }
    }
}