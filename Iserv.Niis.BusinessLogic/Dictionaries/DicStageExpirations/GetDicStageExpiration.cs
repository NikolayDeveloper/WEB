using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicStageExpirations
{
    public class GetDicStageExpiration : BaseQuery
    {
        public DicStageExpirationByDocType Execute(int stateId, int typeId)
        {
            var repository = Uow.GetRepository<DicStageExpirationByDocType>();
            return repository
                .AsQueryable()
                .Where(d => d.RouteStageId == stateId && d.DocumentTypeId == typeId)
                .FirstOrDefault();
        }
    }
}
