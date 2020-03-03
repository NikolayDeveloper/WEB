using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.BusinessLogic.Attachments;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentStatusQuery;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Infrastructure.Helpers;
using Iserv.Niis.Utils.Constans;
using Serilog;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class AddMainAttachToDocumentHandler : BaseHandler
    {
        private readonly IAttachmentHelper _attachmentHelper;
        private readonly IFileStorage _fileStorage;
        
        public AddMainAttachToDocumentHandler(
            IAttachmentHelper attachmentHelper,
            IFileStorage fileStorage)
        {
            _attachmentHelper = attachmentHelper;
            _fileStorage = fileStorage;
        }

        public async Task<Document> Execute(MaterialDetailDto data)
        {
            using (var fileStream = System.IO.File.OpenRead(Path.Combine(Path.GetTempPath(),
                     data.Attachment.TempName)))
            {
                if (!data.Id.HasValue)
                    throw new Exception("Document not Found");
                var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(data.Id.Value));

                if (document == null)
                    throw new Exception("Document not Found");

                var bucketName = "documents";
                string extentionPath;

                switch (document.DocumentType)
                {
                    case DocumentType.Incoming:
                        extentionPath = "incoming";
                        break;
                    case DocumentType.Outgoing:
                        extentionPath = "outgoing";
                        break;
                    case DocumentType.Internal:
                        extentionPath = "internal";
                        break;
                    case DocumentType.DocumentRequest:
                        extentionPath = "documentRequest";
                        document.StatusId = Executor.GetQuery<GetDocumentStatusByCodeQuery>().Process(q => q.Execute(DicDocumentStatusCodes.Completed)).Id;
                        break;
                    default:
                        extentionPath = "unknown";
                        break;
                }
                var fileName = $"{document.Barcode}_{document.Type.Code}{Path.GetExtension(data.Attachment.Name)}";
                //var extentionPathResult = extentionPath != null ? $"{extentionPath}/" : string.Empty;
                //var originalName = $"current/{extentionPathResult}{document.Id}/{fileName}";

                var extention = Path.GetExtension(fileName);
                var originalName = $"current/{extentionPath}/{document.Id}/{Guid.NewGuid().ToString()}{extention}";

                var userId = NiisAmbientContext.Current.User.Identity.UserId;
                var newAttachment = data.Attachment.ContentType.Equals(ContentType.Pdf)
                    ? _attachmentHelper.NewPdfObject(data.Attachment, userId, bucketName, fileStream, originalName, fileName, data.Attachment.IsMain, document.Id)
                    : _attachmentHelper.NewFileObject(data.Attachment, userId, bucketName, fileStream, originalName, fileName, data.Attachment.IsMain, document.Id);

                // TODO: Transaction with complex logic
                //using (var transaction = _context.Database.BeginTransaction())
                //{
                try
                {
                    await Executor.GetCommand<CreateAttachmentCommand>().Process(q => q.ExecuteAsync(newAttachment));

                    document.WasScanned = data.Attachment.WasScanned;
                    if (data.Attachment.IsMain)
                    {
                        var oldAttachment = document.MainAttachment;
                        document.MainAttachment = newAttachment;
                        document.MainAttachmentId = newAttachment.Id;
                        await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));

                        if (oldAttachment != null)
                        {
                            await Executor.GetCommand<DeleteAttachmentCommand>()
                                .Process(q => q.ExecuteAsync(oldAttachment));
                            try
                            {
                                await _fileStorage.Remove(oldAttachment.BucketName, oldAttachment.ValidName);
                            }
                            catch (Exception exception)
                            {
                                var contextExceptionMessage = $"{Guid.NewGuid()}: {exception.Message}";
                                Log.Warning(exception, contextExceptionMessage);
                            }
                        }
                    }
                    else
                    {
                        var oldAttachment = document.AdditionalAttachments.FirstOrDefault(d => d.IsMain == false);

                        if (oldAttachment != null)
                        {
                            await Executor.GetCommand<DeleteAttachmentCommand>()
                                .Process(q => q.ExecuteAsync(oldAttachment));
                            try
                            {
                                await _fileStorage.Remove(oldAttachment.BucketName, oldAttachment.OriginalName);
                            }
                            catch (Exception exception)
                            {
                                var contextExceptionMessage = $"{Guid.NewGuid()}: {exception.Message}";
                                Log.Warning(exception, contextExceptionMessage);
                            }
                        }
                        await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
                    }

                    await _fileStorage.AddAsync(newAttachment.BucketName, newAttachment.OriginalName, fileStream, newAttachment.ContentType);

                    //transaction.Commit();

                    
                }
                catch
                {
                    //todo: log exception
                    throw;
                }
                // }

                return document;
            }

        }
        
        private void AddPageCountToRequest(Request request, int pageCount, int? oldPageCount)
        {
            //TODO: Вынести коды 
            var stageInitialCodes = new[] { "TM01.1", "TMI01.1", "NMPT01.1", "B01.1", "U01.1", "PO01.1", "SA01.1" };
            var currentStage = Executor.GetQuery<GetDicRouteStageByIdQuery>().Process(q => q.Execute(request.CurrentWorkflow.CurrentStageId.Value));
            if (!stageInitialCodes.Contains(currentStage.Code))
            {
                return;
            }

            if (request.PageCount.HasValue == true)
            {
                request.PageCount = oldPageCount.HasValue
                    ? request.PageCount + pageCount - oldPageCount.Value
                    : request.PageCount + pageCount;
            }
            else
            {
                request.PageCount = pageCount;
            }
        }
    }
}
