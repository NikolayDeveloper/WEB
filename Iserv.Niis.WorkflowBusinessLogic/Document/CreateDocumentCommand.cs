using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class CreateDocumentCommand: BaseCommand
    {
        public int Execute(Domain.Entities.Document.Document document)
        {
            var repo = Uow.GetRepository<Domain.Entities.Document.Document>();

            repo.Create(document);
            Uow.SaveChanges();

            return document.Id;
        }
    }
}
