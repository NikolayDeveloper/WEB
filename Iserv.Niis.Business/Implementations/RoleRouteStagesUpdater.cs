using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Business.Implementations
{
    public class RoleRouteStagesUpdater : IRoleRouteStagesUpdater
    {
        private readonly NiisWebContext _context;

        public RoleRouteStagesUpdater(NiisWebContext context)
        {
            _context = context;
        }

        public async Task UpdateAsync(int roleId, int[] stagesIds)
        {
            var existingRoleRouteStares = await GetRoleRouteStages(roleId);

            var roleRouteStagesForDelete = GetRoleRouteStagesForDelete(existingRoleRouteStares, stagesIds);

            await DeleteRoleRouteStagesAsync(roleRouteStagesForDelete);

            var roleRouteStagesForAdd = CreateRoleRouteStagesForAdd(existingRoleRouteStares, stagesIds, roleId);

            await AddeRoleRouteStagesAsync(roleRouteStagesForAdd);

        }

        private async Task AddeRoleRouteStagesAsync(IEnumerable<RoleRouteStageRelation> roleRouteStagesForAdd)
        {
            await _context.RoleRouteStageRelations.AddRangeAsync(roleRouteStagesForAdd);

            await _context.SaveChangesAsync();
        }

        private async Task DeleteRoleRouteStagesAsync(IEnumerable<RoleRouteStageRelation> roleRouteStagesForDelete)
        {
            _context.RoleRouteStageRelations.RemoveRange(roleRouteStagesForDelete);

            await _context.SaveChangesAsync();
        }

        private async Task<List<RoleRouteStageRelation>> GetRoleRouteStages(int roleId)
        {
            return await _context
                .RoleRouteStageRelations
                .Where(x => x.RoleId == roleId).ToListAsync();
        }

        private IEnumerable<RoleRouteStageRelation> GetRoleRouteStagesForDelete(
            IEnumerable<RoleRouteStageRelation> roleRouteStages, int[] stagesIds)
        {
            return roleRouteStages.Where(rrs => !stagesIds.Contains(rrs.StageId));
        }

        private IEnumerable<RoleRouteStageRelation> CreateRoleRouteStagesForAdd(
            IEnumerable<RoleRouteStageRelation> roleRouteStages, int[] stagesIds, int roleId)
        {
            return stagesIds
                .Where(id => !roleRouteStages.Select(rrs => rrs.StageId).Contains(id))
                .Select(stageId => new RoleRouteStageRelation
                {
                    RoleId = roleId,
                    StageId = stageId
                })
                .ToList();
        }
    }
}