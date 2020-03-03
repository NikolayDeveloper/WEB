using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Other;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Other
{
    public class SignatureMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Signature>()
                .ToTable("Signatures")
                .HasKey(x=>x.UserId);
            modelBuilder.Entity<Signature>()
                .HasOne(x => x.User)
                .WithOne(x => x.Signature)
                .HasForeignKey<Signature>(x => x.UserId);
        }
    }
}