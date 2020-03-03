using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ConventionInfos.Request
{
    public class GetConventionInfosByRequestIdQuery: BaseQuery
    {
        public List<RequestConventionInfo> Execute(int requestId)
        {
            var requestConventionInfoRepository = Uow.GetRepository<RequestConventionInfo>();

            return requestConventionInfoRepository.AsQueryable()
                .Where(pdci => pdci.RequestId == requestId)
                .ToList();
        }
    }
}
