using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
//using NetCoreCQRS.Queries;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class GetContractByDocumentIdQuery : BaseQuery
    {
        public async Task<Contract> ExecuteAsync(int documentId)
        {
            var repo = Uow.GetRepository<Contract>();
            var contract = repo
                .AsQueryable()
                .Include(r => r.ProtectionDocType)
                .FirstOrDefault(r => Enumerable.Any<ContractDocument>(r.Documents, d => d.DocumentId == documentId));

            return contract;
        }
    }
}
