using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.EntitiesHistory.Link;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class RequestRequestRelation : Entity<int>, IHistorySupport
    {
        public int ParentId { get; set; }
        public Request.Request Parent { get; set; }
        public int ChildId { get; set; }
        public Request.Request Child { get; set; }
        public bool? IsAnswer { get; set; }

        public Type GetHistoryEntity()
        {
            return typeof(LinkDocumentHistory);
        }
    }
}