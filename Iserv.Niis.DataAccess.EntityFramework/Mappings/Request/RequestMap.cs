using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class RequestMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Request.Request>()
                .ToTable("Requests");
            modelBuilder.Entity<Domain.Entities.Request.Request>()
                .HasOne(x => x.RequestInfo)
                .WithOne(x => x.Request)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey<RequestInfo>(x => x.RequestId)
                .IsRequired();
            modelBuilder.Entity<Domain.Entities.Request.Request>()
                .HasMany(x => x.Workflows)
                .WithOne(y => y.Owner)
                .HasForeignKey(y => y.OwnerId);
            modelBuilder.Entity<Domain.Entities.Request.Request>()
                .HasMany(x => x.PaymentInvoices)
                .WithOne(y => y.Request)
                .HasForeignKey(y => y.RequestId);
            modelBuilder.Entity<Domain.Entities.Request.Request>()
                .HasMany(x => x.EarlyRegs)
                .WithOne(y => y.Request)
                .HasForeignKey(y => y.RequestId);
            modelBuilder.Entity<Domain.Entities.Request.Request>()
                .HasMany(x => x.ExpertSearchSimilarities)
                .WithOne(y => y.Request)
                .HasForeignKey(y => y.RequestId);
            modelBuilder.Entity<Domain.Entities.Request.Request>()
		        .HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Domain.Entities.Request.Request>()
                .HasMany(x => x.MediaFiles)
                .WithOne(x => x.Request)
                .HasForeignKey(x => x.RequestId);
            modelBuilder.Entity<Domain.Entities.Request.Request>()
                .HasMany(x => x.PaymentExecutors)
                .WithOne(x => x.Request)
                .HasForeignKey(x => x.RequestId);
        }
    }
}