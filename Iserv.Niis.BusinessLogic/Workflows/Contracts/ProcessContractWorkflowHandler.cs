using System;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Workflows.Contracts
{
    public class ProcessContractWorkflowHandler : BaseHandler
    {
        private readonly ICalendarProvider _calendarProvider;

        public ProcessContractWorkflowHandler(ICalendarProvider calendarProvider)
        {
            _calendarProvider = calendarProvider;
        }

        public async Task Handle(ContractWorkflow contractWorkflow, int userId)
        {
            var contract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(contractWorkflow.OwnerId));
            //todo перенес из апплаера как есть. понятния не имею зачем это тут надо, скорее всго удалим
            if (contract == null)
            {
                throw new ApplicationException($"Workflow has incorrect contract id: {contractWorkflow.OwnerId}");
            }

            var routeStage = contractWorkflow.CurrentStageId.HasValue
                ? Executor.GetQuery<GetDicRouteStageByIdQuery>().Process(q => q.Execute(contractWorkflow.CurrentStageId.Value))
                : null;
            await Executor.GetCommand<CreateContractWorkflowCommand>().Process(c => c.ExecuteAsync(contractWorkflow));
            contract.CurrentWorkflow = contractWorkflow;
            contract.StatusId = routeStage?.ContractStatusId ?? contract.StatusId;
            contract.MarkAsUnRead();
            contract.GosDate = routeStage != null && routeStage.Code.Equals(RouteStageCodes.DK02_9_2)
                ? _calendarProvider.GetPublicationDate(contract.RegDate ?? contract.DateCreate)
                : contract.GosDate;
            Executor.GetCommand<UpdateContractCommand>().Process(c => c.Execute(contract));

            //TODO: Рабочий процесс await _taskRegister.RegisterAsync(contract.Id);
            // _notificationSender.ProcessContract(contract.Id);
        }
    }
}
