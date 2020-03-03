using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.AutoNotificationDocumentGeneration;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class GetNotificationAutoGenerationRulesQuery: BaseQuery
    {
        public IQueryable<AutoGenerateNotificationDocumentByPetitionAndPaymentRule> Execute()
        {
            var repo = Uow.GetRepository<AutoGenerateNotificationDocumentByPetitionAndPaymentRule>();

            return repo.AsQueryable()
                .Include(r => r.NotificationType)
                .AsQueryable();
        }
    }
}
