using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ContractCustomers
{
    public class DeleteContractCustomerCommand : BaseCommand
    {
        public async Task ExecuteAsync(int contractCustomerId)
        {
            var contractCustomerRepo = Uow.GetRepository<ContractCustomer>();
            var contractCustomer = contractCustomerRepo.GetById(contractCustomerId);
            if (contractCustomer == null)
            {
                throw new DataNotFoundException(nameof(ContractCustomer),
                    DataNotFoundException.OperationType.Delete, contractCustomerId);
            }

            contractCustomerRepo.Delete(contractCustomer);
            await Uow.SaveChangesAsync();
        }
    }
}