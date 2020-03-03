using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.RequestDocumentRelations
{
    public class AddRequestDocumentCommand: BaseCommand
    {
        public void Execute(RequestDocument requestDocument)
        {
            var requestDocumentsRepo = Uow.GetRepository<RequestDocument>();
            requestDocumentsRepo.Create(requestDocument);
            Uow.SaveChanges();
        }
    }
}
