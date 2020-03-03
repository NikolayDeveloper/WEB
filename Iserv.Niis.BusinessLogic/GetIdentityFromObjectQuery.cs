using System.Linq;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic
{
    public class GetIdentityFromObjectQuery : BaseQuery
    {
        public int? Execute<TEntity>(int objectId) where TEntity : Entity<int>
        {
            var repository = Uow.GetRepository<TEntity>();
            var result = repository
                    .AsQueryable()
                    .OrderByDescending(d => d.Id)
                    .FirstOrDefault(d => d.ExternalId == objectId);

            return result?.Id;
        }
    }
}