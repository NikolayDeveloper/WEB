using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Workflows
{
    public class GetWorkflowStageUsersQuery : BaseQuery
    {
        //БЛ#ТЬ. КАКОГО Х#Я при выборе данных о пользователе выборка мапится
        //на эту ХУ#НЮ. Тяжело было портатиь 5 минут и сделать нормальную DTO для этого? Оптимизаторы БЛ#ТЬ
        // TODO: Упростить маппинг (разрулить identity сущности так, чтобы использовать automapper). 
        // SelectOptionDto необходим для того, чтобы не генерировать многомегабайтовый траффик при инициализации источника данных у селектов
        public async ValueTask<List<SelectOptionDto>> ExecuteAsync(int stageId)
        {
            var userRepository = Uow.GetRepository<ApplicationUser>();
            var userRoleRepository = Uow.GetRepository<IdentityUserRole<int>>();
            var roleRouteStageRelationRepository = Uow.GetRepository<RoleRouteStageRelation>();

            var users = userRepository.AsQueryable();
            var userRoles = userRoleRepository.AsQueryable();
            var roleRouteStageRelations = roleRouteStageRelationRepository.AsQueryable();

            var selectOptionDtos = await users
                .Join(
                    userRoles,
                    u => u.Id,
                    ur => ur.UserId,
                    (user, role) => new { user, role })
                .Join(
                    roleRouteStageRelations,
                    userRole => userRole.role.RoleId,
                    roleRouteStage => roleRouteStage.RoleId,
                    (userRole, roleRouteStage) => new { userRole, roleRouteStage })
                .Where(join => join.roleRouteStage.StageId == stageId && join.userRole.user.IsDeleted == false)
                .Select(join => join.userRole.user)
                .Distinct()
                .ProjectTo<SelectOptionDto>()
                .ToListAsync();

            return selectOptionDtos;
        }
    }
}
