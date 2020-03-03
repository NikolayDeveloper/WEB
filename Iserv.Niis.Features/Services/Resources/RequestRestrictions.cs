using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Services.Resources
{
    public static class RequestRestrictions
    {
        public static IQueryable<Domain.Entities.Request.Request> WherePermissions(
            this DbSet<Domain.Entities.Request.Request> set, int userId, IEnumerable<int> roleStageIds)
        {
            return set;
            return set.Where(r =>
                r.User.Id == userId
                // Активные для текущего пользоваетеля
                || r.CurrentWorkflow != null
                && r.CurrentWorkflow.CurrentUserId.HasValue
                && r.CurrentWorkflow.CurrentUserId.Value == userId
                && r.CurrentWorkflow.CurrentStageId.HasValue
                && roleStageIds.Contains(r.CurrentWorkflow.CurrentStageId.Value)
                // Завершенные
                || r.Workflows.Any(w =>
                    w.IsComplete.HasValue
                    && w.IsComplete.Value
                    && w.CurrentUserId == userId));
        }

        public static async Task<List<int>> GetUserRoleStagesIds(NiisWebContext context, int userId)
        {
            return await context.UserRoles
                .Join(
                    context.RoleRouteStageRelations,
                    role => role.RoleId,
                    roleStage => roleStage.RoleId,
                    (userRole, roleStage) => new { userRole, roleStage })
                .Where(ur => ur.userRole.UserId == userId)
                .Select(ur => ur.roleStage.StageId)
                .ToListAsync();
        }
    }
}
