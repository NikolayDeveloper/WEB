using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.Search
{
    public class IntellectualPropertySearchDto
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public string Number { get; set; }
        public string NameRu { get; set; }
        public int? ProtectionDocTypeId { get; set; }
        public Owner.Type Type { get; set; }
        public int? TypeId { get; set; }
        public int? RequestTypeId { get; set; }
        public string TypeNameRu { get; set; }
        public bool IsIndustrial { get; set; }
        public int? AddresseeId { get; set; }
        public DicCustomer Addressee { get; set; }
    }
}
