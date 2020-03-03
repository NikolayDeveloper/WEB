using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class RequestDocumentMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestDocument>()
                .ToTable("RequestsDocuments");
            modelBuilder.Entity<RequestDocument>()
                .HasOne(x => x.Document)
                .WithMany(y => y.Requests)
                .HasForeignKey(y => y.DocumentId)
                .IsRequired();
            modelBuilder.Entity<RequestDocument>()
                .HasOne(x => x.Request)
                .WithMany(y => y.Documents)
                .HasForeignKey(y => y.RequestId)
                .IsRequired();
        }
    }
}