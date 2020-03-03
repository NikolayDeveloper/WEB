using System.Linq;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GetProtectionDocByIdWithIpcsQuery: BaseQuery
    {
        public ProtectionDoc Execute(int protectionDocId)
        {
            var repository = Uow.GetRepository<ProtectionDoc>();

            var protectionDoc = repository
                .AsQueryable()
                .Include(pd => pd.Type)
                .Include(r => r.IpcProtectionDocs).ThenInclude(ipc => ipc.Ipc)
                .FirstOrDefault(pd => pd.Id == protectionDocId);

            if (protectionDoc == null)
            {
                throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, protectionDocId);
            }

            return protectionDoc;
        }
    }
}
