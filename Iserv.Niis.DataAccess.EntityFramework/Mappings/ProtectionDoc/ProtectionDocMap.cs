using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    public class ProtectionDocMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.ProtectionDoc.ProtectionDoc>()
                .ToTable("ProtectionDocs");
            modelBuilder.Entity<Domain.Entities.ProtectionDoc.ProtectionDoc>()
                .HasOne(x => x.ProtectionDocInfo)
                .WithOne(x => x.ProtectionDoc)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey<ProtectionDocInfo>(x => x.ProtectionDocId)
                .IsRequired();
            modelBuilder.Entity<Domain.Entities.ProtectionDoc.ProtectionDoc>()
                .HasMany(x => x.Workflows)
                .WithOne(y => y.Owner)
                .HasForeignKey(y => y.OwnerId);
            modelBuilder.Entity<Domain.Entities.ProtectionDoc.ProtectionDoc>()
                .HasMany(x => x.PaymentInvoices)
                .WithOne(y => y.ProtectionDoc)
                .HasForeignKey(y => y.ProtectionDocId)
                .IsRequired(false);
            modelBuilder.Entity<Domain.Entities.ProtectionDoc.ProtectionDoc>()
                .HasMany(x => x.EarlyRegs)
                .WithOne(y => y.ProtectionDoc)
                .HasForeignKey(y => y.ProtectionDocId);
            modelBuilder.Entity<Domain.Entities.ProtectionDoc.ProtectionDoc>()
                .HasMany(x => x.MediaFiles)
                .WithOne(x => x.ProtectionDoc)
                .HasForeignKey(x => x.ProtectionDocId);
            modelBuilder.Entity<Domain.Entities.ProtectionDoc.ProtectionDoc>()
                .Property(pd => pd.DateOfCreation)
                .HasDefaultValueSql("getdate()");
        }
    }
}