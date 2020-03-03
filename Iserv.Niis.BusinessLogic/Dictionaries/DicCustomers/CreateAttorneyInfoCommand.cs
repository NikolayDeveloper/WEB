using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers
{
    public class CreateAttorneyInfoCommand : BaseCommand
    {
        public async Task<int> ExecuteAsync(CustomerAttorneyInfo customerAttorneyInfo)
        {
            var customerRepo = Uow.GetRepository<CustomerAttorneyInfo>();
            await customerRepo.CreateAsync(customerAttorneyInfo);
            await Uow.SaveChangesAsync();
            return customerAttorneyInfo.Id;
        }
    }
}