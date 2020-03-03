using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class UpdateProtectionDocBulletinRelationCommand: BaseCommand
    {
        public async Task ExecuteAsync(ProtectionDocBulletinRelation relation)
        {
            var repo = Uow.GetRepository<ProtectionDocBulletinRelation>();
            var oldRelation = await repo.GetByIdAsync(relation.Id);
            oldRelation.BulletinId = relation.BulletinId;
            oldRelation.ProtectionDocId = relation.ProtectionDocId;
            oldRelation.IsPublish = relation.IsPublish;
            repo.Update(oldRelation);

            await Uow.SaveChangesAsync();
        }
    }
}
