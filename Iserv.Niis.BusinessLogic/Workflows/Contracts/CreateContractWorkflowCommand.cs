using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Workflows.Contracts
{
    public class CreateContractWorkflowCommand : BaseCommand
    {
        public async Task<int> ExecuteAsync(ContractWorkflow contractWorkflow)
        {
            var contractWorkflowRepository = Uow.GetRepository<ContractWorkflow>();
            contractWorkflowRepository.Create(contractWorkflow);
            await Uow.SaveChangesAsync();
            return contractWorkflow.Id;
        }
    }
}