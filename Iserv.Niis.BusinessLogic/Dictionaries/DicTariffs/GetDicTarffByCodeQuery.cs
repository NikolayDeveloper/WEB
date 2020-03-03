using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicTariffs
{
    public class GetDicTarffByCodeQuery : BaseQuery
    {
        public DicTariff Execute(string code)
        {
            var repo = Uow.GetRepository<DicTariff>();
            var result = repo.AsQueryable()
                .FirstOrDefault(t => t.Code == code);

            return result;
        }
    }
}
