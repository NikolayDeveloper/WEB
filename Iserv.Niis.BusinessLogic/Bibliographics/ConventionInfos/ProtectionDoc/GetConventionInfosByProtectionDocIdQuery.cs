using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.ProtectionDoc
{
    public class GetConventionInfosByProtectionDocIdQuery: BaseQuery
    {
        public async Task<List<ProtectionDocConventionInfo>> ExecuteAsync(int protectionDocId)
        {
            var protectionDocConventionInfoRepository = Uow.GetRepository<ProtectionDocConventionInfo>();

            return await protectionDocConventionInfoRepository.AsQueryable()
                .Where(pdci => pdci.ProtectionDocId == protectionDocId)
                .ToListAsync();
        }
    }
}
