using System;
using System.ComponentModel.DataAnnotations.Schema;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.EntitiesHistory.Link;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class DocumentDocumentRelation : Entity<int>, IHistorySupport
    {
        public int ParentId { get; set; }
        public Document.Document Parent { get; set; }
        public int ChildId { get; set; }
        public Document.Document Child { get; set; }
        public bool? IsAnswer { get; set; }

        public Type GetHistoryEntity()
        {
            return typeof(LinkDocumentHistory);
        }
    }
}