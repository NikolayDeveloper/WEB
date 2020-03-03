using System.Collections.Generic;

namespace Iserv.Niis.Portal.Infrastructure.Pagination
{
    public interface IPagedList<out T>
    {
        Meta Meta { get; }
        IEnumerable<T> Items { get; }
    }
}
