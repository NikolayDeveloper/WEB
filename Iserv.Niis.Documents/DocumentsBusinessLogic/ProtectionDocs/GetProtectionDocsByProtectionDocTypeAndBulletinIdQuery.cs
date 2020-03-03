using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs
{
    public class GetProtectionDocsByProtectionDocTypeAndBulletinIdQuery: BaseQuery
    {
        public List<ProtectionDoc> Execute(string typeCode, int bulletinId)
        {
            var repo = Uow.GetRepository<ProtectionDoc>();

            var result = repo.AsQueryable()
                .Include(pd => pd.Addressee)
                .Where(pd =>
                    pd.Type.Code == typeCode && pd.Bulletins.Any(pb => pb.BulletinId == bulletinId && pb.IsPublish));

            return result.ToList();
        }
    }
}
