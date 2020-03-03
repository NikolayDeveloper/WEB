using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Workflows
{
    public class CreateProtectionDocParallelWorkflowCommand : BaseCommand
    {
        public int Execute(ProtectionDocWorkflow protectionDoctWorkflow)
        {
            var requestParallelWorkflowRepository = Uow.GetRepository<ProtectionDocParallelWorkflow>();
            var protectionDocParallelWorkflow = new ProtectionDocParallelWorkflow
            {
                IsFinished = false,
                OwnerId = protectionDoctWorkflow.OwnerId,
                ProtectionDocWorkflowId = protectionDoctWorkflow.Id
            };

            requestParallelWorkflowRepository.Create(protectionDocParallelWorkflow);

            Uow.SaveChanges();

            return protectionDocParallelWorkflow.Id;
        }
    }
}
