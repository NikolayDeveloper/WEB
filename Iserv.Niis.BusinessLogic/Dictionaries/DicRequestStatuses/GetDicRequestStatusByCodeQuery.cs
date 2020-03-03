using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRequestStatuses
{
    public class GetDicRequestStatusByCodeQuery: BaseQuery
    {
        public async Task<DicRequestStatus> ExecuteAsync(string code)
        {
            var repo = Uow.GetRepository<DicRequestStatus>();

            return await repo.AsQueryable()
                .FirstOrDefaultAsync(r => r.Code == code);
        }
    }
}
