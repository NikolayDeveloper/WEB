using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Model.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class GetUsersAllQuery : BaseQuery
    {
        private readonly IExecutor _executor;

        public GetUsersAllQuery(IExecutor executor)
        {
            _executor = executor;
        }
        
        public List<UserDto> Execute(HttpRequest request)
        {
            var roles = _executor.GetQuery<GetRolesAllQuery>().Process(q => q.Execute());
            var userRoles = _executor.GetQuery<GetUserRolesAllQuery>().Process(q => q.Execute());
            var repo = Uow.GetRepository<ApplicationUser>();

            var query = repo.AsQueryable()
                .AsNoTracking()
                .Include(x => x.Position).ThenInclude(w => w.PositionType)
                .Include(x => x.Department)
                .ThenInclude(x => x.Division)
                .ProjectTo<UserDto>(new { roles, userRoles })
                .ToList();

            return query;
        }
    }
}
