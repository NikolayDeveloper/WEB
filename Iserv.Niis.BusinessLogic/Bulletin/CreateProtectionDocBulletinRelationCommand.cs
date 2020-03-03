using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class CreateProtectionDocBulletinRelationCommand: BaseCommand
    {
        public async Task<int> ExecuteAsync(ProtectionDocBulletinRelation protectionDocBulletin)
        {
            var repo = Uow.GetRepository<ProtectionDocBulletinRelation>();
            await repo.CreateAsync(protectionDocBulletin);

            await Uow.SaveChangesAsync();

            return protectionDocBulletin.Id;
        }
    }
}
