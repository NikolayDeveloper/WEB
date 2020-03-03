using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class SaveDocumentLinkHandler : BaseHandler
    {
        public async Task ExecuteAsync(int documentId, DocumentLinkDto[] dtos)
        {
            var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));

            foreach (var dto in dtos.Where(d => d.ParentDocumentId <= 0))
            {
                dto.ParentDocumentId = document.Id;
                await Executor.GetQuery<SaveDocumentLinkCommand>().Process(q => q.ExecuteAsync(dto));
            }
            foreach (var dto in dtos.Where(d => d.ChildDocumentId >= 0 && d.NeedRemove == true))
            {
                await Executor.GetQuery<RemoveDocumentLinkCommand>().Process(q => q.ExecuteAsync(dto));
            }
        }
    }
}
