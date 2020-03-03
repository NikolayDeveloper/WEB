using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class CreateDocumentCommand: BaseCommand
    {
        public async Task<int> ExecuteAsync(Document document)
        {
            var documentRepo = Uow.GetRepository<Document>();
            await documentRepo.CreateAsync(document);

            await Uow.SaveChangesAsync();

            return document.Id;
        }
    }
}
