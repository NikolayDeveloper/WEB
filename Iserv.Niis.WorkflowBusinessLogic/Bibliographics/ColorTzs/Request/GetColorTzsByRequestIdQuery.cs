using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ColorTzs.Request
{
    public class GetColorTzsByRequestIdQuery: BaseQuery
    {
        public List<DicColorTZRequestRelation> Execute(int requestId)
        {
            var repository = Uow.GetRepository<DicColorTZRequestRelation>();

            return repository.AsQueryable()
                .Where(ctz => ctz.RequestId == requestId)
                .ToList();
        }
    }
}
