using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Document
{
    public class DocumentCustomerMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentCustomer>()
                .ToTable("Document_Customer");
            modelBuilder.Entity<DocumentCustomer>()
                .HasOne(d => d.Customer);
        }
    }
}