using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.Integration
{
    public class IntegrationStatus
    {
        public int Id { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public DateTimeOffset? DateSent { get; set; }
        public int RequestBarcode { get; set; }
        public int OnlineRequisitionStatusId { get; set; }
        public DicOnlineRequisitionStatus OnlineRequisitionStatus { get; set; }
        public string AdditionalInfo { get; set; }
        public string Note { get; set; }

        public override string ToString()
        {
            return $"{nameof(RequestBarcode)} = {RequestBarcode}, {nameof(OnlineRequisitionStatusId)} = {OnlineRequisitionStatusId}";
        }
    }
}
