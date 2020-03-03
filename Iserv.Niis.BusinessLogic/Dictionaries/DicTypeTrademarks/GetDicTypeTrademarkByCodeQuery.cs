using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicTypeTrademarks
{
    public class GetDicTypeTrademarkByCodeQuery : BaseQuery
    {
        public DicTypeTrademark Execute(string typeTrademarkCode)
        {
            var repository = Uow.GetRepository<DicTypeTrademark>();
            var typeTrademark = repository.AsQueryable().FirstOrDefault(t => t.Code.Equals(typeTrademarkCode));
            return typeTrademark;
        }
    }
}
