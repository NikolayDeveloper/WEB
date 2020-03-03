using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Integration
{
    /// <summary> Статус услуги ЦОН </summary>
    public class IntegrationConServiceStatusMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationConServiceStatus>()
                .ToTable("IntegrationConServiceStatuses");
        }
    }
}