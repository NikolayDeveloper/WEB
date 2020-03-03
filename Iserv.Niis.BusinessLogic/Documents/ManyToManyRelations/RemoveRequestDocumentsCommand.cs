using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations
{
    public class RemoveRequestDocumentsCommand: BaseCommand
    {
        public async Task ExecuteAsync(List<RequestDocument> requestDocuments)
        {
            var requestDocumentsRepo = Uow.GetRepository<RequestDocument>();
            var requestDocumentsToDelete = requestDocumentsRepo.AsQueryable()
                .Where(x => requestDocuments.Select(rd => rd.Id).Contains(x.Id));
            requestDocumentsRepo.DeleteRange(requestDocumentsToDelete);

            await Uow.SaveChangesAsync();
        }
    }
}
