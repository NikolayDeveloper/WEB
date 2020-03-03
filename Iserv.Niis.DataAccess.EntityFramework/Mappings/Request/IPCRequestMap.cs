using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class IPCRequestMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IPCRequest>()
                .ToTable("IPC_Request");
            modelBuilder.Entity<IPCRequest>()
                .HasOne(x => x.Request)
                .WithMany(pd => pd.IPCRequests)
                .HasForeignKey(x => x.RequestId)
                .IsRequired();
            modelBuilder.Entity<IPCRequest>()
                .HasOne(x => x.Ipc);
        }
    }
}