using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.ProtectionDoc
{
    public class GetEarlyRegsByProtectionDocIdQuery: BaseQuery
    {
        public async Task<List<ProtectionDocEarlyReg>> ExecuteAsync(int protectionDocId)
        {
            var protectionDocEarlyRegRepository = Uow.GetRepository<ProtectionDocEarlyReg>();

            return await protectionDocEarlyRegRepository.AsQueryable()
                .Where(er => er.ProtectionDocId == protectionDocId)
                .ToListAsync();
        }
    }
}
