using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using NetCoreDataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Documents
{
    /// <summary>
    /// Запрос, который возвращает массив документов по их идентификаторам вместе со связанными заявками.
    /// </summary>
    public class GetDocumentsByIdsWithRequestsQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="ids">Идентификаторы документов.</param>
        /// <returns>Массив документов.</returns>
        public async Task<Document[]> ExecuteAsync(List<int> ids)
        {
            IRepository<Document> documentRepository = Uow.GetRepository<Document>();

            return await documentRepository
                .AsQueryable()
                .Include(document => document.Requests)
                .Where(document => ids.Contains(document.Id))
                .ToArrayAsync();
        }
    }
}
