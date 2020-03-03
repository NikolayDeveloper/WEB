using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class DeleteProtectionDocBulletinRelationCommand : BaseCommand
    {
        public async Task ExecuteAsync(int relationId)
        {
            var repo = Uow.GetRepository<ProtectionDocBulletinRelation>();
            var bulletin = await repo.GetByIdAsync(relationId);
            repo.Delete(bulletin);
            await Uow.SaveChangesAsync();
        }
    }
}
