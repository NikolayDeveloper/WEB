using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicTariffs
{
    public class GetDicTariffsByProtectionDocTypeIdQuery : BaseQuery
    {
        public async Task<List<DicTariff>> ExecuteAsync(int protectionDocTypeId)
        {
            var repo = Uow.GetRepository<DicTariff>();

            return await repo.AsQueryable()
                .Include(d => d.TariffProtectionDocTypes)
                .Where(t => t.TariffProtectionDocTypes.Any(d => d.ProtectionDocTypeId == protectionDocTypeId) 
                    || t.TariffProtectionDocTypes.Count() == 0)
                .ToListAsync();
        }

        private static bool IsNewTariff(DicTariff t)
        {
            return t.Price == null;
        }
    }
}
