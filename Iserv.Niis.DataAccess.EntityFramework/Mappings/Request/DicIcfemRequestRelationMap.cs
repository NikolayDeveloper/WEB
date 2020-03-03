using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class DicIcfemRequestRelationMap : IMapBuilder

    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicIcfemRequestRelation>()
                .ToTable("DicIcfem_Request")
                .HasKey(x => new {x.DicIcfemId, x.RequestId});
            modelBuilder.Entity<DicIcfemRequestRelation>()
                .HasOne(x => x.DicIcfem);
            modelBuilder.Entity<DicIcfemRequestRelation>()
                .HasOne(x => x.Request)
                .WithMany(m => m.Icfems)
                .HasForeignKey(x => x.RequestId)
                .IsRequired();
        }
    }
}