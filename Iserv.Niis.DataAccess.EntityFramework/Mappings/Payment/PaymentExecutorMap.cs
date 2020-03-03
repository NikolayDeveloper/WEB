using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Payment
{
    public class PaymentExecutorMap: IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentExecutor>()
                .Property(pe => pe.IsCharged)
                .HasDefaultValue(false);
        }
    }
}
