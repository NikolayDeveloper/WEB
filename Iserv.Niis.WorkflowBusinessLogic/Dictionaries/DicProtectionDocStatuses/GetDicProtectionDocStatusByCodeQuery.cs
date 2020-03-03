using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocStatuses
{
    public class GetDicProtectionDocStatusByCodeQuery:BaseQuery
    {
        public DicProtectionDocStatus Execute(string dicProtectionDocStatusCode)
        {
            var dicProtectionDocStatusRepository = Uow.GetRepository<DicProtectionDocStatus>();

            var dicProtectionDocStatus = dicProtectionDocStatusRepository.AsQueryable().Where(r => r.Code == dicProtectionDocStatusCode).FirstOrDefault();

            return dicProtectionDocStatus;
        }
    }
}
