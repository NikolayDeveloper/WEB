using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Workflows
{
    public class GetWorkflowStageUsersQuery : BaseQuery
    {
        public List<ApplicationUser> Execute(int stageId)
        {
            var userRepository = Uow.GetRepository<ApplicationUser>();
            var userRoleRepository = Uow.GetRepository<IdentityUserRole<int>>();
            var roleRouteStageRelationRepository = Uow.GetRepository<RoleRouteStageRelation>();

            var users = userRepository.AsQueryable();
            var userRoles = userRoleRepository.AsQueryable();
            var roleRouteStageRelations = roleRouteStageRelationRepository.AsQueryable();

            var applicationUsers = users
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
                .ToList();

            return applicationUsers;
        }
    }
}
