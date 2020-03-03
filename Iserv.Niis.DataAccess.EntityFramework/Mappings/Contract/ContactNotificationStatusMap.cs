using System;
using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Contract
{
    public class ContactNotificationStatusMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractNotificationStatus>()
                .ToTable("ContractsNotificationStatuses")
                .HasKey(x => new { x.ContractId, x.NotificationStatusId });
            modelBuilder.Entity<ContractNotificationStatus>()
                .HasOne(x => x.NotificationStatus)
                .WithMany(y => y.Contracts)
                .HasForeignKey(y => y.NotificationStatusId)
                .IsRequired();
            modelBuilder.Entity<ContractNotificationStatus>()
                .HasOne(x => x.Contract)
                .WithMany(y => y.NotificationStatuses)
                .HasForeignKey(y => y.ContractId)
                .IsRequired();
        }
    }
}
