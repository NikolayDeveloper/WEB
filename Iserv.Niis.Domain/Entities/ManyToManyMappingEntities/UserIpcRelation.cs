using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class UserIpcRelation
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int IpcId { get;set;}
        public DicIPC Ipc { get;set;}

    }
}
