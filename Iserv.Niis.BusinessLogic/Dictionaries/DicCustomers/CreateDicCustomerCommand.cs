using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers
{
    public class CreateDicCustomerCommand : BaseCommand
    {
        public async Task<int> ExecuteAsync(DicCustomer customer)
        {
            var customerRepo = Uow.GetRepository<DicCustomer>();
            await customerRepo.CreateAsync(customer);
            await Uow.SaveChangesAsync();
            return customer.Id;
        }
    }
}