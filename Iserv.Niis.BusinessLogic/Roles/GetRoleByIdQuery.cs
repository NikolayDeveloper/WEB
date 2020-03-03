using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class GetRoleByIdQuery : BaseQuery
    {
        public async Task<ApplicationRole> ExecuteAsync(int roleId)
        {
            var roleRepo = Uow.GetRepository<ApplicationRole>();
            var role = await roleRepo
                .AsQueryable()
                .Include(r => r.Stages)
                .FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null)
            {
                throw new DataNotFoundException(nameof(ApplicationRole),
                    DataNotFoundException.OperationType.Read, roleId);
            }

            return role;
        }
    }
}