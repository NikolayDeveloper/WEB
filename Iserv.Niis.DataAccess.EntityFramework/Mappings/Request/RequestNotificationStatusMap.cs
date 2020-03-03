using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class RequestNotificationStatusMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestNotificationStatus>()
                .ToTable("RequestsNotificationStatuses")
                .HasKey(x => new { x.RequestId, x.NotificationStatusId });
            modelBuilder.Entity<RequestNotificationStatus>()
                .HasOne(x => x.NotificationStatus)
                .WithMany(y => y.Requests)
                .HasForeignKey(y => y.NotificationStatusId)
                .IsRequired();
            modelBuilder.Entity<RequestNotificationStatus>()
                .HasOne(x => x.Request)
                .WithMany(y => y.NotificationStatuses)
                .HasForeignKey(y => y.RequestId)
                .IsRequired();
        }
    }
}
