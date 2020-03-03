using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.Domain.Entities.Document;

namespace Iserv.Niis.Migration.BusinessLogic.Services.OldNiis
{
    public class OldNiisDocumentService : BaseService
    {
        private readonly OldNiisContext _context;
        private readonly DictionaryTypesHelper _dictionaryTypesService;

        public OldNiisDocumentService(
            OldNiisContext context,
            DictionaryTypesHelper dictionaryTypesHelper)
        {
            _context = context;
            _dictionaryTypesService = dictionaryTypesHelper;
        }

        public List<Document> GetDocuments(int packageSize, int lastId)
        {
            var documentTypeIds = _dictionaryTypesService.GetDocumentTypeIds();
            var oldDocuments = _context.DDDocuments
                .AsNoTracking()
                .Where(d => d.Id != 0 && d.Id > lastId && documentTypeIds.Contains(d.DocTypeId))
                .OrderBy(d => d.Id)
                .Take(packageSize)
                .ToList();

            var documents = oldDocuments.Select(d => new Document
            {
                Id = d.Id,
                ExternalId = d.Id,
                Barcode = d.Id,
                TypeId = d.DocTypeId,
                DateCreate = d.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = d.DateUpdate ?? DateTimeOffset.Now,
                AddresseeId = d.CustomerId,
                DepartmentId = d.DepartmentId,
                DocumentNum = d.DocNum,
                OutgoingNumber = d.OutNum,
                IncomingNumber = d.InOutNum,
                NameRu = d.DescMlRu,
                NameEn = d.DescMlEn,
                NameKz = d.DescMlKz,
                DivisionId = d.DivisionId,
                ReceiveTypeId = d.SendType,
                IncomingNumberFilial = d.InNumAdd,
                IsFinished = CustomConverter.StringToNullableBool(d.IsComplete),
                IsDeleted = false,

            }).ToList();

            return documents;
        }

        public List<DocumentWorkflow> GetDocumentWorkflows(List<int> documentIds)
        {
            var oldDocumentWorkflows = _context.WTPTWorkoffices
                .AsNoTracking()
                .Where(w => documentIds.Contains(w.DocumentId))
                .OrderBy(w => w.Id)
                .ToList();

            var documentWorkflows = oldDocumentWorkflows.Select(w => new DocumentWorkflow
            {
                IsComplete = CustomConverter.StringToNullableBool(w.IsComplete),
                OwnerId = w.DocumentId,
                IsCurent = true,
                ControlDate = w.ControlDate,
                DateCreate = w.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = w.DateUpdate ?? DateTimeOffset.Now,
                CurrentStageId = w.ToStageId,
                CurrentUserId = w.ToUserId,
                FromStageId = w.FromStageId,
                FromUserId = w.FromUserId,
                Description = w.Description,
                IsMain = true,
                IsSystem = CustomConverter.StringToNullableBool(w.IsSystem),
                RouteId = w.TypeId
            }).ToList();

            return documentWorkflows;
        }

        public int GetDocumentsCount()
        {
            var documentTypeIds = _dictionaryTypesService.GetDocumentTypeIds();
            return _context.DDDocuments
                .AsNoTracking()
                .Where(d => documentTypeIds.Contains(d.DocTypeId))
                .Count();
        }
    }
}
