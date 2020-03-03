using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Notifications.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Contract
{
    public class WorkflowApplier: IWorkflowApplier<Domain.Entities.Contract.Contract>
    {
        private readonly NiisWebContext _context;
        private readonly ITaskRegister<Domain.Entities.Contract.Contract> _taskRegister;
        private readonly INotificationSender _notificationSender;
        private readonly ICalendarProvider _calendarProvider;

        public WorkflowApplier(NiisWebContext context, ITaskRegister<Domain.Entities.Contract.Contract> taskRegister, INotificationSender notificationSender, ICalendarProvider calendarProvider)
        {
            _context = context;
            _taskRegister = taskRegister;
            _notificationSender = notificationSender;
            _calendarProvider = calendarProvider;
        }

        public async Task ApplyAsync(BaseWorkflow workflow)
        {
            if (!(workflow is ContractWorkflow))
            {
                throw new ArgumentException($"The argument type: ${workflow.GetType()} is not {nameof(ContractWorkflow)}");
            }

            var contractWorkflow = (ContractWorkflow) workflow;
            contractWorkflow.DateUpdate = DateTimeOffset.Now;
            if (contractWorkflow.Id < 1)
            {
                contractWorkflow.DateCreate = DateTimeOffset.Now;
            }
            await _context.ContractWorkflows.AddAsync(contractWorkflow);


            Domain.Entities.Contract.Contract contract = contractWorkflow.Owner ?? _context.Contracts.Single(c=>c.Id == contractWorkflow.OwnerId);
            if (contract == null)
                throw new ApplicationException($"Workflow has incorrect contract id: {contractWorkflow.OwnerId}");
            var stage =
                await _context.DicRouteStages.SingleOrDefaultAsync(rs => rs.Id == contractWorkflow.CurrentStageId);
            contract.CurrentWorkflow = contractWorkflow;
            contract.StatusId = stage?.ContractStatusId ?? contract.StatusId;

            contract.CurrentWorkflow = contractWorkflow;
            contract.CurrentWorkflowId = contractWorkflow.Id;
            contract.DateUpdate = DateTimeOffset.Now;

            contract.IsRead = false;
            contract.GosDate = stage != null && stage.Code.Equals("DK02.9.2")
                ? _calendarProvider.GetPublicationDate(contract.RegDate ?? contract.DateCreate)
                : contract.GosDate;
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
            await _taskRegister.RegisterAsync(contract.Id);
            _notificationSender.ProcessContract(contract.Id);
        }

        public async Task ApplyInitialAsync(Domain.Entities.Contract.Contract contract, int userId)
        {
            var initialWorkflow = CreateInitialWorkflow(contract, userId);
            if (initialWorkflow != null)
            {
                await ApplyAsync(initialWorkflow);
            }
        }

        #region Private Methods

        private ContractWorkflow CreateInitialWorkflow(Domain.Entities.Contract.Contract contract, int userId)
        {
            var protectionDocType = _context.DicProtectionDocTypes.Single(t => t.Id == contract.ProtectionDocTypeId);
            var initialStage = _context.DicRouteStages.SingleOrDefault(s => s.IsFirst && s.RouteId == protectionDocType.RouteId);
            if (initialStage == null) return null;

            return new ContractWorkflow
            {
                CurrentUserId = userId,
                OwnerId = contract.Id,
                Owner = contract,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
            };
        }

        #endregion
    }
}