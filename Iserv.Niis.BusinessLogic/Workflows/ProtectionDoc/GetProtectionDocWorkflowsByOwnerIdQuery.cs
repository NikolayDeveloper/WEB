using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Workflows.ProtectionDoc
{
    public class GetProtectionDocWorkflowsByOwnerIdQuery : BaseQuery
    {
        public async ValueTask<List<ProtectionDocWorkflow>> ExecuteAsync(int ownerId)
        {
            var protectionDocWorkflowRepository = Uow.GetRepository<ProtectionDocWorkflow>();
            var protectionDocWorkflows =  await protectionDocWorkflowRepository
                .AsQueryable()
                .Include(r => r.FromStage)
                .Include(r => r.CurrentStage)
                .Include(r => r.FromUser)
                .Include(r => r.CurrentUser)
                .Include(r => r.Route)
                .Where(rc => rc.OwnerId == ownerId)
                .ToListAsync();

            return protectionDocWorkflows;
        }
    }
}