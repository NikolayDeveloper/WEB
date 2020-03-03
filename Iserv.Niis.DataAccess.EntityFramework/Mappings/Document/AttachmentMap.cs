using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Document
{
    public class AttachmentMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attachment>()
                .ToTable("Attachments");

            modelBuilder.Entity<Attachment>()
                .HasOne(x => x.MainDocument)
                .WithOne(x => x.MainAttachment)
                .HasForeignKey<Domain.Entities.Document.Document>(x => x.MainAttachmentId);
        }
    }
}