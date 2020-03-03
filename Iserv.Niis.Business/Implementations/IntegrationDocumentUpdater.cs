using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Business.Implementations
{
    public class IntegrationDocumentUpdater : IIntegrationDocumentUpdater
    {
        private readonly NiisWebContext _context;
        private readonly DictionaryHelper _dictionaryHelper;
        private readonly IFileStorage _fileStorage;

        public IntegrationDocumentUpdater(NiisWebContext context, DictionaryHelper dictionaryHelper, IFileStorage fileStorage)
        {
            _context = context;
            _dictionaryHelper = dictionaryHelper;
            _fileStorage = fileStorage;
        }

        public async Task Add(RequestWorkflow requestWorkflow)
        {
            var request = _context.Requests
                .First(r => r.Id == requestWorkflow.OwnerId);
            //TODO: уточнить у аналитиков тип этап маршрута т.к данный этап относиться к материалам 
            var dicRouteStageSendingId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicRouteStage), RouteStageCodes.DocumentOutgoing_03_1);
            if (requestWorkflow.IsComplete == false && requestWorkflow.FromStageId == dicRouteStageSendingId)
            {
                var integrationDocument = _context.IntegrationDocuments
                    .FirstOrDefault(x => x.RequestBarcode == request.Barcode);
                if (integrationDocument != null)
                {
                    _context.IntegrationDocuments.Remove(integrationDocument);
                    await _context.SaveChangesAsync();
                }
                return;
            }

            var dicRouteStageTransferToPatentHolderId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicRouteStage), RouteStageCodes.OD01_5);
            var dicRouteStageCurrentTrademarkId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicRouteStage), RouteStageCodes.OD05_01);
            if (requestWorkflow.IsComplete == false
                && requestWorkflow.FromStageId == dicRouteStageTransferToPatentHolderId
                && requestWorkflow.CurrentStageId == dicRouteStageCurrentTrademarkId)
            {
                var dicReceiveTypeElectronicFeedId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicReceiveType), DicReceiveTypeCodes.ElectronicFeed);
                if (request.ReceiveTypeId == dicReceiveTypeElectronicFeedId)
                {
                    var dicDocClassificationRequestMaterialsId =
                        _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDocumentClassification), DicDocumentClassificationCodes.RequestMaterialsOutgoing);
                    var docClassifications =
                        _dictionaryHelper.GetChildClass(new[] { dicDocClassificationRequestMaterialsId });
                    var document = _context.Requests
                        .Where(x => x.Id == request.Id)
                        .SelectMany(x => x.Documents)
                        .Where(x => x.Document.OutgoingNumber != null && x.Document.MainAttachment != null &&
                                    (x.Document.Type.ClassificationId == dicDocClassificationRequestMaterialsId ||
                                     docClassifications.Contains(x.Document.Type.ClassificationId.Value)))
                        .Select(x => x.Document)
                        .FirstOrDefault();
                    if (document == null)
                    {
                        return;
                    }
                    var file = await _fileStorage.GetAsync(document.MainAttachment.BucketName,
                        document.MainAttachment.OriginalName);
                    if (file == null)
                    {
                        return;
                    }
                    await _context.IntegrationDocuments.AddAsync(new IntegrationDocument
                    {
                        DateCreate = DateTimeOffset.Now,
                        DocumentBarcode = document.Barcode,
                        DocumentTypeId = document.TypeId,
                        File = file,
                        Note = document.NameRu,
                        FileName = document.MainAttachment.OriginalName,
                        InOutDate = document.DateCreate,
                        RequestBarcode = request.Barcode,
                        InOutNumber = document.OutgoingNumber
                    });
                    UpdateStatement(request, file, document.MainAttachment.OriginalName);
                    _context.SaveChanges();
                }
            }
        }
        private void UpdateStatement(Request request, byte[] file, string fileName)
        {
            var protectionDoc = _context.ProtectionDocs
                .FirstOrDefault(x => x.RequestId == request.Id);
            if (protectionDoc == null)
                return;
            var statements = _context.Statements
                .Where(x => x.RequestId == request.Id);
            foreach (var statement in statements)
            {
                statement.File = file;
                statement.FileName = fileName;
                statement.GosNumber = protectionDoc.GosNumber;
                statement.ReqDate = protectionDoc.RegDate;
                statement.GosDate = protectionDoc.GosDate;
                statement.ReqNumber = protectionDoc.RegNumber;
                statement.DateUpdate = DateTimeOffset.Now;
            }
            if (statements.Any())
            {
                _context.Statements.UpdateRange(statements);
            }
        }
    }
}