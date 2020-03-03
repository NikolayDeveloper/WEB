using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Document
{
    public class DocumentMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Document.Document>()
                .ToTable("Documents");
            modelBuilder.Entity<Domain.Entities.Document.Document>()
                .HasMany(x => x.AdditionalAttachments)
                .WithOne(x => x.Document)
                .HasForeignKey(x => x.DocumentId);
            modelBuilder.Entity<Domain.Entities.Document.Document>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Document)
                .HasForeignKey(x => x.DocumentId);

            modelBuilder.Entity<Domain.Entities.Document.Document>()
                .HasMany(x => x.DocumentLinks)
                .WithOne(x => x.ParentDocument)
                .HasForeignKey(x => x.ParentDocumentId);
            modelBuilder.Entity<Domain.Entities.Document.Document>()
                .HasMany(x => x.DocumentParentLinks)
                .WithOne(x => x.ChildDocument)
                .HasForeignKey(x => x.ChildDocumentId);
        }
    }
}