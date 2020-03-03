using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Model.Models.Contract
{
    public class ContractItemDto
    {
        public int? Id { get; set; }
        public string ContractNum { get; set; }
        public DateTimeOffset? RegDate { get; set; }
        public DateTimeOffset? GosDate { get; set; }
        public string GosNumber { get; set; }
        public string Initiator { get; set; }
        public string Executor { get; set; }
        public string StatusNameRu { get; set; }
        public string CurrentStageNameRu { get; set; }
        public DateTimeOffset? ValidDate { get; set; }
        public string CategoryNameRu { get; set; }
        public string TypeNameRu { get; set; }
        public string SideOneNameRu { get; set; }
        public string SideTwoNameRu { get; set; }
    }
}
