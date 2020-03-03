using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    public class ICISProtectionDocMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ICISProtectionDoc>()
                .ToTable("ICIS_ProtectionDoc");
            modelBuilder.Entity<ICISProtectionDoc>()
                .HasOne(x => x.ProtectionDoc)
                .WithMany(pd => pd.IcisProtectionDocs)
                .HasForeignKey(x => x.ProtectionDocId)
                .IsRequired();
            modelBuilder.Entity<ICISProtectionDoc>()
                .HasOne(x => x.Icis);
        }
    }
}