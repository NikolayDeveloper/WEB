using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    /// <summary>
    /// Запрос, который возвращает документы заявки по идентификатору заявки и типам документов. 
    /// </summary>
    public class GetDocumentsByRequestIdAndTypeCodesQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки, из которой нужно брать документы.</param>
        /// <param name="typeCodes">Тип документов, которые нужно брать.</param>
        /// <returns></returns>
        public List<Domain.Entities.Document.Document> Execute(int requestId, string[] typeCodes)
        {
            var documents = Uow.GetRepository<Domain.Entities.Document.Document>()
                .AsQueryable()
                .Include(d => d.Workflows).ThenInclude(cw => cw.CurrentStage)
                .Where(d => d.Requests.Any(r => r.RequestId == requestId) && typeCodes.Contains(d.Type.Code))
                .ToList();

            return documents;
        }
    }
}