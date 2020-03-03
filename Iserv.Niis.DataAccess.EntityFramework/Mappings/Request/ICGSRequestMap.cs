using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class ICGSRequestMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ICGSRequest>()
                .ToTable("ICGS_Request");
            modelBuilder.Entity<ICGSRequest>()
                .HasOne(x => x.Request)
                .WithMany(pd => pd.ICGSRequests)
                .HasForeignKey(x => x.RequestId)
                .IsRequired();
            modelBuilder.Entity<ICGSRequest>()
                .HasOne(x => x.Icgs);
        }
    }
}