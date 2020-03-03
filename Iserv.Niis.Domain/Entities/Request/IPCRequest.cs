using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Request
{
    public class IPCRequest : Entity<int>, IHaveConcurrencyToken
    {
        public int IpcId { get; set; }
        public DicIPC Ipc { get; set; }
        public int RequestId { get; set; }
        public Request Request { get; set; }

        public bool IsMain { get; set; }
    }
}