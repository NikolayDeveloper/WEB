using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocSubTypes
{
    public class GetDicProtectionDocSubTypesQuery: BaseQuery
    {
        public async Task<List<DicProtectionDocSubType>> ExecuteAsync()
        {
            var repo = Uow.GetRepository<DicProtectionDocSubType>();
            return await repo
                .AsQueryable()
                .ToListAsync();
        }
    }
}
