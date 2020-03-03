using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    public class DicIcfemProtectionDocRelationMap : IMapBuilder

    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicIcfemProtectionDocRelation>()
                .ToTable("DicIcfem_ProtectionDoc")
                .HasKey(x => new { x.DicIcfemId, x.ProtectionDocId });
            modelBuilder.Entity<DicIcfemProtectionDocRelation>()
                .HasOne(x => x.DicIcfem);
            modelBuilder.Entity<DicIcfemProtectionDocRelation>()
                .HasOne(x => x.ProtectionDoc)
                .WithMany(m => m.Icfems)
                .HasForeignKey(x => x.ProtectionDocId)
                .IsRequired();
        }
    }
}