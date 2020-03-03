using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Workflows.Documents
{
    public class GetWorkflowsByDocumentIdQuery: BaseQuery
    {
        public async Task<DocumentWorkflow> ExecuteAsync(int workflowId)
        {
            var workflowsRepo = Uow.GetRepository<DocumentWorkflow>();
            var workflow = await workflowsRepo.AsQueryable()
                .Include(w => w.FromStage)
                .Include(w => w.CurrentStage)
                .Include(w => w.FromUser)
                .Include(w => w.CurrentUser)
                .Include(w => w.Route)
                .Include(w => w.Owner)
                .FirstOrDefaultAsync(w => w.Id == workflowId);

            if (workflow == null)
            {
                throw new DataNotFoundException(nameof(DocumentWorkflow), DataNotFoundException.OperationType.Read, workflowId);
            }

            return workflow;
        }
    }
}
