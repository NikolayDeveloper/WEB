using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Customers
{
    public class CreateRequestCustomerCommand: BaseCommand
    {
        public int Execute(RequestCustomer requestCustomer)
        {
            var repo = Uow.GetRepository<RequestCustomer>();
            repo.Create(requestCustomer);
            Uow.SaveChanges();
            return requestCustomer.Id;
        }
    }
}
