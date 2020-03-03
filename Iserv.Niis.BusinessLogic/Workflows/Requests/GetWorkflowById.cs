using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Workflows.Requests
{
    public class GetWorkflowById : BaseQuery
    {
        public DocumentWorkflow Execute(int wfId)
        {
            var repo = Uow.GetRepository<DocumentWorkflow>();
            var currentRequestWorkflow = repo
                .AsQueryable()
                .Include(r => r.CurrentStage)
                .Where(r => r.Id == wfId)
                .FirstOrDefault();
            return currentRequestWorkflow;
        }
    }
}
