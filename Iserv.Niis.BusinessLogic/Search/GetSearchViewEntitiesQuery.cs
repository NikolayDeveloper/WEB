using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Search
{
    public class GetSearchViewEntitiesQuery : BaseQuery
    {
        public IQueryable<SearchViewEntity> Execute()
        {
            var repo = Uow.GetRepository<SearchViewEntity>();

            return repo.AsQueryable();
        }
    }
}