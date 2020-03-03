using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.AccountingData
{
    public class CustomerAttorneyInfoMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerAttorneyInfo>()
                .ToTable("CustomerAttorneyInfos");
        }
    }
}