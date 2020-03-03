using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Role;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Administration.ApplicationRole
{
    public class Create
    {
        public class Command : IRequest<Unit>
        {
            public Command(RoleDetailsDto roleDetailsDto)
            {
                RoleDetailsDto = roleDetailsDto;
            }

            public RoleDetailsDto RoleDetailsDto { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.RoleDetailsDto.NameRu).NotEmpty();
                RuleFor(x => x.RoleDetailsDto.NameKz).NotEmpty();
                RuleFor(x => x.RoleDetailsDto.NameEn).NotEmpty();
                RuleFor(x => x.RoleDetailsDto.Code).NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Unit>
        {
            private readonly IMapper _mapper;
            private readonly NiisWebContext _context;
            private readonly RoleManager<Domain.Entities.Security.ApplicationRole> _roleManager;
            private readonly IRoleClaimsUpdater _roleClaimsUpdater;
            private readonly IRoleRouteStagesUpdater _roleRouteStagesUpdater;

            public CommandHandler(
                IMapper mapper,
                RoleManager<Domain.Entities.Security.ApplicationRole> roleManager,
                IRoleClaimsUpdater roleClaimsUpdater,
                IRoleRouteStagesUpdater roleRouteStagesUpdater, NiisWebContext context)
            {
                _mapper = mapper;
                _roleManager = roleManager;
                _roleClaimsUpdater = roleClaimsUpdater;
                _roleRouteStagesUpdater = roleRouteStagesUpdater;
                _context = context;
            }

            public async Task<Unit> Handle(Command message)
            {
                var roleDetailsDto = message.RoleDetailsDto;

                var role = MapFromDto(roleDetailsDto);
                try
                {
                    NormalizeRoleCode(role);

                    var identityResult = await _roleManager.CreateAsync(role);

                    if (identityResult == IdentityResult.Success)
                    {
                        await SavePermissionsAsync(role, roleDetailsDto.Permissions);

                        await SaveRoleRouteStagesAsync(role.Id, roleDetailsDto.RoleStages);
                    }
                    else
                    {
                        throw new ValidationException(string.Join(", ", identityResult.Errors.Select(x => x.Description)));
                    }
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }

                return await Unit.Task;

            }

            private Domain.Entities.Security.ApplicationRole MapFromDto(RoleDetailsDto roleDto)
            {
                return _mapper.Map<Domain.Entities.Security.ApplicationRole>(roleDto);
            }

            private void NormalizeRoleCode(Domain.Entities.Security.ApplicationRole role)
            {
                role.Code = role.Code.ToUpper().Replace(" ", "_");
            }
            private async Task SavePermissionsAsync(Domain.Entities.Security.ApplicationRole role, string[] permissions)
            {
                await _roleClaimsUpdater.UpdateAsync(role, permissions);
            }

            private async Task SaveRoleRouteStagesAsync(int roleId, int[] roleRouteStages)
            {
                await _roleRouteStagesUpdater.UpdateAsync(roleId, roleRouteStages);
            }
        }
    }
}
