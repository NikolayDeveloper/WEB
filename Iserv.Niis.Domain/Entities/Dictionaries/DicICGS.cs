using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// МЕЖДУНАРОДНАЯ КЛАССИФИКАЦИЯ ТОВАРОВ И УСЛУГ
    /// </summary>
    public class DicICGS : DictionaryEntity<int>, IHaveConcurrencyToken
    {
        public DicICGS()
        {
            DicDetailIcgss = new HashSet<DicDetailICGS>();
        }
        public int RevisionNumber { get; set; }
        public string DescriptionShort { get; set; }
        public ICollection<DicDetailICGS> DicDetailIcgss { get; set; }
    }
}