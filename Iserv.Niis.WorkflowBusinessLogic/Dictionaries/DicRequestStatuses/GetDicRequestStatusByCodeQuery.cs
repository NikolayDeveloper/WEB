using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRequestStatuses
{
    public class GetDicRequestStatusByCodeQuery: BaseQuery
    {
        public DicRequestStatus Execute(string code)
        {
            var repo = Uow.GetRepository<DicRequestStatus>();

            return repo.AsQueryable()
                .FirstOrDefault(r => r.Code == code);
        }
    }
}
