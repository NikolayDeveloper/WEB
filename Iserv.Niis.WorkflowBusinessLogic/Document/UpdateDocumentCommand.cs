using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class UpdateDocumentCommand : BaseCommand
    {
        public void Execute(Domain.Entities.Document.Document document)
        {
            var repo = Uow.GetRepository<Domain.Entities.Document.Document>();
            repo.Update(document);

            Uow.SaveChanges();
        }
    }
}
