using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.DocumentUserSignature
{
    public class UpdateDocumentUserSignatureCommand : BaseCommand
    {
        public async Task Execute(Domain.Entities.Document.DocumentUserSignature documentUserSignature)
        {
            var repo = Uow.GetRepository<Domain.Entities.Document.DocumentUserSignature>();
            repo.Update(documentUserSignature);

            await Uow.SaveChangesAsync();
        }
    }
}
