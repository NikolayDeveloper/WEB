using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings
{
    public class ExpertSearchViewEntityMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpertSearchViewEntity>(entity =>
            {
                entity.ToTable("ExpertSearchView");

                entity.Property(e => e.OwnerType)
                    .IsRequired()
                    .HasColumnName("OwnerType");

                entity.Property(e => e.ProtectionDocTypeId)
                    .HasColumnName("ProtectionDocTypeId");

                /*entity.Property(e => e.SearchStatus)
                    .IsRequired()
                    .HasColumnName("SearchStatus");*/

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("Id")
                    .ValueGeneratedNever();

                /*entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasColumnName("Barcode")
                    .ValueGeneratedNever();

                entity.Property(e => e.RequestTypeId).HasColumnName("RequestTypeId");
                entity.Property(e => e.RequestTypeNameRu).HasColumnName("RequestTypeNameRu");
                entity.Property(e => e.StatusId).HasColumnName("StatusId");
                entity.Property(e => e.StatusCode).HasColumnName("StatusCode");
                entity.Property(e => e.PreviewImage).HasColumnName("PreviewImage");
                entity.Property(e => e.StatusNameRu).HasColumnName("StatusNameRu");
                entity.Property(e => e.GosNumber).HasColumnName("GosNumber");
                entity.Property(e => e.GosDate).HasColumnName("GosDate");
                entity.Property(e => e.RequestNum).HasColumnName("RequestNum");
                entity.Property(e => e.RequestDate).HasColumnName("RequestDate");
                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.NameRu).HasColumnName("NameRu");
                entity.Property(e => e.NameKz).HasColumnName("NameKz");
                entity.Property(e => e.NameEn).HasColumnName("NameEn");
                entity.Property(e => e.Declarant).HasColumnName("Declarant");
                entity.Property(e => e.Owner).HasColumnName("Owner");
                entity.Property(e => e.PatentAttorney).HasColumnName("PatentAttorney");
                entity.Property(e => e.Author).HasColumnName("Author");
                entity.Property(e => e.PatentOwner).HasColumnName("PatentOwner");
                entity.Property(e => e.AddressForCorrespondence).HasColumnName("AddressForCorrespondence");
                entity.Property(e => e.Confidant).HasColumnName("Confidant");
                entity.Property(e => e.ReceiveTypeId).HasColumnName("ReceiveTypeId");
                entity.Property(e => e.ReceiveTypeNameRu).HasColumnName("ReceiveTypeNameRu");
                entity.Property(e => e.Icgs).HasColumnName("Icgs");
                entity.Property(e => e.Icfems).HasColumnName("Icfems");
                entity.Property(e => e.Icis).HasColumnName("Icis");
                entity.Property(e => e.Ipcs).HasColumnName("Ipcs");
                entity.Property(e => e.Transliteration).HasColumnName("Transliteration");
                entity.Property(e => e.PriorityRegCountryNames).HasColumnName("PriorityRegCountryNames");
                entity.Property(e => e.PriorityRegNumbers).HasColumnName("PriorityRegNumbers");
                entity.Property(e => e.PriorityData).HasColumnName("PriorityData");
                entity.Property(e => e.NumberBulletin).HasColumnName("NumberBulletin");
                entity.Property(e => e.PublicDate).HasColumnName("PublicDate");
                entity.Property(e => e.ValidDate).HasColumnName("ValidDate");
                entity.Property(e => e.ExtensionDateTz).HasColumnName("ExtensionDateTz");
                entity.Property(e => e.TransferDate).HasColumnName("TransferDate");
                entity.Property(e => e.EarlyTerminationDate).HasColumnName("EarlyTerminationDate");
                entity.Property(e => e.Formula).HasColumnName("Formula");
                entity.Property(e => e.Referat).HasColumnName("Referat");
                entity.Property(e => e.Description).HasColumnName("Description");
                entity.Property(e => e.DisclaimerRu).HasColumnName("DisclaimerRu");
                entity.Property(e => e.DisclaimerKz).HasColumnName("DisclaimerKz");*/
            });

            modelBuilder.Entity<ExpertSearchViewEntity>()
                .HasOne(x => x.Request);

            modelBuilder.Entity<ExpertSearchViewEntity>()
                .HasOne(x => x.ProtectionDoc);
        }
    }
}