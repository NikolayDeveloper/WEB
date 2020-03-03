using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType
{
    public class GetDicDocumentTypeByIdQuery : BaseQuery
    {
        public async Task<Domain.Entities.Dictionaries.DicDocumentType> ExecuteAsync(int typeId)
        {
            var repo = Uow.GetRepository<Domain.Entities.Dictionaries.DicDocumentType>();
            return await repo.GetByIdAsync(typeId);
        }
    }
}
