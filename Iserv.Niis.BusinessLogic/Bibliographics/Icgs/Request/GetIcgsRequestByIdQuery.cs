using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icgs.Request
{
    public class GetIcgsRequestByIdQuery: BaseQuery
    {
        public async Task<ICGSRequest> Execute(int id)
        {
            var repo = Uow.GetRepository<ICGSRequest>();
            return await repo.GetByIdAsync(id);
        }
    }
}
