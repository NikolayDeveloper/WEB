using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations
{
    public class AddProtectionDocDocumentsCommand: BaseCommand
    {
        public async Task ExecuteAsync(List<ProtectionDocDocument> protectionDocDocuments)
        {
            var protectionDocDocumentsRepository = Uow.GetRepository<ProtectionDocDocument>();
            await protectionDocDocumentsRepository.CreateRangeAsync(protectionDocDocuments);

            await Uow.SaveChangesAsync();
        }
    }
}
