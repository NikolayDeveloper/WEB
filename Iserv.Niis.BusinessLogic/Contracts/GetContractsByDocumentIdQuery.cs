using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class GetContractsByDocumentIdQuery: BaseQuery
    {
        public async Task<List<Contract>> ExecuteAsync(int documentId)
        {
            var repo = Uow.GetRepository<Contract>();
            var contracts = await repo.AsQueryable()
                .Where(r => r.Documents.Any(d => d.DocumentId == documentId))
                .ToListAsync();

            return contracts;
        }
    }
}
