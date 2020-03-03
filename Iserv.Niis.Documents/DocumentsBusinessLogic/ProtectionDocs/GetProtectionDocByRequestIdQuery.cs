using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs
{
    public class GetProtectionDocByRequestIdQuery: BaseQuery
    {
        public List<ProtectionDoc> Execute(int requestId)
        {
            var repo = Uow.GetRepository<ProtectionDoc>();

            return repo.AsQueryable()
                .Include(pd => pd.Bulletins).ThenInclude(pb => pb.Bulletin)
                .Where(pd => pd.RequestId == requestId)
                .ToList();
        }
    }
}
