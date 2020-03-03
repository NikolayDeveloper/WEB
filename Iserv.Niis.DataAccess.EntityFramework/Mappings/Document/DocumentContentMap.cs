using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Document
{
    public class DocumentContentMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Document.DocumentContent>()
                .ToTable("DocumentContents");
            modelBuilder.Entity<Domain.Entities.Document.DocumentContent>()
                .HasOne(x => x.Document)
                .WithOne(x => x.Content)
                .HasForeignKey<DocumentContent>(dc => dc.DocumentId);
        }
    }
}