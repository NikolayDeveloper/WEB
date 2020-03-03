using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models.Role;
using Microsoft.AspNetCore.Identity;
//using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using Iserv.Niis.DataBridge.Implementations;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class CreateRoleHandler : BaseHandler
    {
        private readonly IMapper _mapper;
        private readonly IRoleClaimsUpdater _roleClaimsUpdater;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRoleRouteStagesUpdater _roleRouteStagesUpdater;

        public CreateRoleHandler(IMapper mapper, RoleManager<ApplicationRole> roleManager, IRoleClaimsUpdater roleClaimsUpdater, IRoleRouteStagesUpdater roleRouteStagesUpdater)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _roleClaimsUpdater = roleClaimsUpdater;
            _roleRouteStagesUpdater = roleRouteStagesUpdater;
        }

        public async Task ExecuteAsync(RoleDetailsDto roleDetailsDto)
        {
            var role = _mapper.Map<ApplicationRole>(roleDetailsDto);
            NormalizeRoleCode(role);
            try
            {
                var resultOfRoleCreated = await _roleManager.CreateAsync(role);
                if (resultOfRoleCreated.Succeeded == false)
                {
                    throw new ValidationException(string.Join(", ", resultOfRoleCreated.Errors.Select(x => x.Description)));
                }

                await _roleClaimsUpdater.UpdateAsync(role, roleDetailsDto.Permissions);
                await _roleRouteStagesUpdater.UpdateAsync(role.Id, roleDetailsDto.RoleStages);
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException?.Message ?? e.Message);
            }
        }

        private static void NormalizeRoleCode(ApplicationRole role)
        {
            role.Code = role.Code.ToUpper().Replace(" ", "_");
        }

    }
}