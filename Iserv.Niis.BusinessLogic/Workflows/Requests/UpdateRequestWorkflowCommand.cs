using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Workflows.Requests
{
    public class UpdateRequestWorkflowCommand: BaseCommand
    {
        public async Task ExecuteAsync(RequestWorkflow workflow)
        {
            var repo = Uow.GetRepository<RequestWorkflow>();
            repo.Update(workflow);
            
            await Uow.SaveChangesAsync();
        }
    }
}
