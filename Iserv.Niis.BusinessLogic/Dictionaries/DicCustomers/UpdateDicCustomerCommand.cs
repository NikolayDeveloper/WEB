using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers
{
    public class UpdateDicCustomerCommand : BaseCommand
    {
        public async Task ExecuteAsync(DicCustomer customer)
        {
            var customerRepo = Uow.GetRepository<DicCustomer>();
            customerRepo.Update(customer);
            await Uow.SaveChangesAsync();
        }
    }
}