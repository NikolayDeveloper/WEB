using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class FilterPreviousStagesByRequestIdHandler: BaseHandler
    {
        public async Task<DicRouteStage> ExecuteAsync(int requestId)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));

            if(request == null)
            {
                throw new DataNotFoundException(nameof(Request), DataNotFoundException.OperationType.Read, requestId);
            }

            var previousStage = Executor.GetQuery<GetDicRouteStageByIdQuery>()
                                .Process(q => q.Execute(request.CurrentWorkflow.FromStageId ?? default(int)));

            return previousStage;
        }
    }
}
