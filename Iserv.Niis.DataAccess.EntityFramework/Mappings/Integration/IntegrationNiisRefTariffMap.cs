using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Integration
{
    public class IntegrationNiisRefTariffMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationNiisRefTariff>()
                .ToTable("IntegrationNiisRefTariffs");
            modelBuilder.Entity<IntegrationNiisRefTariff>()
                .Property(x => x.Text)
                .IsRequired();
        }
    }
}
