using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Document
{
    public class DocumentNotificationStatusMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentNotificationStatus>()
                .ToTable("DocumentsNotificationStatuses")
                .HasKey(x => new { x.DocumentId, x.NotificationStatusId });
            modelBuilder.Entity<DocumentNotificationStatus>()
                .HasOne(x => x.Document)
                .WithMany(y => y.NotificationStatuses)
                .HasForeignKey(y => y.DocumentId)
                .IsRequired();
            modelBuilder.Entity<DocumentNotificationStatus>()
                .HasOne(x => x.NotificationStatus)
                .WithMany(y => y.Documents)
                .HasForeignKey(y => y.NotificationStatusId)
                .IsRequired();
        }
    }
}
