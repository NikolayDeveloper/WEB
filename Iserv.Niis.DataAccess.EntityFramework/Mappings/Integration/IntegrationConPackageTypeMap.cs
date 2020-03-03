using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Integration
{
    /// <summary> Тип интеграционного пакета ЦОН </summary>
    public class IntegrationConPackageTypeMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationConPackageType>()
                .ToTable("IntegrationConPackageTypes");
        }
    }
}