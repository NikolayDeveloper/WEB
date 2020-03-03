using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Integration
{
    public class IntegrationPaymentCalcMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationPaymentCalc>()
                .ToTable("IntegrationPaymentCalcs");
        }
    }
}
