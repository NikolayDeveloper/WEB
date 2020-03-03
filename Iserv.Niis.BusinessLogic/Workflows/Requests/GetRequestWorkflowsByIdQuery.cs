using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Workflows.Requests
{
    public class GetRequestWorkflowsByIdQuery : BaseQuery
    {
        public async ValueTask<List<WorkflowDto>> ExecuteAsync(int requestWorkflowId)
        {
            var requestWorkflowRepository = Uow.GetRepository<RequestWorkflow>();
            var workflowDtos = await requestWorkflowRepository
                .AsQueryable()
                .Include(r => r.FromStage)
                .Include(r => r.CurrentStage)
                .Include(r => r.FromUser)
                .Include(r => r.CurrentUser)
                .Include(r => r.Route)
                .Where(rc => rc.OwnerId == requestWorkflowId)
                .OrderByDescending(rc => rc.DateCreate)
                .ProjectTo<WorkflowDto>()
                .ToListAsync();

            return workflowDtos;
        }
    }
}