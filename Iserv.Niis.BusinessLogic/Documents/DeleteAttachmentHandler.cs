using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Attachments;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class DeleteAttachmentHandler: BaseHandler
    {
        private readonly IFileStorage _fileStorage;

        public DeleteAttachmentHandler(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public async Task ExecuteAsync(int documentId, bool isMain)
        {
            var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));
            Attachment attachmentToDelete;
            if (isMain)
            {
                attachmentToDelete = document.MainAttachment;
                document.MainAttachment = null;
                document.MainAttachmentId = null;
            }
            else
            {
                attachmentToDelete = document.AdditionalAttachments.FirstOrDefault(a => a.IsMain == false);
                document.AdditionalAttachments.Remove(attachmentToDelete);
            }

            await _fileStorage.Remove(attachmentToDelete?.BucketName ?? string.Empty, attachmentToDelete?.ValidName ?? string.Empty);
            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
            await Executor.GetCommand<DeleteAttachmentCommand>().Process(c => c.ExecuteAsync(attachmentToDelete));
        }
    }
}
