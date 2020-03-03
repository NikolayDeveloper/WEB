using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocStatuses
{
    public class GetDicProtectionDocStatusByCodeQuery: BaseQuery
    {
        public async Task<DicProtectionDocStatus> ExecuteAsync(string code)
        {
            var dicProtectionDocStatusRepository = Uow.GetRepository<DicProtectionDocStatus>();

            var status = await dicProtectionDocStatusRepository
                .AsQueryable()
                .FirstOrDefaultAsync(pds => pds.Code.Equals(code));

            if (status == null)
                throw new DataNotFoundException(nameof(DicProtectionDocStatus), DataNotFoundException.OperationType.Read, code);

            return status;
        }
    }
}
