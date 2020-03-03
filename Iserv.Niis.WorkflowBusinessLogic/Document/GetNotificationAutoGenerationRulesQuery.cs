using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.AutoNotificationDocumentGeneration;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GetNotificationAutoGenerationRulesQuery: BaseQuery
    {
        public IQueryable<AutoGenerateNotificationDocumentByStageRule> Execute()
        {
            var repo = Uow.GetRepository<AutoGenerateNotificationDocumentByStageRule>();

            return repo.AsQueryable()
                .Include(r => r.NotificationType)
                .AsQueryable();
        }
    }
}
