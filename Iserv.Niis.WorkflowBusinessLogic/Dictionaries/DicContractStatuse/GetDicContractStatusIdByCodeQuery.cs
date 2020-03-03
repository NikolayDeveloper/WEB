using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicContractStatuse
{
    public class GetDicContractStatusIdByCodeQuery : BaseQuery
    {
        public int Execute(string code)
        {
            var dicContractStatuseRepo = Uow.GetRepository<DicContractStatus>();
            return dicContractStatuseRepo
                .AsQueryable()
                .Where(c => c.Code == code)
                .Select(c => c.Id)
                .FirstOrDefault();
        }
    }
}