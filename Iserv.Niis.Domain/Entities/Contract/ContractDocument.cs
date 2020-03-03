using Iserv.Niis.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Domain.Entities.Contract
{
    public class ContractDocument : Entity<int>
    {
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
        public int DocumentId { get; set; }
        public Document.Document Document { get; set; }
    }
}
