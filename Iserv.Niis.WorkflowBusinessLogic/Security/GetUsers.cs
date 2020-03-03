using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Security
{
    public class GetUsers: BaseQuery
    {
        public IList<ApplicationUser> Execute()
        {
            var repo = Uow.GetRepository<ApplicationUser>();
			return repo.AsQueryable()				
				.Include(r => r.Position).ThenInclude(p=>p.PositionType)
                .Where(u => u.IsDeleted == false)
				.ToList();
        }
    }
}
