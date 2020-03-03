using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Workflows.Requests
{
    public class ProcessRequestWorkflowHandler : BaseHandler
    {
       // private readonly ITaskRegister<Request> _taskRegister;

        public ProcessRequestWorkflowHandler(ICalendarProvider calendarProvider)
        {
            
        }

        public async Task Handle(RequestWorkflow requestWorkflow, Request request)
        {
            //todo перенес из апплаера как есть. понятния не имею зачем это тут надо, скорее всго удалим
            if (request == null)
            {
                throw new ApplicationException($"Workflow has incorrect request id: {requestWorkflow.OwnerId}");
            }

            var routeStage = requestWorkflow.CurrentStageId.HasValue
                ? Executor.GetQuery<GetDicRouteStageByIdQuery>().Process(q => q.Execute(requestWorkflow.CurrentStageId.Value))
                : null;

            CheckStage(request, routeStage);
            await Executor.GetCommand<CreateRequestWorkflowCommand>().Process(c => c.ExecuteAsync(requestWorkflow));

            request.CurrentWorkflowId = requestWorkflow.Id;
            request.StatusId = routeStage?.RequestStatusId ?? request.StatusId;
            request.IsComplete = requestWorkflow.IsComplete ?? request.IsComplete;
            request.MarkAsUnRead();

            
            await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));

            //todo: есть смысл переделать это на хендлеры
            //TODO: Рабочий процесс await _taskRegister.RegisterAsync(request.Id);
            // _notificationSender.ProcessContract(request.Id);
        }

        //TODO: Перенести эту логику
        private void CheckStage(Request request, DicRouteStage nextStage)
        {
            var queue = Executor.GetQuery<GetWorkflowTaskQueuesByRequestIdQuery>().Process(q => q.Execute(request.Id)).Result.LastOrDefault();
            
            if (nextStage.Code == "U04" && request.CurrentWorkflow.CurrentStage.Code == "U03.8"
            ) //Готовые для передачи в Госреестр (ПМ)
                if (queue != null && request.Documents.All(a => a.Document.Type.Code != "001.004G.1") &&
                    queue.ConditionStage.Code == "U03.8" && queue.ResultStage.Code == "U04" &&
                    queue.ResolveDate > DateTime.Now)
                    throw new Exception(
                        "Переход на этап \"Создание охранного документа\" не возможно пока не истечет срок публикации, либо отсутствует входящий документ «Ходатайство о досрочной публикации» ");
        }
    }
}
