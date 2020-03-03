using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ContractCustomers
{
    public class CreateContractCustomerCommand : BaseCommand
    {
        public async Task<int> ExecuteAsync(ContractCustomer contractCustomer)
        {
            var contractCustomerRepo = Uow.GetRepository<ContractCustomer>();
            await contractCustomerRepo.CreateAsync(contractCustomer);

            
            await Uow.SaveChangesAsync();
            return contractCustomer.Id;
        }
    }
}