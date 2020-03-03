using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocTypes
{
    public class GetDicProtectionDocTypeByDicRouteCodeQuery : BaseQuery
    {
        public DicProtectionDocType Execute(string dicRouteCode)
        {
            var dicProtectionDocTypeRepository = Uow.GetRepository<DicProtectionDocType>();

            var dicProtectionDocType = dicProtectionDocTypeRepository.AsQueryable().Where(r => r.Code == dicRouteCode).FirstOrDefault();

            return dicProtectionDocType;
        }
    }
}
