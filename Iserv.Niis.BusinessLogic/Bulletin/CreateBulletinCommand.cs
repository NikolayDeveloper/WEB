using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class CreateBulletinCommand: BaseCommand
    {
        public async Task<int> ExecuteAsync(Domain.Entities.Bulletin.Bulletin bulletin)
        {
            var repo = Uow.GetRepository<Domain.Entities.Bulletin.Bulletin>();
            await repo.CreateAsync(bulletin);

            await Uow.SaveChangesAsync();
            return bulletin.Id;
        }
    }
}
