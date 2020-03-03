using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Document
{
    public class WorkflowApplier : IWorkflowApplier<Domain.Entities.Document.Document>
    {
        private readonly NiisWebContext _context;

        public WorkflowApplier(NiisWebContext context)
        {
            _context = context;
        }
        public void Apply(DocumentWorkflow workflow)
        {
            var document = workflow.Owner ?? _context.Documents.SingleOrDefault(d => d.Id == workflow.OwnerId);
            if (document == null)
                throw new ApplicationException($"Workflow has incorrect document id: {workflow.OwnerId}");
            
            document.CurrentWorkflow = workflow;
            _context.DocumentWorkflows.Add(workflow);
        }

        public void ApplyInitial(Domain.Entities.Document.Document document, int userId)
        {
            var initialWorkflow = CreateInitialWorkflow(document, userId);
            if (initialWorkflow != null)
            {
                Apply(initialWorkflow);
            }
        }

        public async Task ApplyAsync(BaseWorkflow workflow)
        {
            if (!(workflow is DocumentWorkflow))
            {
                throw new ArgumentException($"The argument type: ${workflow.GetType()} is not {nameof(DocumentWorkflow)}");
            }

            var documentWorkflow = (DocumentWorkflow)workflow;

            var document = documentWorkflow.Owner ?? await _context.Documents.SingleOrDefaultAsync(d => d.Id == documentWorkflow.OwnerId);
            if (document == null)
                throw new ApplicationException($"Workflow has incorrect document id: {documentWorkflow.OwnerId}");

            document.CurrentWorkflow = documentWorkflow;
            await _context.DocumentWorkflows.AddAsync(documentWorkflow);
        }
        
        public async Task ApplyInitialAsync(Domain.Entities.Document.Document document, int userId)
        {
            var initialWorkflow = await CreateInitialWorkflowAsync(document, userId);
            if (initialWorkflow != null)
            {
                await ApplyAsync(initialWorkflow);
            }
        }

        private DocumentWorkflow CreateInitialWorkflow(Domain.Entities.Document.Document document, int userId)
        {
            var documentType = _context.DicDocumentTypes.Single(t => t.Id == document.TypeId);
            var initialStage = _context.DicRouteStages.SingleOrDefault(s => s.IsFirst && s.RouteId == documentType.RouteId);
            if (initialStage != null)
            {
                return new DocumentWorkflow
                {
                    OwnerId = document.Id,
                    Owner = document,
                    CurrentStageId = initialStage.Id,
                    CurrentUserId = userId,
                    RouteId = initialStage.RouteId,
                    IsComplete = initialStage.IsLast,
                    IsSystem = initialStage.IsSystem
                };
            }

            return null;
        }

        private async Task<DocumentWorkflow> CreateInitialWorkflowAsync(Domain.Entities.Document.Document document, int userId)
        {
            var documentType = await _context.DicDocumentTypes.SingleAsync(t => t.Id == document.TypeId);
            var initialStage = await _context.DicRouteStages.SingleOrDefaultAsync(s => s.IsFirst && s.RouteId == documentType.RouteId);
            if (initialStage == null) return null;

            return new DocumentWorkflow
            {
                OwnerId = document.Id,
                Owner = document,
                CurrentStageId = initialStage.Id,
                CurrentUserId = userId,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem
            };
        }
    }
}
