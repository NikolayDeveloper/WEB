using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class ContractProtectionDocRelation: Entity<int>
    {
        public int ContractId { get; set; }
        public Contract.Contract Contract { get; set; }
        public int ProtectionDocId { get; set; }
        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }

        public ICollection<ContractProtectionDocICGSProtectionDocRelation> ContractProtectionDocICGSProtectionDocs { get; set; }
    }
}