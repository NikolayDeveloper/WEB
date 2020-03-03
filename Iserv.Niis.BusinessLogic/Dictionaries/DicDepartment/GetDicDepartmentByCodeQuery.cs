using System.Linq;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDepartment
{
    public class GetDicDepartmentByCodeQuery: BaseQuery
    {
        public Domain.Entities.Dictionaries.DicDepartment Execute(string code)
        {
            var repo = Uow.GetRepository<Domain.Entities.Dictionaries.DicDepartment>();
            var department = repo.AsQueryable().FirstOrDefault(d => d.Code.Equals(code));

            return department;
        }
    }
}
