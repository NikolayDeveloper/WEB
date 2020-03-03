using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Security
{
    public class DocumentAccessPermissions : Entity<int>
    {
        public DocumentAccessPermissions()
        {
            AccessTypes = new HashSet<DicEntityAccessType>();
        }

        public int ClassificationId { get; set; }
        public DicDocumentClassification Classification { get; set; }
        public DicDocumentType Type { get; set; }
        public ICollection<DicEntityAccessType> AccessTypes { get; set; }
    }
}