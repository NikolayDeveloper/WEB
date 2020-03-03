using Iserv.Niis.Domain.Entities.Document;
using Iserv.OldNiis.DataAccess.EntityFramework;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisAttachmentFileService
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisAttachmentFileService(NiisWebContextMigration context)
        {
            _context = context;
        }

        public void CreateAttachment(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
            _context.SaveChanges();
        }
    }
}
