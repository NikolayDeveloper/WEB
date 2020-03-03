using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.RequestCustomers
{
    public class DeleteRequestCustomerCommand : BaseCommand
    {
        public async Task ExecuteAsync(int requestCustomerId)
        {
            var requestCustomerRepo = Uow.GetRepository<RequestCustomer>();
            var requestCustomer = await requestCustomerRepo.GetByIdAsync(requestCustomerId);
            if (requestCustomer == null)
            {
                throw new DataNotFoundException(nameof(RequestCustomer),
                    DataNotFoundException.OperationType.Delete, requestCustomerId);
            }

            requestCustomerRepo.Delete(requestCustomer);
            await Uow.SaveChangesAsync();
        }
    }
}