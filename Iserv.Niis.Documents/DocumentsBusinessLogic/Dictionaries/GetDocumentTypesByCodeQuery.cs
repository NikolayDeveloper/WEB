using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries
{
    public class GetDocumentTypesByCodeQuery: BaseQuery
    {
        public List<DicDocumentType> Execute(string code)
        {
            var typeRepository = Uow.GetRepository<DicDocumentType>();
            var types = typeRepository.AsQueryable()
                .Include(t => t.TemplateFile)
                .Where(t => t.Code == code);

            return types.ToList();
        }
    }
}
