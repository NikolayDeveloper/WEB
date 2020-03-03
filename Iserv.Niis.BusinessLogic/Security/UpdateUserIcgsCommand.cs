using System.Linq;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class UpdateUserIcgsCommand : BaseCommand
    {
        public void Execute(int userId, int[] icgsIds)
        {
            if (icgsIds == null) return;

            var repo = Uow.GetRepository<UserIcgsRelation>();
            var oldUserIcgs = repo
                .AsQueryable()
                .Where(ur => ur.UserId == userId)
                .ToList();

            var userIcgsForRemove = oldUserIcgs.Where(ui => !icgsIds.Contains(ui.IcgsId)).ToList();
            var userIcgsForAdd = icgsIds.Except(oldUserIcgs.Select(ui => ui.IcgsId))
                .Select(icgsId => new UserIcgsRelation {UserId = userId, IcgsId = icgsId});

            repo.DeleteRange(userIcgsForRemove);
            repo.CreateRange(userIcgsForAdd);

            Uow.SaveChanges();
        }
    }
}