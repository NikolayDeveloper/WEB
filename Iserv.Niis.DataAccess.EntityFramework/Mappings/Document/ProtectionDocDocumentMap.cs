using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Document
{
    public class ProtectionDocDocumentMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProtectionDocDocument>()
                .ToTable("ProtectionDocDocuments");
            modelBuilder.Entity<ProtectionDocDocument>()
                .HasOne(x => x.Document)
                .WithMany(y => y.ProtectionDocs)
                .HasForeignKey(x => x.DocumentId)
                .IsRequired();
            modelBuilder.Entity<ProtectionDocDocument>()
                .HasOne(x => x.ProtectionDoc)
                .WithMany(y => y.Documents)
                .HasForeignKey(x => x.ProtectionDocId)
                .IsRequired();
        }
    }
}