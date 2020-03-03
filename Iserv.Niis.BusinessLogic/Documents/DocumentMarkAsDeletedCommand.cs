using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Threading.Tasks;
using Iserv.Niis.Exceptions;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class DocumentMarkAsDeletedCommand:BaseCommand
    {
        public async Task Execute(int documentId)
        {
            var repo = Uow.GetRepository<Document>();
            var document = repo.GetById(documentId);
            
            if (document == null)
                throw new DataNotFoundException(nameof(Document), DataNotFoundException.OperationType.Delete, documentId);

            document.IsDeleted = true;
            
            await Uow.SaveChangesAsync();
        }
    }
}
