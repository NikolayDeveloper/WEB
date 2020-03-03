using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.WorkflowBusinessLogic.Workflows
{
    public class GetProtectionDocParallelWorkflowByPDWorfIdCommand : BaseCommand
    {
        public async Task<ProtectionDocParallelWorkflow> ExecuteAsync(int protectionDocWorkflowId)
        {
            var requestParallelWorkflowRepository = Uow.GetRepository<ProtectionDocParallelWorkflow>();
            var parallelWorkflows = requestParallelWorkflowRepository.AsQueryable();

            return await parallelWorkflows.FirstOrDefaultAsync(x => x.ProtectionDocWorkflowId == protectionDocWorkflowId && !x.IsFinished);
        }
    }
}
