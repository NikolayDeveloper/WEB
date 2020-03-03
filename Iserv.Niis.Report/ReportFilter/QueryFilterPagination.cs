using Iserv.Niis.Domain.Abstract;
using System.Linq;

namespace Iserv.Niis.Report.ReportFilter
{
    internal static class QueryFilterPagination
    {
        internal static IQueryable<T> AddPaging<T>(this IQueryable<T> query, int itemsPerCount, int pageNumber)
            where T : Entity<int>, new()
        {
            query = query.Skip(itemsPerCount * (pageNumber - 1)).Take(itemsPerCount);

            return query;
        }
    }
}
