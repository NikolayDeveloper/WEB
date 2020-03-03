using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GetProtectionDocByRequestIdQuery : BaseQuery
    {
        public ProtectionDoc Execute(int requestId)
        {
            var repo = Uow.GetRepository<ProtectionDoc>();
            return repo.AsQueryable()
                .Include(p => p.Type)
                .Include(p => p.Workflows)
                .Include(p => p.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(p => p.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Where(p => p.RequestId == requestId)
                .FirstOrDefault();
        }
    }
}
