using Iserv.Niis.Domain.EntitiesFile;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class UpdateDocumentTemplateFileCommand: BaseCommand
    {
        public void Execute(DocumentTemplateFile file)
        {
            var repo = Uow.GetRepository<DocumentTemplateFile>();
            repo.Update(file);

            Uow.SaveChanges();
        }
    }
}
