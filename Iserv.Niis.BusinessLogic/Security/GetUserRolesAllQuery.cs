using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class GetUserRolesAllQuery : BaseQuery
    {
        public List<IdentityUserRole<int>> Execute()
        {
            var userRoleRepository = Uow.GetRepository<IdentityUserRole<int>>();
            return userRoleRepository.GetAll().ToList();
        }
    }
}
