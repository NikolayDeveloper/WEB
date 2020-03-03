using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Contract
{
    public class ContractMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Contract.Contract>()
                .ToTable("Contracts");
            modelBuilder.Entity<Domain.Entities.Contract.Contract>()
                .HasMany(x => x.Workflows)
                .WithOne(y => y.Owner)
                .HasForeignKey(y => y.OwnerId);
            modelBuilder.Entity<Domain.Entities.Contract.Contract>()
                .HasMany(x => x.PaymentInvoices)
                .WithOne(y => y.Contract)
                .HasForeignKey(y => y.ContractId);
        }
    }
}