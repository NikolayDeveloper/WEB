using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class UpdateProtectionDocProtectionDocInfoCommand : BaseCommand
    {
        public async Task<ProtectionDoc> ExecuteAsync(int protectionDocId, ProtectionDocInfo info)
        {
            var protectionDocRepository = Uow.GetRepository<ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);
            if (protectionDoc == null)
            {
                throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, protectionDocId);
            }
            protectionDoc.ProtectionDocInfo = info;
            protectionDocRepository.Update(protectionDoc);
            await Uow.SaveChangesAsync();
            return protectionDoc;
        }
    }
}
