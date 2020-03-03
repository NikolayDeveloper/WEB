using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
//using NetCoreCQRS.Queries;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GetProtectionDocByDocumentIdQuery : BaseQuery
    {
        public async Task<ProtectionDoc> ExecuteAsync(int documentId)
        {
            var repo = Uow.GetRepository<ProtectionDoc>();
            var contract = repo
                .AsQueryable()
                .Include(r => r.Type)
                .Include(pd=>pd.Request)
                .ThenInclude(r=>r.ProtectionDocType)
                .FirstOrDefault(r => Enumerable.Any<ProtectionDocDocument>(r.Documents, d => d.DocumentId == documentId));

            return contract;
        }
    }
}
