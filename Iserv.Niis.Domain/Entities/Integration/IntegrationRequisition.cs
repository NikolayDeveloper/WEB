using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.Integration
{
    public class IntegrationRequisition
    {
        public int Id { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public int RequestBarcode { get; set; }
        public string Sender { get; set; }
        public int OnlineRequisitionStatusId { get; set; }
        public DicOnlineRequisitionStatus OnlineRequisitionStatus { get; set; }
        public string RequestNumber { get; set; }
        public int ProtectionDocTypeId { get; set; }
        public DicProtectionDocType ProtectionDocType { get; set; }
        public string StatusURL { get; set; }
        public string Callback { get; set; }
        public bool? IsCallbackProcessed { get; set; }
        public string ChainId { get; set; }
        public string Xml { get; set; }
        public string Note { get; set; }
    }
}
