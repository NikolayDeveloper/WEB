using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.Role;
using Microsoft.AspNetCore.Http;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class GetRolesPaginationHandler : BaseHandler
    {
        private readonly IExecutor _executor;

        public GetRolesPaginationHandler(IExecutor executor)
        {
            _executor = executor;
        }

        public async Task<IPagedList<RoleDto>> Execute(IQueryCollection queryCollection, PaginationParams parameters)
        {
            var roleClaims = await _executor.GetQuery<GetRoleClaimsQuery>().Process(q => q.ExecuteAsync());
            var roles = _executor.GetQuery<GetIQueryableRolesQuery>().Process(q => q.Execute());
            var roleDtos = roles.ProjectTo<RoleDto>(new { roleClaims});
            return roleDtos
                .Filter(queryCollection)
                .Sort(queryCollection)
                .ToPagedList(parameters);
        }
    }
}