using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class ApplyProtectionDocInfoFromRequestCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, ProtectionDocInfo protectionDocInfo)
        {
            var protectionDocRepository = Uow.GetRepository<ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);
            protectionDoc.ProtectionDocInfo = protectionDocInfo;

            await Uow.SaveChangesAsync();
        }
    }
}
