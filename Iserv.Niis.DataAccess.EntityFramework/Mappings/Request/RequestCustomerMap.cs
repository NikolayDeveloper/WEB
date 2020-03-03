using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class RequestCustomerMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestCustomer>()
                .ToTable("RequestCustomers");
            modelBuilder.Entity<RequestCustomer>()
                .HasOne(x => x.Request)
                .WithMany(pd => pd.RequestCustomers)
                .HasForeignKey(x => x.RequestId);
        }
    }
}