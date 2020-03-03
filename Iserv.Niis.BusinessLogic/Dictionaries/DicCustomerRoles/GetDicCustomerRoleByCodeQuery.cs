using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomerRoles
{
    public class GetDicCustomerRoleByCodeQuery: BaseQuery
    {
        public DicCustomerRole Execute(string code)
        {
            var repo = Uow.GetRepository<DicCustomerRole>();
            return repo.AsQueryable()
                .FirstOrDefault(r => r.Code == code);
        }
    }
}
