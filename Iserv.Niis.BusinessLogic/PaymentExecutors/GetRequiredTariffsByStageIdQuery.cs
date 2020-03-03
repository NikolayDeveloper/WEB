using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentExecutors
{
    public class GetRequiredTariffsByStageIdQuery: BaseQuery
    {
        public async Task<List<DicTariff>> ExecuteAsync(int stageId)
        {
            var repo = Uow.GetRepository<RequiredPayment>();
            var requirements = repo.AsQueryable()
                .Where(r => r.StageId == stageId).Select(r => r.TariffId);

            var tariffRepo = Uow.GetRepository<DicTariff>();
            var result = await tariffRepo.AsQueryable()
                .Where(r => requirements.Contains(r.Id))
                .ToListAsync();

            return result;
        }
    }
}
