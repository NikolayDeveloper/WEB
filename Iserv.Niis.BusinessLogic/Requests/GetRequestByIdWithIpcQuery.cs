using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestByIdWithIpcQuery: BaseQuery
    {
        public Request Execute(int requestId)
        {
            var repository = Uow.GetRepository<Request>();

            var protectionDoc = repository
                .AsQueryable()
                .Include(r => r.IPCRequests).ThenInclude(ipc => ipc.Ipc)
                .FirstOrDefault(pd => pd.Id == requestId);

            if (protectionDoc == null)
            {
                throw new DataNotFoundException(nameof(Request), DataNotFoundException.OperationType.Read, requestId);
            }

            return protectionDoc;
        }
    }
}
