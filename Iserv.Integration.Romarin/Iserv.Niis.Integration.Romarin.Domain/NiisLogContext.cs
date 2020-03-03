using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Integration.Romarin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Integration.Romarin.Domain
{
    public class NiisLogContext : DbContext
    {
        public NiisLogContext(DbContextOptions options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IntegrationRomarinLog>().ToTable("IntegrationRomarinLog");
        }

        public DbSet<IntegrationRomarinLog> IntegrationRomarinLogs { get; set; }
    }
}
