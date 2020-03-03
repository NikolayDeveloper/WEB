using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icfem.Request
{
    public class GetIcfemByRequestIdQuery: BaseQuery
    {
        public List<DicIcfemRequestRelation> Execute(int requestId)
        {
            var repository = Uow.GetRepository<DicIcfemRequestRelation>();

            return repository.AsQueryable()
                .Where(pdci => pdci.RequestId == requestId)
                .ToList();
        }
    }
}
