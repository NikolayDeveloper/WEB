using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.Model.Models.Role;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Iserv.Niis.Features.Administration.ApplicationRole
{
    public class Update
    {
        public class Command : IRequest<Unit>
        {
            public int RoleId { get; set; }
            public RoleDetailsDto RoleDetailsDto { get; set; }
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
            private readonly RoleManager<Domain.Entities.Security.ApplicationRole> _roleManager;
            private readonly IMapper _mapper;
            private readonly IRoleClaimsUpdater _roleClaimsUpdater;
            private readonly IRoleRouteStagesUpdater _roleRouteStagesUpdater;
            
            public CommandHandler(
                IMapper mapper,
                RoleManager<Domain.Entities.Security.ApplicationRole> roleManager,
                IRoleClaimsUpdater roleClaimsUpdater, 
                IRoleRouteStagesUpdater roleRouteStagesUpdater)
            {
                _mapper = mapper;
                _roleManager = roleManager;
                _roleClaimsUpdater = roleClaimsUpdater;
                _roleRouteStagesUpdater = roleRouteStagesUpdater;
            }

            public async Task<Unit> Handle(Command message)
            {
                var role = await GetRoleAsync(message.RoleId);

                MapFromDto(role, message.RoleDetailsDto);

                try
                {
                    await SaveRoleAsync(role);

                    await SavePermissionsAsync(role, message.RoleDetailsDto.Permissions);

                    await SaveRoleRouteStagesAsync(message.RoleId, message.RoleDetailsDto.RoleStages);
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }


                return await Unit.Task;
            }

            private async Task<Domain.Entities.Security.ApplicationRole> GetRoleAsync(int roleId)
            {
                return await _roleManager.FindByIdAsync(roleId.ToString());
            }

            private async Task SaveRoleAsync(Domain.Entities.Security.ApplicationRole role)
            {
                await _roleManager.UpdateAsync(role);
            }

            private void MapFromDto(Domain.Entities.Security.ApplicationRole role, RoleDetailsDto roleDto)
            {
                _mapper.Map(roleDto, role);
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
