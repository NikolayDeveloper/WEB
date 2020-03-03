using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Business.Implementations
{
    public class UserIcgsUpdater : IUserIcgsUpdater
    {
        private readonly NiisWebContext _context;

        public UserIcgsUpdater(NiisWebContext context)
        {
            _context = context;
        }

        public async Task UpdateAsync(ApplicationUser user, params int[] icgsIds)
        {
            if (icgsIds == null) return;

            var userIcgs = await _context.UserIcgsRelations
                .Where(ui => ui.UserId == user.Id)
                .ToListAsync();


            var userIcgsForRemove = userIcgs
                .Where(ui => !icgsIds.Contains(ui.IcgsId))
                .ToList();

            var userIcgsForAdd = icgsIds
                .Where(i => userIcgs.All(ui => ui.IcgsId != i))
                .ToList();

            _context.UserIcgsRelations.RemoveRange(userIcgsForRemove);

            await _context.UserIcgsRelations.AddRangeAsync(userIcgsForAdd.Select(i => new UserIcgsRelation { UserId = user.Id, IcgsId = i }));

            await _context.SaveChangesAsync();
        }
    }
}
