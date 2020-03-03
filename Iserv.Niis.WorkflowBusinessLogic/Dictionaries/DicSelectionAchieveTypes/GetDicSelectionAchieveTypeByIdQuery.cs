using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;


namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicSelectionAchieveTypes
{
    public class GetDicSelectionAchieveTypeByIdQuery : BaseQuery
    {
        public DicSelectionAchieveType Execute(int id)
        {
            var repo = Uow.GetRepository<DicSelectionAchieveType>();
            return repo.AsQueryable()
                .FirstOrDefault(s => s.Id == id);
        }
    }
}
