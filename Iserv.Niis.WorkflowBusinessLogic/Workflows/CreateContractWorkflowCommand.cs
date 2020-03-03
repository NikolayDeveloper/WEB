using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Workflows
{
    public class CreateContractWorkflowCommand : BaseCommand
    {
        public void Execute(ContractWorkflow contract)
        {
            var requestWorkflowRepository = Uow.GetRepository<ContractWorkflow>();

            requestWorkflowRepository.Create(contract);

            Uow.SaveChanges();
        }
    }
}
