using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    public class DicColorTZProtectionDocRelationMap : IMapBuilder

    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicColorTZProtectionDocRelation>()
                .ToTable("DicColorTZ_ProtectionDoc")
                .HasKey(x => new { x.ColorTzId, x.ProtectionDocId });
            modelBuilder.Entity<DicColorTZProtectionDocRelation>()
                .HasOne(x => x.ColorTz);
            modelBuilder.Entity<DicColorTZProtectionDocRelation>()
                .HasOne(x => x.ProtectionDoc)
                .WithMany(m => m.ColorTzs)
                .HasForeignKey(x => x.ProtectionDocId)
                .IsRequired();
        }
    }
}