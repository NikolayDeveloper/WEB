using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Dictionaries
{
    public class DicNotificationStatusMap: IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicNotificationStatus>()
                .ToTable("DicNotificationStatuses")
                .HasKey(x => new { x.Id });
            modelBuilder.Entity<DicNotificationStatus>()
                .HasMany(x => x.Contracts)
                .WithOne(y => y.NotificationStatus)
                .HasForeignKey(y => y.NotificationStatusId);
            modelBuilder.Entity<DicNotificationStatus>()
                .HasMany(x => x.Requests)
                .WithOne(y => y.NotificationStatus)
                .HasForeignKey(y => y.NotificationStatusId);
            modelBuilder.Entity<DicNotificationStatus>()
                .HasMany(x => x.Documents)
                .WithOne(y => y.NotificationStatus)
                .HasForeignKey(y => y.NotificationStatusId);
        }
    }
}