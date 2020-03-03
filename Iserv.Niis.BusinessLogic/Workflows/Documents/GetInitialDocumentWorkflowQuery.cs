using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Workflows.Documents
{
    public class GetInitialDocumentWorkflowQuery: BaseCommand
    {
        public async Task<DocumentWorkflow> ExecuteAsync(int documentId, int userId)
        {
            var documentRepo = Uow.GetRepository<Document>();
            var document = await documentRepo.GetByIdAsync(documentId);
            if (document == null)
                throw new DataNotFoundException(nameof(Document), DataNotFoundException.OperationType.Read, documentId);

            var docTypesRepo = Uow.GetRepository<DicDocumentType>();
            var docType = await docTypesRepo.GetByIdAsync(document.TypeId);
            if (docType == null)
                throw new DataNotFoundException(nameof(DicDocumentType), DataNotFoundException.OperationType.Read, document.TypeId);

            var stagesRepo = Uow.GetRepository<DicRouteStage>();
            var initialStage = await stagesRepo.AsQueryable()
                .FirstOrDefaultAsync(s => s.IsFirst && s.RouteId == docType.RouteId);
            if (initialStage == null)
            {
                throw new DataNotFoundException(nameof(DicRouteStage), DataNotFoundException.OperationType.Read, docType.RouteId.ToString());
            }

            return new DocumentWorkflow
            {
                OwnerId = document.Id,
                IsCurent = true,
                CurrentStageId = initialStage.Id,
                CurrentUserId = userId,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem
            };
        } 
    }
}
