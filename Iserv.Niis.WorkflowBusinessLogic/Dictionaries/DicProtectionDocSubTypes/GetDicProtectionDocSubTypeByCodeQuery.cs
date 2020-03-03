using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocSubTypes
{
    public class GetDicProtectionDocSubTypeByCodeQuery: BaseQuery
    {
        public DicProtectionDocSubType Execute(string code)
        {
            var repo = Uow.GetRepository<DicProtectionDocSubType>();

            return repo.AsQueryable()
                .FirstOrDefault(d => d.Code == code);
        }
    }
}
