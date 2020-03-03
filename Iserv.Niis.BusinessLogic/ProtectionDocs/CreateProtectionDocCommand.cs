using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class CreateProtectionDocCommand: BaseCommand
    {
        public async Task<int> ExecuteAsync(ProtectionDoc protectionDoc)
        {
            var protectionDocsRepository = Uow.GetRepository<ProtectionDoc>();
            
            await protectionDocsRepository.CreateAsync(protectionDoc);
            await Uow.SaveChangesAsync();
            return protectionDoc.Id;
        }
    }
}
