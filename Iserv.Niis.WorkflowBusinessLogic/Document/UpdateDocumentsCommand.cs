using System.Collections.Generic;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class UpdateDocumentsCommand : BaseCommand
    {
        public void Execute(IEnumerable<Domain.Entities.Document.Document> documents)
        {
            var repository = Uow.GetRepository<Domain.Entities.Document.Document>();

            repository.UpdateRange(documents);

            Uow.SaveChanges();
        }
    }
}
