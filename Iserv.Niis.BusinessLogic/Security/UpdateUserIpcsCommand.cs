using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class UpdateUserIpcsCommand : BaseCommand
    {
        public void Execute(int userId, int[] ipcIds)
        {
            if (ipcIds == null)
            {
                return;
            }
            var repo = Uow.GetRepository<UserIpcRelation>();
            var oldUserIpc = repo
                .AsQueryable()
                .Where(ur => ur.UserId == userId)
                .ToList();

            var userIpcForRemove = oldUserIpc.Where(i => !ipcIds.Contains(i.IpcId)).ToList();
            var userIpcForAdd = ipcIds.Except(oldUserIpc.Select(i => i.IpcId))
                .Select(ipcId => new UserIpcRelation { UserId = userId, IpcId = ipcId }).ToList();

            repo.DeleteRange(userIpcForRemove);
            repo.CreateRange(userIpcForAdd);
            Uow.SaveChanges();
        }
    }
}
