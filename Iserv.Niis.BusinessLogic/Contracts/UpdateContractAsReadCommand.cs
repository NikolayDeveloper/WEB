using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class UpdateContractAsReadCommand : BaseCommand
    {
        public void Execute(int contractId)
        {
            var contractRepository = Uow.GetRepository<Contract>();
            var contract = contractRepository.GetById(contractId);

            contract.MarkAsRead();

            contractRepository.Update(contract);
            Uow.SaveChanges();
        }
    }
}
