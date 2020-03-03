using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings
{
    public class SearchViewEntityMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchViewEntity>(entity =>
            {
                entity.ToTable("SearchView");

                entity.Property(e => e.OwnerType)
                    .IsRequired()
                    .HasColumnName("OwnerType");
                //.HasColumnType("int")
                //.HasDefaultValueSql("1");

                entity.Property(e => e.DocumentType)
                    .HasColumnName("DocumentType");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasColumnName("Barcode")
                    .ValueGeneratedNever();

                entity.Property(e => e.Num)
                    .HasColumnName("Num");

                entity.Property(e => e.Date)
                    .HasColumnName("Date");

                entity.Property(e => e.Description)
                    .HasColumnName("Description");

                entity.Property(e => e.Xin)
                    .HasColumnName("Xin");

                entity.Property(e => e.Customer)
                    .HasColumnName("Customer");

                entity.Property(e => e.Address)
                    .HasColumnName("Address");

                entity.Property(e => e.CountryId)
                    .HasColumnName("CountryId");

                entity.Property(e => e.CountryNameRu)
                    .HasColumnName("CountryNameRu");

                entity.Property(e => e.ReceiveTypeId)
                    .HasColumnName("ReceiveTypeId");

                entity.Property(e => e.ReceiveTypeNameRu)
                    .HasColumnName("ReceiveTypeNameRu");
            });

            modelBuilder.Entity<SearchRequestViewEntity>(entity => { entity.ToTable("SearchRequestsView"); });
            modelBuilder.Entity<SearchProtectionDocViewEntity>(entity => { entity.ToTable("SearchProtectionDocsView"); });
            modelBuilder.Entity<SearchContractViewEntity>(entity => { entity.ToTable("SearchContractsView"); });
        }
    }
}