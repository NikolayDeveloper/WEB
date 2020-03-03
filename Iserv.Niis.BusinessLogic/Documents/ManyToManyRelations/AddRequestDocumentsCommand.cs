using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations
{
    public class AddRequestDocumentsCommand: BaseCommand
    {
        public async Task ExecuteAsync(List<RequestDocument> requestDocuments)
        {
            var requestDocumentsRepo = Uow.GetRepository<RequestDocument>();
            await requestDocumentsRepo.CreateRangeAsync(requestDocuments);

            await Uow.SaveChangesAsync();
        }
    }
}
