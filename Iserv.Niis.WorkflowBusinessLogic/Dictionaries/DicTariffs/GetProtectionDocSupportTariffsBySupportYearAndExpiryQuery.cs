using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicTariffs
{
    public class GetProtectionDocSupportTariffsBySupportYearAndExpiryQuery: BaseQuery
    {
        public List<DicTariff> Execute(int year, bool isExpired, int protectionDocTypeId)
        {
            var repo = Uow.GetRepository<DicTariff>();
            var result = repo.AsQueryable()
                .Include(d => d.TariffProtectionDocTypes)
                .Where(t => (t.ProtectionDocSupportYearsFrom <= year || t.ProtectionDocSupportYearsUntil <= year) &&
                            t.IsProtectionDocSupportDateExpired == isExpired &&
                            t.TariffProtectionDocTypes.Any(d => d.ProtectionDocTypeId == protectionDocTypeId));
            return result.ToList();
        }
    }
}
