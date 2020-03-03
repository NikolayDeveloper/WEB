using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.WorkflowBusinessLogic.Workflows
{
    public class FinishProtectionDocParallelWorkflowCommand : BaseCommand
    {
        public void Execute(ProtectionDocWorkflow protectionDoctWorkflow)
        {
            var requestParallelWorkflowRepository = Uow.GetRepository<ProtectionDocParallelWorkflow>();
            var parallelWorkflows = requestParallelWorkflowRepository.AsQueryable();
            
            var appParallelWorkflow = parallelWorkflows
                .FirstOrDefault(x => x.ProtectionDocWorkflowId == protectionDoctWorkflow.Id);

            if(appParallelWorkflow is null)
            {
                throw new DataNotFoundException(nameof(ProtectionDocParallelWorkflow), DataNotFoundException.OperationType.Read, "ProtectionDocWorkflowId");
            }

            appParallelWorkflow.IsFinished = true;

            requestParallelWorkflowRepository.Update(appParallelWorkflow);

            Uow.SaveChanges();
        }
    }
}