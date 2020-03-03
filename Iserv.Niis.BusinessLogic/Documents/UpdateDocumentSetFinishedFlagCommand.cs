using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class UpdateDocumentSetFinishedFlagCommand : BaseCommand
    {
        public async Task ExecuteAsync(int documentId)
        {
            var repository = Uow.GetRepository<Document>();

            var document = await repository.AsQueryable().FirstOrDefaultAsync(r => r.Id == documentId);

            if (document != null)
            {
                document.IsFinished = true;
                await Uow.SaveChangesAsync();
            }
        }
    }
}
