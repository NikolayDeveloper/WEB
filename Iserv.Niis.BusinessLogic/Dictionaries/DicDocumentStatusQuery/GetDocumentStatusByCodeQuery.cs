using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;
using Iserv.Niis.Exceptions;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentStatusQuery
{
    public class GetDocumentStatusByCodeQuery : BaseQuery
    {
        public DicDocumentStatus Execute(string code)
        {
            var repository = Uow.GetRepository<DicDocumentStatus>();

            var documentStatus = repository.AsQueryable().FirstOrDefault(t => t.Code.Equals(code));

            if (documentStatus == null)
                throw new DataNotFoundException(nameof(DicDocumentStatus), DataNotFoundException.OperationType.Read, code);

            return documentStatus;
        }
    }
}
