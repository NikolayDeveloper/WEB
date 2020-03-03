using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class FilterPreviousStagesByProtectionDocIdHandler : BaseHandler
    {
        public async Task<DicRouteStage> ExecuteAsync(int protectionDocId)
        {
            var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(protectionDocId));

            if (protectionDoc == null)
            {
                throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, protectionDocId);
            }

            var previousStage = Executor.GetQuery<GetDicRouteStageByIdQuery>()
                .Process(q => q.Execute(protectionDoc.CurrentWorkflow.FromStageId ?? default(int)));

            return previousStage;
        }
    }
}
