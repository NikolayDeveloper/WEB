using System;
using System.IO;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Attachments;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Infrastructure.Helpers;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Utils.Constans;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class AddMainAttachToContractHandler : BaseHandler
    {
        private readonly IAttachmentHelper _attachmentHelper;
        private readonly IFileStorage _fileStorage;
        
        public AddMainAttachToContractHandler(
            IAttachmentHelper attachmentHelper,
            IFileStorage fileStorage)
        {
            _attachmentHelper = attachmentHelper;
            _fileStorage = fileStorage;
        }

        public async Task<Contract> Execute(IntellectualPropertyScannerDto data)
        {
            using (var fileStream = File.OpenRead(Path.Combine(Path.GetTempPath(),
                     data.Attachment.TempName)))
            {
                if (!data.Id.HasValue)
                    throw new Exception("Contract not Fount");

                var contract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync((int) data.Id));

                if (contract == null)
                    throw new Exception("Contract not Fount");

                var bucketName = "contracts";
                var userId = NiisAmbientContext.Current.User.Identity.UserId;
                var fileName = $"{contract.Barcode}_{contract.ProtectionDocType.Code}{Path.GetExtension(data.Attachment.Name)}";

                var extention = Path.GetExtension(fileName);
                var originalName = $"current/{contract.Id}/{Guid.NewGuid().ToString()}{extention}";

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

                    var oldAttachment = contract.MainAttachment;

                    contract.MainAttachment = newAttachment;
                    contract.MainAttachmentId = newAttachment.Id;
                    contract.PageCount = newAttachment.PageCount;
                    contract.CopyCount = newAttachment.CopyCount;
                    Executor.GetCommand<UpdateContractCommand>().Process(q => q.Execute(contract));
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

                return contract;
            }

        }
    }
}
