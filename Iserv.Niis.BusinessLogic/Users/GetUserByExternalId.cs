using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Users
{
    public class GetUserByExternalId : BaseQuery
    {
        public int? Execute(int userId)
        {
            var repository = Uow.GetRepository<ApplicationUser>();
            var result = repository
                    .AsQueryable()
                    .FirstOrDefault(d => d.ExternalId == userId);

            return result?.Id;
        }
    }
}