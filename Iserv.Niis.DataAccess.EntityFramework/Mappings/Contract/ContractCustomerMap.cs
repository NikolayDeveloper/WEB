using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Contract
{
    public class ContractCustomerMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractCustomer>()
                .ToTable("ContractCustomers");
            modelBuilder.Entity<ContractCustomer>()
                .HasOne(x => x.Contract)
                .WithMany(pd => pd.ContractCustomers)
                .HasForeignKey(x => x.ContractId);
        }
    }
}