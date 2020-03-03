using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Integration
{
    public class IntegrationStatusMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationStatus>()
                .ToTable("IntegrationStatuses");
            modelBuilder.Entity<IntegrationStatus>()
                .Property(x=>x.DateCreate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
