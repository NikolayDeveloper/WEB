using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.EarlyRegs.Request
{
    public class GetEarlyRegsByRequestIdQuery: BaseQuery
    {
        public List<RequestEarlyReg> Execute(int requestId)
        {
            var requestEarlyRegRepository = Uow.GetRepository<RequestEarlyReg>();

            return requestEarlyRegRepository.AsQueryable()
                .Where(er => er.RequestId == requestId)
                .ToList();
        }
    }
}
