using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Search
{
    public class GetExpertSearchSimilarByIdQuery : BaseQuery
    {
        public ExpertSearchSimilar Execute(int id)
        {
            var repo = Uow.GetRepository<ExpertSearchSimilar>();

            return repo.GetById(id);
        }
    }
}