using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    public class ICGSProtectionDocMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ICGSProtectionDoc>()
                .ToTable("ICGS_ProtectionDoc");
            modelBuilder.Entity<ICGSProtectionDoc>()
                .HasOne(x => x.ProtectionDoc)
                .WithMany(pd => pd.IcgsProtectionDocs)
                .HasForeignKey(x => x.ProtectionDocId)
                .IsRequired();
            modelBuilder.Entity<ICGSProtectionDoc>()
                .HasOne(x => x.Icgs);
        }
    }
}