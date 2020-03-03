using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class DicColorTZRequestRelationMap : IMapBuilder

    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicColorTZRequestRelation>()
                .ToTable("DicColorTZ_Request")
                .HasKey(x => new { x.ColorTzId, x.RequestId });
            modelBuilder.Entity<DicColorTZRequestRelation>()
                .HasOne(x => x.ColorTz);
            modelBuilder.Entity<DicColorTZRequestRelation>()
                .HasOne(x => x.Request)
                .WithMany(m => m.ColorTzs)
                .HasForeignKey(x => x.RequestId)
                .IsRequired();
        }
    }
}