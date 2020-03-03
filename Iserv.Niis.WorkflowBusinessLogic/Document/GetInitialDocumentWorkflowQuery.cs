using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GetInitialDocumentWorkflowQuery: BaseQuery
    {
        public DocumentWorkflow Execute(int documentId, int userId)
        {
            var documentRepo = Uow.GetRepository<Domain.Entities.Document.Document>();
            var document = documentRepo.GetById(documentId);
            if (document == null)
                throw new DataNotFoundException(nameof(Domain.Entities.Document.Document), DataNotFoundException.OperationType.Read, documentId);

            var docTypesRepo = Uow.GetRepository<DicDocumentType>();
            var docType = docTypesRepo.GetById(document.TypeId);
            if (docType == null)
                throw new DataNotFoundException(nameof(DicDocumentType), DataNotFoundException.OperationType.Read, document.TypeId);

            var stagesRepo = Uow.GetRepository<DicRouteStage>();
            var initialStage = stagesRepo.AsQueryable()
                .FirstOrDefault(s => s.IsFirst && s.RouteId == docType.RouteId);
            if (initialStage == null)
            {
                throw new DataNotFoundException(nameof(DicRouteStage), DataNotFoundException.OperationType.Read, docType.RouteId.ToString());
            }

            return new DocumentWorkflow
            {
                OwnerId = document.Id,
                CurrentStageId = initialStage.Id,
                CurrentUserId = userId,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem
            };
        }
    }
}
