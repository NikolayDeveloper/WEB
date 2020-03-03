using Iserv.Niis.ExternalServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.ExternalServices.DataAccess.EntityFramework
{
    public class NiisIntegrationContext : DbContext
    {
        public NiisIntegrationContext(DbContextOptions<NiisIntegrationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reference>().HasKey(u => new {u.Id, u.Type});
            modelBuilder.Entity<IntegrationLogString>().HasKey(u => new {u.Id, u.Index});
            modelBuilder.Entity<IntegrationCalcPatent>().HasKey(u => new {u.ActionId, u.PatentId});
            modelBuilder.Entity<IntegrationCalcLink>()
                .HasKey(u => new {u.ActionId, u.PatentId, u.CustomerId, u.LinkId, u.SecondPass});
            modelBuilder.Entity<IntegrationCalcHistory>().HasKey(u => new {u.ActionId, u.HistoryId});
            modelBuilder.Entity<IntegrationCalcCustomer>().HasKey(u => new {u.ActionId, u.SecondPass, u.CustomerId});

            modelBuilder.Entity<LogSystemInfo>().Property(x => x.DbDateTime).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<LogAction>().Property(x => x.DbDateTime).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<IntegrationMonitorLog>().Property(x => x.DbDateTime).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<IntegrationLogAction>().Property(x => x.ActionDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<IntegrationHistorySent>().Property(x => x.Date).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<IntegrationBulletin>().Property(x => x.Date).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<IntegrationBulletin>().Property(x => x.Sent).HasDefaultValue(false);
        }

        #region DBSets

        public DbSet<IntegrationBulletin> IntegrationBulletins { get; set; }
        public DbSet<IntegrationCalcCustomer> IntegrationCalcCustomers { get; set; }
        public DbSet<IntegrationCalcHistory> IntegrationCalcHistories { get; set; }
        public DbSet<IntegrationCalcLink> IntegrationCalcLinks { get; set; }
        public DbSet<IntegrationCalcPatent> IntegrationCalcPatents { get; set; }
        public DbSet<IntegrationHistory> IntegrationHistories { get; set; }
        public DbSet<IntegrationHistorySent> IntegrationHistorySents { get; set; }
        public DbSet<IntegrationLogAction> IntegrationLogActions { get; set; }
        public DbSet<IntegrationLogDigitalSignature> IntegrationLogDigitalSignatures { get; set; }
        public DbSet<IntegrationLogString> IntegrationLogStrings { get; set; }
        public DbSet<IntegrationLogSystemInfo> IntegrationLogSystemInfos { get; set; }
        public DbSet<IntegrationMonitorLog> IntegrationMonitorLogs { get; set; }
        public DbSet<LogAction> LogActions { get; set; }
        public DbSet<LogSystemInfo> LogSystemInfos { get; set; }
        public DbSet<Reference> References { get; set; }

        #endregion
    }
}