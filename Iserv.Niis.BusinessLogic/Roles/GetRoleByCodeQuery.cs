using System.Linq;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class GetRoleByCodeQuery : BaseQuery
    {
        public ApplicationRole Execute(string roleCode)
        {
            var roleRepo = Uow.GetRepository<ApplicationRole>();
            var role = roleRepo
                .AsQueryable()
                .Include(r => r.Stages)
                .FirstOrDefault(r => r.Code == roleCode);
            if (role == null)
            {
                throw new DataNotFoundException(nameof(ApplicationRole),
                    DataNotFoundException.OperationType.Read, roleCode);
            }

            return role;
        }
    }
}
