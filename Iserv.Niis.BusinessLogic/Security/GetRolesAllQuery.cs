using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class GetRolesAllQuery : BaseQuery
    {
        public List<ApplicationRole> Execute()
        {
            var roleRepository = Uow.GetRepository<ApplicationRole>();
            return roleRepository.GetAll().ToList();
        }
    }
}
