using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Security
{
    public class ClaimConstant : Entity<int>
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameKz { get; set; }
    }
}
