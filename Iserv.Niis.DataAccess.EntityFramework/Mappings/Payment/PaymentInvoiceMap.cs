using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Payment
{
    public class PaymentInvoiceMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentInvoice>()
                .ToTable("PaymentInvoices");
            modelBuilder.Entity<PaymentInvoice>()
                .HasOne(x => x.CreateUser);
            modelBuilder.Entity<PaymentInvoice>()
                .HasOne(x => x.WhoBoundUser);
            modelBuilder.Entity<PaymentInvoice>()
                .HasOne(x => x.WriteOffUser);
            modelBuilder.Entity<PaymentInvoice>()
                .HasMany(x => x.PaymentUses)
                .WithOne(u => u.PaymentInvoice)
                .HasForeignKey(x => x.PaymentInvoiceId)
                .IsRequired(false);
        }
    }
}