using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Role;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Administration.ApplicationRole
{
    public class List
    {
        public class Query : IRequest<IQueryable<RoleDto>> { }

        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<RoleDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public async Task<IQueryable<RoleDto>> Handle(Query message)
            {
                var roleClaims = await _context.RoleClaims.ToListAsync();
                var query = _context.Roles
                    .AsNoTracking()
                    .Include(r => r.Stages)
                    .ProjectTo<RoleDto>(new { roleClaims = roleClaims });

                return query;
            }
        }
    }
}
