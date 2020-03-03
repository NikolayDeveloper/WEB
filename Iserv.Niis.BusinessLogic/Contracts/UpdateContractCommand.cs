using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class UpdateContractCommand : BaseCommand
    {
        public int Execute(Contract contract)
        {
            var contractRepository = Uow.GetRepository<Contract>();
            contractRepository.Update(contract);
            Uow.SaveChanges();
            return contract.Id;
        }
    }
}
