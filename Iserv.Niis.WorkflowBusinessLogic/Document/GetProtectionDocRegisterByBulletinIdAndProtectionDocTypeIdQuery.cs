using System.Linq;
using Iserv.Niis.Common.Codes;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GetProtectionDocRegisterByBulletinIdAndProtectionDocTypeIdQuery: BaseQuery
    {
        public Domain.Entities.Document.Document Execute(int bulletinId, int protectionDocTypeId)
        {
            var repo = Uow.GetRepository<Domain.Entities.Document.Document>();

            return repo.AsQueryable()
                .Include(d => d.ProtectionDocs)
                .FirstOrDefault(d =>
                    d.BulletinId == bulletinId && d.ProtectionDocTypeId == protectionDocTypeId /* &&
                    /// удаленно (старый док)
                    d.Type.Code == DicDocumentTypeCodes.Reestr_006_014_3*/);
        }
    }
}
