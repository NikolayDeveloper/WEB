using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Contract
{
    public class TaskRegister: ITaskRegister<Domain.Entities.Contract.Contract>
    {
        private readonly NiisWebContext _context;
        private readonly ICalendarProvider _calendarProvider;
        private readonly Dictionary<string, string> _resultStageMap;

        public TaskRegister(NiisWebContext context, ICalendarProvider calendarProvider)
        {
            _context = context;
            _calendarProvider = calendarProvider;
            _resultStageMap = new Dictionary<string, string> {
                { "DK03.03", "DK02.9.1"},
                { "DK02.4.0", String.Empty},
                { "DK02.4.2", "DK02.4"}
            };
        }

        public async Task RegisterAsync(int contractId)
        {
            var contract = await _context.Contracts
                .Include(c => c.CurrentWorkflow).ThenInclude(w => w.CurrentStage)
                .Include(c => c.Documents).ThenInclude(d => d.Document).ThenInclude(d => d.Type)
                .SingleOrDefaultAsync(c => c.Id == contractId);
            var resultStage = GetResultStage(contract.CurrentWorkflow.CurrentStage.Code);
            var resolveDate = GetResolveDate(contract);
            if (resolveDate != null)
            {
                var workflowTask = new WorkflowTaskQueue
                {
                    Contract = contract,
                    ResolveDate = resolveDate.Value,
                    ResultStage = resultStage == contract.CurrentWorkflow.CurrentStage ? null : resultStage,
                    ConditionStage = contract.CurrentWorkflow.CurrentStage
                };
                await _context.WorkflowTaskQueues.AddAsync(workflowTask);
                await _context.SaveChangesAsync();
            }
        }

        private DicRouteStage GetResultStage(string currentStageCode)
        {
            if (!_resultStageMap.ContainsKey(currentStageCode))
            {
                return null;
            }
            var resultStageCode = _resultStageMap[currentStageCode];

            return _context.DicRouteStages
                .SingleOrDefault(s => !string.IsNullOrWhiteSpace(resultStageCode) && s.Code.Equals(resultStageCode));
        }

        private DateTimeOffset? GetResolveDate(Domain.Entities.Contract.Contract contract)
        {
            var dateCreate = contract.DateCreate;
            var currentStage = contract.CurrentWorkflow.CurrentStage;
            if (currentStage.Code == "DK02.4.2")
            {
                var answerForRequestForCustomerOfDk = contract.Documents.FirstOrDefault(d =>
                    d.Document.Type.Code == DicDocumentType.Codes.AnswerForRequestForCustomerOfDk)?.Document;
                if (answerForRequestForCustomerOfDk != null)
                {
                    return _calendarProvider.GetExecutionDate(answerForRequestForCustomerOfDk.DateCreate, ExpirationType.CalendarMonth, 3);
                }
            }
            
            if (currentStage.Code == "DK02.4.0")
            {
                return _calendarProvider.GetExecutionDate(dateCreate, ExpirationType.CalendarMonth, 1);
            }
            if (currentStage.Code == "DK03.03")
            {
                if (currentStage.ExpirationValue == null) throw new Exception($"CurrantStage {currentStage.Code} has not ExpirationValue");
                return _calendarProvider.GetExecutionDate(dateCreate, ExpirationType.CalendarDay, currentStage.ExpirationValue.Value);
            }
            return null;
        }
    }
}
