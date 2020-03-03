using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Integration
{
    public class IntegrationDocumentMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationDocument>()
                .ToTable("IntegrationDocuments");
            modelBuilder.Entity<IntegrationDocument>()
                .Property(i => i.DocumentTypeId)
                .IsRequired();
            modelBuilder.Entity<IntegrationDocument>()
                .Property(i => i.DocumentBarcode)
                .IsRequired();
            modelBuilder.Entity<IntegrationDocument>()
                .Property(i => i.RequestBarcode)
                .IsRequired();
            modelBuilder.Entity<IntegrationDocument>()
                .Property(i => i.DateCreate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}