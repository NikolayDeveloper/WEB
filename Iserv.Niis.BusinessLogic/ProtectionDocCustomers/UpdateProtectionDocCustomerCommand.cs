using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ProtectionDocCustomers
{
    public class UpdateProtectionDocCustomerCommand: BaseCommand
    {
        public async Task ExecuteAsync(ProtectionDocCustomer protectionDocCustomer)
        {
            var repo = Uow.GetRepository<ProtectionDocCustomer>();
            repo.Update(protectionDocCustomer);
            await Uow.SaveChangesAsync();
        }
    }
}
