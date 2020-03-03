using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.RequestCustomers
{
    public class CreateRequestCustomerCommand : BaseCommand
    {
        public async Task<int> ExecuteAsync(RequestCustomer requestCustomer)
        {
            var requestCustomerRepo = Uow.GetRepository<RequestCustomer>();
            await requestCustomerRepo.CreateAsync(requestCustomer);
            await Uow.SaveChangesAsync();
            return requestCustomer.Id;
        }
    }
}