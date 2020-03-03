using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.ContractCustomers
{
    public class UpdateContractCustomerCommand : BaseQuery
    {
        public async Task ExecuteAsync(ContractCustomer contractCustomer)
        {
            var contractCustomerRepo = Uow.GetRepository<ContractCustomer>();
            contractCustomerRepo.Update(contractCustomer);
            await Uow.SaveChangesAsync();
        }
    }
}