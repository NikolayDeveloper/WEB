using System.Linq;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicSendType
{
    public class GetSendTypeByCodeQuery: BaseQuery
    {
        public Domain.Entities.Dictionaries.DicMain.DicSendType Execute(string code)
        {
            var repo = Uow.GetRepository<Domain.Entities.Dictionaries.DicMain.DicSendType>();
            var result = repo.AsQueryable()
                .FirstOrDefault(s => s.Code == code);

            return result;
        }
    }
}
