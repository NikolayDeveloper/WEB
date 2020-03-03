using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class UpdateUserCommand : BaseCommand
    {
        public void Execute(ApplicationUser user)
        {
            var repo = Uow.GetRepository<ApplicationUser>();
            repo.Update(user);
            Uow.SaveChanges();
        }
    }
}