using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Search
{
    public class GetExpertSearchSimilarByIdRangeQuery : BaseQuery
    {
        public List<ExpertSearchSimilar> Execute(List<int> ids)
        {
            var repo = Uow.GetRepository<ExpertSearchSimilar>();

            return repo.AsQueryable().Where(r => ids.Contains(r.Id)).ToList();
        }
    }
}