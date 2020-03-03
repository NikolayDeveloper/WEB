using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.RequestCustomers
{
    public class UpdateRequestCustomerCommand : BaseCommand
    {
        public async Task ExecuteAsync(RequestCustomer requestCustomer)
        {
            var requestCustomerRepo = Uow.GetRepository<RequestCustomer>();
            requestCustomerRepo.Update(requestCustomer);
            await Uow.SaveChangesAsync();
        }
    }
}