using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    public class ProtectionDocCustomerMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProtectionDocCustomer>()
                .ToTable("ProtectionDocCustomers");
            modelBuilder.Entity<ProtectionDocCustomer>()
                .HasOne(x => x.ProtectionDoc)
                .WithMany(pd => pd.ProtectionDocCustomers)
                .HasForeignKey(x => x.ProtectionDocId);
            modelBuilder.Entity<ProtectionDocCustomer>()
                .HasOne(x => x.ProtectionDoc);
        }
    }
}