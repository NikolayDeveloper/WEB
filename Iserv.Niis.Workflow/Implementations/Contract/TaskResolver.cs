using System;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Contract
{
    public class TaskResolver : ITaskResolver<Domain.Entities.Contract.Contract>
    {
        private readonly NiisWebContext _context;
        private readonly IWorkflowApplier<Domain.Entities.Contract.Contract> _contractWorkflowApplier;
        private readonly IWorkflowApplier<Domain.Entities.Document.Document> _documentWorkflowApplier;

        public TaskResolver(NiisWebContext context, IWorkflowApplier<Domain.Entities.Contract.Contract> contractWorkflowApplier, IWorkflowApplier<Domain.Entities.Document.Document> documentWorkflowApplier)
        {
            _context = context;
            _contractWorkflowApplier = contractWorkflowApplier;
            _documentWorkflowApplier = documentWorkflowApplier;
        }

        public void Resolve()
        {
            var tasks = _context.WorkflowTaskQueues
                .Include(t => t.Contract).ThenInclude(c => c.CurrentWorkflow)
                .Include(t => t.Contract).ThenInclude(c => c.Documents)
                .Include(t => t.Contract).ThenInclude(c => c.ContractCustomers).ThenInclude(c => c.CustomerRole)
                .Include(t => t.ResultStage)
                .Include(t => t.ConditionStage)
                .Where(t => t.ResolveDate < DateTimeOffset.Now);
            foreach (var task in tasks)
            {
                if (task.ResultStage != null)
                {
                    _contractWorkflowApplier.ApplyAsync(GenerateWorkflow(task.Contract.CurrentWorkflow, task.ResultStage)).Wait(); 
                }
                else
                {
                    ResolveSpecificLogicTask(task);
                }
            }
            _context.WorkflowTaskQueues.RemoveRange(tasks);
        }

        private void ResolveSpecificLogicTask(WorkflowTaskQueue task)
        {
            if (task.ConditionStage.Code == "DK02.4.0")
            {
                var contract = task.Contract;
                if (contract == null)
                {
                    return;
                }
                ContractDocument newContractDocument = GenerateDocument(contract);
                if (contract.CurrentWorkflow.CurrentUserId != null)
                {
                    _documentWorkflowApplier.ApplyInitialAsync(newContractDocument.Document,
                        contract.CurrentWorkflow.CurrentUserId.Value);
                }
                contract.Documents.Add(newContractDocument);
                contract.IsRead = false;
                _context.SaveChangesAsync().Wait();
            }
        }

        private ContractDocument GenerateDocument(Domain.Entities.Contract.Contract contract)
        {
            var document = new Domain.Entities.Document.Document
            {
                DocumentType = DocumentType.Outgoing,
                Type = _context.DicDocumentTypes.FirstOrDefault(d => d.Code == DicDocumentType.Codes.NotificationOfAbsencePaymentOfDK),
                Addressee = GetAddressee(contract)
            };
            var newContractDocument = new ContractDocument
            {
                Contract = contract,
                Document = document
            };
            return newContractDocument;
        }

        private static DicCustomer GetAddressee(Domain.Entities.Contract.Contract contract)
        {
            return contract.ContractCustomers?.FirstOrDefault(c => new[]
            {
                DicCustomerRole.Codes.PatentAttorney, DicCustomerRole.Codes.Confidant,
                DicCustomerRole.Codes.Correspondence
            }.Contains(c.CustomerRole.Code))?.Customer;
        }

        private ContractWorkflow GenerateWorkflow(ContractWorkflow currentWorkflow, DicRouteStage taskResultStage)
        {
            return new ContractWorkflow
            {
                OwnerId = currentWorkflow.OwnerId,
                FromUserId = currentWorkflow.CurrentUserId,
                FromStageId = currentWorkflow.CurrentStageId,
                CurrentUserId = currentWorkflow.CurrentUserId,
                CurrentStageId = taskResultStage.Id,
                RouteId = taskResultStage.RouteId,
                IsComplete = taskResultStage.IsLast,
                IsSystem = taskResultStage.IsSystem,
                IsMain = taskResultStage.IsMain
            };
        }
    }
}