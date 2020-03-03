using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Other
{
    public class RoleRouteStageRelationMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleRouteStageRelation>()
                .ToTable("Role_RouteStage")
                .HasKey(x => new { x.RoleId, x.StageId });
            modelBuilder.Entity<RoleRouteStageRelation>()
                .HasOne(x => x.Role)
                .WithMany(m => m.Stages)
                .HasForeignKey(x => x.RoleId)
                .IsRequired();
            modelBuilder.Entity<RoleRouteStageRelation>()
                .HasOne(x => x.Stage);
        }
    }
}