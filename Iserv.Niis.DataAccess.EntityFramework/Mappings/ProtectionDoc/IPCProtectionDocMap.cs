using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    public class IPCProtectionDocMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IPCProtectionDoc>()
                .ToTable("IPC_ProtectionDoc");
            modelBuilder.Entity<IPCProtectionDoc>()
                .HasOne(x => x.ProtectionDoc)
                .WithMany(pd => pd.IpcProtectionDocs)
                .HasForeignKey(x => x.ProtectionDocId)
                .IsRequired();
            modelBuilder.Entity<IPCProtectionDoc>()
                .HasOne(x => x.Ipc);
        }
    }
}