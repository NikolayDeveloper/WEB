using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class UpdateBulletinCommand: BaseCommand
    {
        public async Task ExecuteAsync(Domain.Entities.Bulletin.Bulletin bulletin)
        {
            var repo = Uow.GetRepository<Domain.Entities.Bulletin.Bulletin>();
            var oldBulletin = await repo.GetByIdAsync(bulletin.Id);
            oldBulletin.PublishDate = bulletin.PublishDate;
            repo.Update(oldBulletin);

            await Uow.SaveChangesAsync();
        }
    }
}
