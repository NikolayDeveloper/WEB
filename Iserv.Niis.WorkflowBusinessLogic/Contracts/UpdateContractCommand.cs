using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Contracts
{
    public class UpdateContractCommand : BaseCommand
    {
        public void Execute (Contract contract)
        {
            var requestRepository = Uow.GetRepository<Contract>();

            requestRepository.Update(contract);

            Uow.SaveChanges();
        }
    }
}
