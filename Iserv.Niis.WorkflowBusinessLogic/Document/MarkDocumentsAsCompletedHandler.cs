using System.Collections.Generic;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    /// <summary>
    /// Обработчик, который изменяет статусы документов на "Завершен".
    /// </summary>
    public class MarkDocumentsAsCompletedHandler : BaseHandler
    {
        /// <summary>
        /// Выполнение обработчика.
        /// </summary>
        /// <param name="documents">Документы.</param>
        public void Execute(IEnumerable<Domain.Entities.Document.Document> documents)
        {
            DicDocumentStatus finishedStatus = Executor
                .GetQuery<GetDocumentStatusByCodeQuery>()
                .Process(query => query.Execute(DicDocumentStatusCodes.Completed));

            foreach (var document in documents)
            {
                document.Status = null;
                document.StatusId = finishedStatus.Id;
            }

            Executor
                .GetCommand<UpdateDocumentsCommand>()
                .Process(command => command.Execute(documents));
        }
    }
}
