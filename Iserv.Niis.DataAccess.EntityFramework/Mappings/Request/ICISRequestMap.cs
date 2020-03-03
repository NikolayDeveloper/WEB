using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class ICISRequestMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ICISRequest>()
                .ToTable("ICIS_Request");
            modelBuilder.Entity<ICISRequest>()
                .HasOne(x => x.Request)
                .WithMany(pd => pd.ICISRequests)
                .HasForeignKey(x => x.RequestId)
                .IsRequired();
            modelBuilder.Entity<ICISRequest>()
                .HasOne(x => x.Icis);
        }
    }
}