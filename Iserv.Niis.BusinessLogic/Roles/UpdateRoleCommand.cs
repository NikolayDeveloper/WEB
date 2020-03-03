using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class UpdateRoleCommand : BaseCommand
    {
        public void Execute(ApplicationRole role)
        {
            var repository = Uow.GetRepository<ApplicationRole>();
            repository.Update(role);
            Uow.SaveChanges();
        }
    }
}
