using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class UserIcgsRelation
    {
        public int UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int IcgsId { get; set; }

        public DicICGS Icgs { get; set; }
    }
}