using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.ProtectionDoc
{
    public class ProtectionDocRedefine : Entity<int>, IHaveConcurrencyToken
    {
        public int ProtectionDocId { get; set; } //doc_id
        public ProtectionDoc ProtectionDoc { get; set; }
        public int? RedefinitionTypeId { get; set; }
        public DicRedefinitionType RedefinitionType { get; set; }
        public DateTimeOffset? RedefinitionDate { get; set; }
        public string Description { get; set; }
        public string DescriptionKz { get; set; }
    }
}