using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments
{
    public class GetSupportExpiredProtectionDocsQuery: BaseQuery
    {
        public List<ProtectionDoc> Execute(int months = 0)
        {
            var repo = Uow.GetRepository<ProtectionDoc>();
            var result = repo.AsQueryable()
                .Where(pd =>
                    pd.MaintainDate.HasValue &&
                    pd.MaintainDate.Value.AddMonths(months) < NiisAmbientContext.Current.DateTimeProvider.Now &&
                    new[] {RouteStageCodes.OD05, RouteStageCodes.OD05_01}.Contains(pd.CurrentWorkflow.CurrentStage
                        .Code));

            return result.ToList();
        }
    }
}
