using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Model.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Workflow
{
    public class GetStageUserOptions
    {
        public class Query : IRequest<IEnumerable<SelectOptionDto>>
        {
            public int StageId { get; set; }
        }
        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IEnumerable<SelectOptionDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<SelectOptionDto>> Handle(Query message)
            {
                
                return await _context.Users
                    .Join(
                        _context.UserRoles,
                        u => u.Id,
                        ur => ur.UserId,
                        (user, role) => new { user, role })
                    .Join(
                        _context.RoleRouteStageRelations,
                        userRole => userRole.role.RoleId,
                        roleRouteStage => roleRouteStage.RoleId,
                        (userRole, roleRouteStage) => new { userRole, roleRouteStage })
                    .Where(join => join.roleRouteStage.StageId == message.StageId)
                    .Select(join => join.userRole.user)
                    .ProjectTo<SelectOptionDto>()
                    .ToListAsync();
            }
        }
    }
}