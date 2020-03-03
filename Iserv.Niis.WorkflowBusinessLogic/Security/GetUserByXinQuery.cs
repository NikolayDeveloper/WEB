using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Security
{
    public class GetUserByXinQuery: BaseQuery
    {
        public ApplicationUser Execute(string xin)
        {
            if (string.IsNullOrWhiteSpace(xin))
                throw new ArgumentException($"{nameof(xin)} is null!");

            var repo = Uow.GetRepository<ApplicationUser>();
            return repo.AsQueryable()
                .FirstOrDefault(u => u.XIN.Equals(xin));
        }
    }
}
