using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Administration.ApplicationUser
{
    public class List
    {
        public class Query : IRequest<IQueryable<UserDto>> { }

        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<UserDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public async Task<IQueryable<UserDto>> Handle(Query message)
            {
                var roles = await _context.Roles.ToListAsync();
                var userRole = await _context.UserRoles.ToListAsync();

                var query = _context.Users
                    .AsNoTracking()
                    .Include(x=> x.Position)
                    .Include(x=>x.Department)
                    .ThenInclude(x=>x.Division)
                    .ProjectTo<UserDto>(new { roles = roles, userRoles = userRole });

                return query;
            }
        }
    }
}
