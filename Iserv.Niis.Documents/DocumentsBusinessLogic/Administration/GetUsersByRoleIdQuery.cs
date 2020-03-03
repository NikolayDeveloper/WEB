using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Administration
{
    public class GetUsersByRoleIdQuery: BaseQuery
    {
        public List<ApplicationUser> Execute(int roleId)
        {
            var userRoleRepository = Uow.GetRepository<IdentityUserRole<int>>();
            var userRoles = userRoleRepository.AsQueryable()
                .Where(r => r.RoleId == roleId);

            var userRepository = Uow.GetRepository<ApplicationUser>();
            var user = userRepository.AsQueryable()
                .Where(u => userRoles.Select(ur => ur.UserId).Contains(u.Id));

            return user.ToList();
        }
    }
}
