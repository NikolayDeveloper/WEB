using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Documents
{
    /// <summary>
    /// Запрос, возвращающий запрос к <see cref="Document"/>, включающий все не удаленные документы.
    /// </summary>
    public class GetRequestDocumentsQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <returns>Запрос к <see cref="Document"/>.</returns>
        public IQueryable<Document> Execute()
        {
            var repo = Uow.GetRepository<Document>();

            var documents = repo.AsQueryable()
                .Include(d => d.Comments)
                .Include(d => d.Workflows).ThenInclude(w => w.CurrentUser)
                .Include(d => d.Status)
                .Include(d => d.Type)
                .Where(d => !d.IsDeleted);

            return documents;
        }
    }
}