using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Workflows.Contracts
{
    public class UpdateContractWorkflowCommand : BaseCommand
    {
        public void ExecuteAsync(ContractWorkflow contractWorkflow)
        {
            var contractWorkflowRepository = Uow.GetRepository<ContractWorkflow>();
            contractWorkflowRepository.Update(contractWorkflow);
            Uow.SaveChanges();
        }
    }
}