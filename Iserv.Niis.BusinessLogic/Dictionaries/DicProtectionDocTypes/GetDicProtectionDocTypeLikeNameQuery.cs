using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes
{
    public class GetDicProtectionDocTypeLikeNameQuery : BaseQuery
    {
        public int[] Execute(string name)
        {
            var dicProtectionDocTypeRepository = Uow.GetRepository<DicProtectionDocType>();
            var dicProtectionDocTypeIds = dicProtectionDocTypeRepository.AsQueryable().Where(d => d.NameRu.Contains(name)).Select(d => d.Id).ToArray();
            return dicProtectionDocTypeIds;
        }
    }
}
