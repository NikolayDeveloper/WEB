using Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Documents
{
    /// <summary>
    /// Обработчик, который прикрепляет дочерние документы к родительскому.
    /// <para></para>
    /// Дочерние документы добавляются в материалы заявок родительского документа.
    /// </summary>
    public class LinkDocumentWithParentRequestsHandler : BaseHandler
    {
        /// <summary>
        /// Выполнение обработчика.
        /// </summary>
        /// <param name="parentId">Идентификатор родительского документа.</param>
        /// <param name="childDocumentIds">Идентификаторы дочерних документов.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(int parentId, List<int> childDocumentIds)
        {
            (Document parentDocument, Document[] childDocuments) = await GetParentAndChildDocuments(parentId, childDocumentIds);

            int[] parentDocumentRequestIds = parentDocument.Requests
                .Select(document => document.RequestId)
                .ToArray();

            List<RequestDocument> requestDocuments = new List<RequestDocument>();

            foreach (Document childDocument in childDocuments)
            {
                foreach(int requestId in parentDocumentRequestIds)
                {
                    requestDocuments.Add(new RequestDocument { RequestId = requestId, DocumentId = childDocument.Id });
                }
            }

            await Executor
                .GetCommand<AddRequestDocumentsCommand>()
                .Process(command => command.ExecuteAsync(requestDocuments));

            DocumentLinkDto[] documentLinks = childDocuments.Select(document =>
                new DocumentLinkDto
                {
                    ChildDocumentId = document.Id
                }).ToArray();

            await Executor
                .GetHandler<SaveDocumentLinkHandler>()
                .Process(handler => handler.ExecuteAsync(parentId, documentLinks));
        }

        /// <summary>
        /// Возвращает родительский и дочерние документы по их идентификаторам.
        /// </summary>
        /// <param name="parentId">Идентификатор родительского документа.</param>
        /// <param name="childDocumentIds">Идентификаторы дочерних документов.</param>
        /// <returns></returns>
        private async Task<(Document parentDocument, Document[] childDocuments)> GetParentAndChildDocuments(int parentId, List<int> childDocumentIds)
        {
            childDocumentIds.Add(parentId);

            Document[] allDocuments = await Executor
                .GetQuery<GetDocumentsByIdsWithRequestsQuery>()
                .Process(query => query.ExecuteAsync(childDocumentIds));

            Document parentDocument = allDocuments
                .First(document => document.Id == parentId);

            Document[] childDocuments = allDocuments
                .Where(document => document.Id != parentId)
                .ToArray();

            return (parentDocument, childDocuments);
        }
    }
}
