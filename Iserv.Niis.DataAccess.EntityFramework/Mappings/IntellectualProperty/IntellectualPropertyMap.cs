using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.IntellectualProperty
{
    public class IntellectualPropertyMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.IntellectualProperty.IntellectualProperty>()
                .ToTable("IntellectualProperties");

            modelBuilder.Entity<Domain.Entities.IntellectualProperty.IntellectualProperty>()
                .Property(p => p.DeclaredName)
                .IsRequired();
            //modelBuilder.Entity<Domain.Entities.IntellectualProperty.IntellectualProperty>()
            //    .Property(p => p.Applicants)
            //    .IsRequired();
            //modelBuilder.Entity<Domain.Entities.IntellectualProperty.IntellectualProperty>()
            //    .Property(p => p.Authors)
            //    .IsRequired();
            modelBuilder.Entity<Domain.Entities.IntellectualProperty.IntellectualProperty>()
                .Property(p => p.IssuePatentDate)
                .IsRequired();
            modelBuilder.Entity<Domain.Entities.IntellectualProperty.IntellectualProperty>()
                .Property(p => p.ValidDate)
                .IsRequired();
            modelBuilder.Entity<Domain.Entities.IntellectualProperty.IntellectualProperty>()
                .Property(p => p.ExtensionDate)
                .IsRequired();
            modelBuilder.Entity<Domain.Entities.IntellectualProperty.IntellectualProperty>()
                .Property(p => p.BulletinDate)
                .IsRequired();
            modelBuilder.Entity<Domain.Entities.IntellectualProperty.IntellectualProperty>()
                .HasOne(d => d.ProtectionDoc)
                .WithOne(p => p.IntellectualProperty)
                .HasForeignKey<Domain.Entities.ProtectionDoc.ProtectionDoc>(p => p.IntellectualPropertyId);
        }
    }
}