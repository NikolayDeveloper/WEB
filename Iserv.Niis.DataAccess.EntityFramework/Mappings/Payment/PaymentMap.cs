using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Payment
{
    public class PaymentMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Payment.Payment>()
                .ToTable("Payments");
            modelBuilder.Entity<Domain.Entities.Payment.Payment>()
                .HasMany(c => c.PaymentUses)
                .WithOne(p => p.Payment)
                .HasForeignKey(p => p.PaymentId)
                .IsRequired(false);
        }
    }
}