using System.Linq;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicEarlyRegTypes
{
    public class GetDicEarlyRegTypeByCodeQuery : BaseQuery
    {
        public Domain.Entities.Dictionaries.DicEarlyRegType Execute(string code)
        {
            var repository = Uow.GetRepository<Domain.Entities.Dictionaries.DicEarlyRegType>();
            var result = repository.AsQueryable()
                .FirstOrDefault(r => r.Code.Equals(code));

            return result;
        }
    }
}
