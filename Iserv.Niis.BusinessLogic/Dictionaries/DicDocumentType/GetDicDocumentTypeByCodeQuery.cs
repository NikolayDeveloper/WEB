using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType
{
    public class GetDicDocumentTypeByCodeQuery : BaseQuery
    {
        public Domain.Entities.Dictionaries.DicDocumentType Execute(string code)
        {
            var repo = Uow.GetRepository<Domain.Entities.Dictionaries.DicDocumentType>();
            var documentType = repo
                .AsQueryable()
                .FirstOrDefault(d => d.Code == code);
            return documentType;
        }
    }
}
