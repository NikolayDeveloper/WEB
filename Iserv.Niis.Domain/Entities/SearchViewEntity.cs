using System;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Domain.Entities
{
    public class SearchViewEntity
    {
        public Owner.Type OwnerType { get; set; }
        public DocumentType? DocumentType { get; set; }
        public int Id { get; set; }
        public int Barcode { get; set; }
        public string Num { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string Description { get; set; }
        public string Xin { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public int? CountryId { get; set; }
        public string CountryNameRu { get; set; }
        public int? ReceiveTypeId { get; set; }
        public string ReceiveTypeNameRu { get; set; }
    }
}