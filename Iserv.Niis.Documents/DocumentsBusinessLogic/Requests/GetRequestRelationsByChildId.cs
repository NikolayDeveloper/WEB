using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Requests
{
    public class GetRequestRelationsByChildId: BaseQuery
    {
        public List<RequestRequestRelation> Execute(int childId)
        {
            var relationRepository = Uow.GetRepository<RequestRequestRelation>();
            var relations = relationRepository.AsQueryable()
                .Where(rr => rr.ChildId == childId);

            return relations.ToList();
        }
    }
}
