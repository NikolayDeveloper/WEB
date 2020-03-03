using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations
{
    public class RemoveProtectionDocDocumentsCommand: BaseCommand
    {
        public async Task ExecuteAsync(List<ProtectionDocDocument> protectionDocDocuments)
        {
            var protectionDocDocumentsRepository = Uow.GetRepository<ProtectionDocDocument>();
            var protectionDocDocumentsToDelete = protectionDocDocumentsRepository.AsQueryable()
                .Where(x => protectionDocDocuments.Select(rd => rd.Id).Contains(x.Id));
            protectionDocDocumentsRepository.DeleteRange(protectionDocDocumentsToDelete);

            await Uow.SaveChangesAsync();
        }
    }
}
