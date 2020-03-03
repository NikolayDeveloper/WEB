using System;
using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Dictionaries
{
    public class DicCustomerMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicCustomer>()
                .Property(c => c.Address).HasMaxLength(Int32.MaxValue);
            modelBuilder.Entity<DicCustomer>()
               .Property(c => c.ShortAddress).HasMaxLength(Int32.MaxValue);
            modelBuilder.Entity<DicCustomer>()
                .HasMany(c => c.CustomerAttorneyInfos)
                .WithOne(c => c.Customer);
            modelBuilder.Entity<DicCustomer>()
                .Property(c => c.IsBeneficiary)
                .HasDefaultValue(false);
            modelBuilder.Entity<DicCustomer>()
                .Property(c => c.IsNotMention)
                .HasDefaultValue(false);
            modelBuilder.Entity<DicCustomer>()
                .HasOne(d => d.Country)
                .WithMany(p => p.DicCustomers)
                .HasForeignKey(d => d.CountryId);
            ;
        }
    }
}