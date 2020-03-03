using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class GetCurrenciesQuery : BaseQuery
    {
        public IList<string> Execute()
        {
            return Uow.GetRepository<Payment>().AsQueryable().AsNoTracking()
                .Where(x => x.CurrencyType != null && x.CurrencyType.Length > 0)
                .Select(x => x.CurrencyType)
                .Distinct()
                .OrderBy(x => x)
                .ToArray();
        }
    }
}