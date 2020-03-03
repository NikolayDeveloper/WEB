using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Administration
{
    public class GetUsersByPositionIdQuery: BaseQuery
    {
        public List<ApplicationUser> Execute(int positionId)
        {
            var userRepository = Uow.GetRepository<ApplicationUser>();
            var users = userRepository.AsQueryable()
                .Include(u => u.Department).ThenInclude(d => d.Users)
                .Where(u => u.PositionId == positionId && u.IsDeleted == false);

            return users.ToList();
        }
    }
}
