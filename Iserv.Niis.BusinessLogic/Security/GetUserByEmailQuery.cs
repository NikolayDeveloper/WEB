using System;
using System.Linq;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class GetUserByEmailQuery : BaseQuery
    {
        public ApplicationUser Execute(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"{nameof(email)} is null!");

            var repo = Uow.GetRepository<ApplicationUser>();
            return repo.AsQueryable()
                .FirstOrDefault(u => u.Email.Equals(email));
        }
    }
}