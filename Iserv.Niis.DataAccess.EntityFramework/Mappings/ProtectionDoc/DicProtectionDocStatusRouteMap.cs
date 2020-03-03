using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    /// <summary>
    /// Маппинг связи между статусом охранного документа и маршрутом.
    /// </summary>
    public class DicProtectionDocStatusRouteMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicProtectionDocStatusRoute>()
                .ToTable("DicProtectionDocStatusesRoutes")
                .HasKey(drsr => new { drsr.DicProtectionDocStatusId, drsr.DicRouteId });

            modelBuilder.Entity<DicProtectionDocStatusRoute>()
                .HasOne(dpdsr => dpdsr.DicProtectionDocStatus)
                .WithMany(dpds => dpds.DicProtectionDocStatusesRoutes)
                .HasForeignKey(dpdsr => dpdsr.DicProtectionDocStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DicProtectionDocStatusRoute>()
                .HasOne(dpdsr => dpdsr.DicRoute)
                .WithMany(dr => dr.DicProtectionDocStatusesRoutes)
                .HasForeignKey(dpdsr => dpdsr.DicRouteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
