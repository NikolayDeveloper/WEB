using System.Collections.Generic;

namespace Iserv.Niis.Infrastructure.Pagination
{
    public interface IPagedList<T>
    {
        Meta Meta { get; }
        IEnumerable<T> Items { get; set; }
    }
}
