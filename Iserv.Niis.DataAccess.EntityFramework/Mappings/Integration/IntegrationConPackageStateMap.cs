using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Integration
{
    /// <summary> Статус интеграционного пакета ЦОН </summary>
    public class IntegrationConPackageStateMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationConPackageState>()
                .ToTable("IntegrationConPackageStates");
        }
    }
}