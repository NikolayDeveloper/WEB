using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Exceptions;


namespace Iserv.Niis.BusinessLogic.Users
{
    public class GetExpertsForAllocateQuery : BaseQuery
    {
        private static readonly string[] PositionNames = new[] { "старший эксперт", "эксперт", "главный эксперт" };
        public List<ApplicationUser> Execute(int currentUserId)
        {
            var userRepository = Uow.GetRepository<ApplicationUser>();
            var departmentId = userRepository
                .AsQueryable()
                .FirstOrDefault(u => u.Id == currentUserId && u.IsDeleted == false)?.DepartmentId;

            if (departmentId.HasValue == false)
            {
                throw new DataNotFoundException(nameof(ApplicationUser), DataNotFoundException.OperationType.Read, currentUserId);
            }

            var experts = userRepository
                .AsQueryable()
                .Include(u => u.Ipcs).ThenInclude(i => i.Ipc)
                .Include(u => u.Position).ThenInclude(w => w.PositionType)
                .Where(u => u.DepartmentId == departmentId
                    && u.Position != null
                    && PositionNames.Contains(u.Position.PositionType.NameRu.ToLower()))
                .ToList();

            return experts;
        }
    }
}
