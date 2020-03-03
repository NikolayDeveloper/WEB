using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.ProtectionDoc
{
    public class GetColorTzsByProtectionDocIdQuery: BaseQuery
    {
        public async Task<List<DicColorTZProtectionDocRelation>> ExecuteAsync(int protectionDocId)
        {
            var repository = Uow.GetRepository<DicColorTZProtectionDocRelation>();

            return await repository.AsQueryable()
                .Where(ctz => ctz.ProtectionDocId == protectionDocId)
                .ToListAsync();
        }
    }
}
