using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.ProtectionDoc
{
    public class IPCProtectionDoc : Entity<int>, IHaveConcurrencyToken
    {
        public int IpcId { get; set; }
        public DicIPC Ipc { get; set; }
        public int ProtectionDocId { get; set; }
        public ProtectionDoc ProtectionDoc { get; set; }

        public bool IsMain { get; set; }
    }
}