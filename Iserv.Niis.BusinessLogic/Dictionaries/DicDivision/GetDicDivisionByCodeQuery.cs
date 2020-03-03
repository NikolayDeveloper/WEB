using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDivision
{
    public class GetDicDivisionByCodeQuery: BaseQuery
    {
        public Domain.Entities.Dictionaries.DicDivision Execute(string code)
        {
            var repo = Uow.GetRepository<Domain.Entities.Dictionaries.DicDivision>();
            var division = repo.AsQueryable().FirstOrDefault(d => d.Code.Equals(code));

            return division;
        }
    }
}
