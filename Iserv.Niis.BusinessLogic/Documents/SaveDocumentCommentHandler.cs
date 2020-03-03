using Iserv.Niis.DI;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class SaveDocumentCommentHandler : BaseHandler
    {
        public async Task ExecuteAsync(int documentId, DocumentCommentDto[] dtos)
        {
            var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));

            foreach(var dto in dtos.Where(d => d.Id <= 0))
            {
                dto.DocumentId = document.Id;
                if (dto.WorkflowId <= 0)
                {
                    var currentUserId = NiisAmbientContext.Current.User.Identity.UserId;
                    var currentWf = document.CurrentWorkflows.FirstOrDefault(d => d.CurrentUserId == currentUserId);

                    if (currentWf != null)
                        dto.WorkflowId = currentWf.Id;
                }

                await Executor.GetQuery<SaveDocumentCommentCommand>().Process(q => q.ExecuteAsync(dto));
            }
        }
    }
}
