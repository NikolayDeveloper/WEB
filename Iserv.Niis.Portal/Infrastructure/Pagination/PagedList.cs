using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Portal.Infrastructure.Pagination
{
    public class PagedList<T> : IPagedList<T>
    {
        public static readonly int DefaultLimit = 100;
        public PagedList(IQueryable<T> superSet, int limit, int page)
        {
            limit = limit <= 0
                ? DefaultLimit
                : limit;
            page = page < 1
                ? 1
                : page;
            Items = superSet?.Skip((page - 1) * limit)
                        .Take(limit)
                        .AsEnumerable();
            Meta = new Meta(superSet.Count(), Items.Count(), page);
        }
        public Meta Meta { get; }
        public IEnumerable<T> Items { get; }
    }
}
