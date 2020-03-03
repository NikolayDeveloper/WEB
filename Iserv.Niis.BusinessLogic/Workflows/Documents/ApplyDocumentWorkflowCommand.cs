using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Workflows.Documents
{
    public class ApplyDocumentWorkflowCommand: BaseCommand
    {
        public async Task<int> ExecuteAsync(DocumentWorkflow workflow, bool isPrevious = false)
        {
            var documentRepo = Uow.GetRepository<Document>();
            var documentWfRepo = Uow.GetRepository<DocumentWorkflow>();
            var routeStageRepo = Uow.GetRepository<DicRouteStage>();
            var document = await documentRepo.GetByIdAsync(workflow.OwnerId);
            if (document == null)
                throw new DataNotFoundException(nameof(Document), DataNotFoundException.OperationType.Read, workflow.OwnerId);

            var workflowRepo = Uow.GetRepository<DocumentWorkflow>();

            var currentWorkflows = documentWfRepo
                .AsQueryable()
                .Include(d => d.CurrentStage)
                .Where(d => d.OwnerId == workflow.OwnerId && d.IsCurent == true);

            DocumentWorkflow currentWorkflow = null;

            var prevStage = await routeStageRepo.GetByIdAsync(workflow.FromStageId.GetValueOrDefault(0));
            if (prevStage != null)
            {
                currentWorkflow = currentWorkflows.FirstOrDefault(d => d.CurrentStage.Code == prevStage.Code);
            }

            if (isPrevious)
            {
                var previousWorkflow = await workflowRepo.GetByIdAsync(currentWorkflow?.PreviousWorkflowId);
                workflow.PreviousWorkflowId = previousWorkflow?.PreviousWorkflowId;
            }
            else
            {
                workflow.PreviousWorkflowId = currentWorkflow?.Id;
            }

            if (currentWorkflow != null)
            {
                currentWorkflow.IsCurent = false;
                workflowRepo.Update(currentWorkflow);
            }

            workflow.IsCurent = true;
            await workflowRepo.CreateAsync(workflow);

            await Uow.SaveChangesAsync();

            return workflow.Id;
        }
    }
}
