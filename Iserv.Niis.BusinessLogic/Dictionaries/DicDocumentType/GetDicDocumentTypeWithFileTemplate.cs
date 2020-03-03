using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType
{
    public class GetDicDocumentTypeWithFileTemplate : BaseQuery
    {
        public IQueryable<Domain.Entities.Dictionaries.DicDocumentType> Execute()
        {
            var repository = Uow.GetRepository<Domain.Entities.Dictionaries.DicDocumentType>();

            return repository.AsQueryable()
                                 .Where(r => r.TemplateFileId.HasValue
                                 && r.TemplateFile != null
                                 && r.TemplateFile.File != null);

        }
    }
}
