using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Role;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Administration.ApplicationRole
{
    public class Single
    {
        public class Query : IRequest<RoleDetailsDto>
        {
            public Query(int roleId)
            {
                RoleId = roleId;
            }
            public int RoleId { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, RoleDetailsDto>
        {
            private readonly IMapper _mapper;
            private readonly NiisWebContext _context;
            private readonly RoleManager<Domain.Entities.Security.ApplicationRole> _roleManager;

            public QueryHandler(IMapper mapper, NiisWebContext context, RoleManager<Domain.Entities.Security.ApplicationRole> roleManager)
            {
                _mapper = mapper;
                _context = context;
                _roleManager = roleManager;
            }

            async Task<RoleDetailsDto> IAsyncRequestHandler<Query, RoleDetailsDto>.Handle(Query message)
            {
                var role = await _context.Roles
                    .Include(r=> r.Stages)
                    .FirstOrDefaultAsync(x => x.Id == message.RoleId);
                
                if (role == null)
                    throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                        DataNotFoundException.OperationType.Read, message.RoleId);

                return _mapper.Map<RoleDetailsDto>(role, opts => opts.Items["roleClaims"] = _context.RoleClaims);
            }
        }
    }
}
