using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.EntitiesFile;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class GetDocumentTemplateFileByFileNameQuery: BaseQuery
    {
        public DocumentTemplateFile Execute(string fileName)
        {
            var repo = Uow.GetRepository<DocumentTemplateFile>();
            var file = repo.AsQueryable()
                .FirstOrDefault(f => f.FileName == fileName);

            return file;
        }
    }
}
