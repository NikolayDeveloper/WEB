using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Attachments
{
    public class GetAttachmentFileQuery: BaseQuery
    {
        private readonly IFileStorage _fileStorage;

        public GetAttachmentFileQuery(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public async Task<byte[]> Execute(int attachmentId)
        {
            var repo = Uow.GetRepository<Attachment>();
            var attachment = repo.AsQueryable()
                .FirstOrDefault(r => r.Id == attachmentId);
            var file = await _fileStorage.GetAsync(attachment?.BucketName, attachment?.OriginalName);
            return file;
        }
    }
}
