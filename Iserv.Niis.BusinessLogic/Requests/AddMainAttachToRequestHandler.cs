using System;
using System.IO;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Attachments;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Infrastructure.Helpers;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Utils.Constans;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class AddMainAttachToRequestHandler : BaseHandler
    {
        private readonly IAttachmentHelper _attachmentHelper;
        private readonly IFileStorage _fileStorage;
        
        public AddMainAttachToRequestHandler(
            IAttachmentHelper attachmentHelper,
            IFileStorage fileStorage)
        {
            _attachmentHelper = attachmentHelper;
            _fileStorage = fileStorage;
        }

        public async Task<Request> Execute(IntellectualPropertyScannerDto data)
        {
            using (var fileStream = File.OpenRead(Path.Combine(Path.GetTempPath(),
                     data.Attachment.TempName)))
            {
                if (!data.Id.HasValue)
                    throw new Exception("Request not Fount");

                var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync((int) data.Id));

                if (request == null)
                    throw new Exception("Request not Fount");

                var bucketName = "requests";
                var userId = NiisAmbientContext.Current.User.Identity.UserId;
                var fileName = $"{request.Barcode}_{request.ProtectionDocType.NameRu}{Path.GetExtension(data.Attachment.Name)}";
                //var originalName = $"current/{request.Id}/{fileName}";

                var extention = Path.GetExtension(fileName);
                var originalName = $"current/{request.Id}/{Guid.NewGuid().ToString()}{extention}";

                var newAttachment = data.Attachment.ContentType.Equals(ContentType.Pdf)
                    ? _attachmentHelper.NewPdfObject(data.Attachment, userId, bucketName, fileStream, originalName, fileName, true)
                    : _attachmentHelper.NewFileObject(data.Attachment, userId, bucketName, fileStream, originalName, fileName, true);

                // TODO: Transaction with complex logic
                //using (var transaction = _context.Database.BeginTransaction())
                //{
                try
                {
                    await Executor.GetCommand<CreateAttachmentCommand>().Process(q => q.ExecuteAsync(newAttachment));
                    await _fileStorage.AddAsync(newAttachment.BucketName, newAttachment.OriginalName, fileStream, newAttachment.ContentType);

                    var oldAttachment = request.MainAttachment;

                    request.MainAttachment = newAttachment;
                    request.MainAttachmentId = newAttachment.Id;
                    request.PageCount = newAttachment.PageCount;
                    request.CopyCount = newAttachment.CopyCount;
                    await Executor.GetCommand<UpdateRequestCommand>().Process(q => q.ExecuteAsync(request));
                    //transaction.Commit();

                    if (oldAttachment != null)
                    {
                        if (oldAttachment.ValidName != newAttachment.ValidName)
                        {
                            await _fileStorage.Remove(oldAttachment.BucketName, oldAttachment.OriginalName);
                        }

                        await Executor.GetCommand<DeleteAttachmentCommand>().Process(q => q.ExecuteAsync(oldAttachment));
                    }
                }
                catch
                {
                    //todo: log exception
                    throw;
                }

                return request;
            }

        }
    }
}
