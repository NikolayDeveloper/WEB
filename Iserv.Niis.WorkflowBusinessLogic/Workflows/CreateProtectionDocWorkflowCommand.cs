using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Workflows
{
    public class CreateProtectionDocWorkflowCommand : BaseCommand
    {
        public int Execute(ProtectionDocWorkflow protectionDoctWorkflow)
        {
            var requestWorkflowRepository = Uow.GetRepository<ProtectionDocWorkflow>();

            requestWorkflowRepository.Create(protectionDoctWorkflow);

            Uow.SaveChanges();

            return protectionDoctWorkflow.Id;
        }
    }
}
