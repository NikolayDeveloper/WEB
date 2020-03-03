using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class DeleteContractCommand : BaseCommand
    {
        public void Execute(int contractId)
        {
            var contractRepository = Uow.GetRepository<Contract>();
            var contract = contractRepository.GetById(contractId);

            if (contract == null)
            {
                throw new DataNotFoundException(nameof(Contract), DataNotFoundException.OperationType.Delete, contractId);
            }

            contractRepository.Delete(contract);
            Uow.SaveChanges();
        }
    }
}
