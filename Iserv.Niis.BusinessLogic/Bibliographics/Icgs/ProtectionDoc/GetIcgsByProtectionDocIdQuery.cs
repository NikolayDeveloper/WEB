using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icgs.ProtectionDoc
{
    public class GetIcgsByProtectionDocIdQuery: BaseQuery
    {
        public async Task<List<ICGSProtectionDoc>> ExecuteAsync(int protectionDocId)
        {
            var repository = Uow.GetRepository<ICGSProtectionDoc>();

            return await repository.AsQueryable()
                .Where(i => i.ProtectionDocId == protectionDocId)
                .ToListAsync();
        }
    }
}
