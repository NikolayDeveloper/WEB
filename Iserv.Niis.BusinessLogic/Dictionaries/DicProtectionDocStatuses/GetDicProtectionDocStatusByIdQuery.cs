using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocStatuses
{
    public class GetDicProtectionDocStatusByIdQuery: BaseQuery
    {
        public async Task<DicProtectionDocStatus> ExecuteAsync(int id)
        {
            var dicProtectionDocStatusRepository = Uow.GetRepository<DicProtectionDocStatus>();

            return await dicProtectionDocStatusRepository.GetByIdAsync(id);
        }
    }
}
