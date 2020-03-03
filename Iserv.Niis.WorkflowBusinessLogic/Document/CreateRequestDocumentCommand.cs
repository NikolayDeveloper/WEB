using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class CreateRequestDocumentCommand: BaseCommand
    {
        public void Execute(RequestDocument requestDocument)
        {
            var repo = Uow.GetRepository<RequestDocument>();

            repo.Create(requestDocument);
            Uow.SaveChanges();
        }
    }
}
