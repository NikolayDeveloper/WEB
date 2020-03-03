using System.Linq;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class GetUserByIdQuery : BaseQuery
    {
        public ApplicationUser Execute(int id)
        {
            var repo = Uow.GetRepository<ApplicationUser>();
            return repo.AsQueryable()
                .Include(u => u.Department).ThenInclude(d => d.Division)
                .Include(u => u.Icgss)
                .Include(u => u.Position).ThenInclude(w => w.PositionType)
                .Include(u => u.Ipcs)
                .FirstOrDefault(u => u.Id == id);
        }
    }
}