using Iserv.Niis.Domain.Abstract;
using System.Linq;

namespace Iserv.Niis.Report.ReportFilter
{
    internal static class QueryFilterSort
    {
        internal static IQueryable<T> AddSorting<T>(this IQueryable<T> query)
            where T : Entity<int>, new()
        {
            //query = query.Skip(itemsPerCount * (pageNumber - 1)).Take(itemsPerCount);

            return query;
        }
    }
}
