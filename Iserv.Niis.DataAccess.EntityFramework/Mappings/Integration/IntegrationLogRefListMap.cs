using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Integration
{
    public class IntegrationLogRefListMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationLogRefList>()
                .ToTable("IntegrationLogRefLists");
            modelBuilder.Entity<IntegrationLogRefList>()
                .Property(x => x.RefName)
                .IsRequired();
            modelBuilder.Entity<IntegrationLogRefList>()
                .Property(x => x.SqLquery)
                .IsRequired();

        }
    }
}
