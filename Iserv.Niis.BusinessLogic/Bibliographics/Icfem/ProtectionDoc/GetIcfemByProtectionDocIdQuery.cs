using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icfem.ProtectionDoc
{
    public class GetIcfemByProtectionDocIdQuery: BaseQuery
    {
        public async Task<List<DicIcfemProtectionDocRelation>> ExecuteAsync(int protectionDocId)
        {
            var repository = Uow.GetRepository<DicIcfemProtectionDocRelation>();

            return await repository.AsQueryable()
                .Where(icfem => icfem.ProtectionDocId == protectionDocId)
                .ToListAsync();
        }
    }
}
