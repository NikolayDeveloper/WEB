using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Integration
{
    public class IntegrationRequisitionMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationRequisition>()
                .ToTable("IntegrationRequisitions");
            modelBuilder.Entity<IntegrationRequisition>()
                .Property(x => x.Sender)
                .IsRequired();
            modelBuilder.Entity<IntegrationRequisition>()
                .Property(x => x.RequestNumber)
                .IsRequired();
            modelBuilder.Entity<IntegrationRequisition>()
                .Property(x=>x.DateCreate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
