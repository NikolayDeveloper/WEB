using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using NetCoreDataAccess.Repository;

using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Dictionaries;


namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentStatusQuery
{
    /// <summary>
    /// Запрос, который возвращает статус документа по идентификатору документа.
    /// </summary>
    public class GetDocumentStatusByDocumentIdQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="documentId">Идентификатор документа.</param>
        /// <returns>Статус документа.</returns>
        public async Task<DicDocumentStatus> ExecuteAsync(int documentId)
        {
            IRepository<Document> documentRepository = Uow.GetRepository<Document>();

            Document document = await documentRepository
                 .AsQueryable()
                 .Include(doc => doc.Status)
                 .FirstOrDefaultAsync(doc => doc.Id == documentId);                 

            return document.Status;
        }
    }
}