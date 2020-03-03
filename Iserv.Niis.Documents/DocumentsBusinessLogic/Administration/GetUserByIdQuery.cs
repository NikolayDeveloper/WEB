using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ; .Queries;
using System.Linq;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Administration
{
    public class GetUserByIdQuery : BaseQuery
    {
        public ApplicationUser Execute(int userId)
        {
            var userRepository = Uow.GetRepository<ApplicationUser>();
            var user = userRepository.AsQueryable()
                .Include(u => u.Position)
                .ThenInclude(position => position.PositionType)
                .First(u => u.Id == userId);

            return user;
        }
    }
}