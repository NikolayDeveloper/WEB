using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class CreateRequestRequestRelationCommand: BaseCommand
    {
        public int Execute(RequestRequestRelation relation)
        {
            var repo = Uow.GetRepository<RequestRequestRelation>();
            repo.Create(relation);

            Uow.SaveChanges();
            return relation.Id;
        }
    }
}
