using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes
{
    public class GetDicProtectionDocTypeByIdQuery : BaseQuery
    {
        public DicProtectionDocType Execute(int protectionDocTypeId)
        {
            var dicProtectionDocTypeRepository = Uow.GetRepository<DicProtectionDocType>();
            var dicProtectionDocType = dicProtectionDocTypeRepository.GetById(protectionDocTypeId);
            return dicProtectionDocType;
        }
    }
}
