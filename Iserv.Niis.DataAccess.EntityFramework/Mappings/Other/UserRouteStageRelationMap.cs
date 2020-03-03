using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Other
{
    public class UserRouteStageRelationMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRouteStageRelation>()
                .ToTable("User_RouteStage")
                .HasKey(x => new { x.UserId, x.StageId });
            modelBuilder.Entity<UserRouteStageRelation>()
                .HasOne(x => x.User)
                .WithMany(m => m.Stages)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
            modelBuilder.Entity<UserRouteStageRelation>()
                .HasOne(x => x.Stage);
        }
    }
}