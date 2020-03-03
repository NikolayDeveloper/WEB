using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.DocumentUserSignature
{
    public class CreateDocumentUserSignatureCommand : BaseCommand
    {
        public async Task Execute(Domain.Entities.Document.DocumentUserSignature documentUserSignature)
        {
            var repo = Uow.GetRepository<Domain.Entities.Document.DocumentUserSignature>();
            repo.Create(documentUserSignature);

            await Uow.SaveChangesAsync();
        }
    }
}
