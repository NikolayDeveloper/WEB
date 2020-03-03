using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    /// <summary>
    /// Маппинг связи между статусом заявки и маршрутом.
    /// </summary>
    public class DicRequestStatusRouteMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicRequestStatusRoute>()
                .ToTable("DicRequestStatusesRoutes")
                .HasKey(drsr => new {drsr.DicRequestStatusId, drsr.DicRouteId});

            modelBuilder.Entity<DicRequestStatusRoute>()
                .HasOne(drsr => drsr.DicRequestStatus)
                .WithMany(drs => drs.DicRequestStatusesRoutes)
                .HasForeignKey(drsr => drsr.DicRequestStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DicRequestStatusRoute>()
                .HasOne(drsr => drsr.DicRoute)
                .WithMany(dr => dr.DicRequestStatusesRoutes)
                .HasForeignKey(drsr => drsr.DicRouteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
