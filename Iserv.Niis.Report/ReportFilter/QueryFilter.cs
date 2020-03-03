using Iserv.Niis.Domain.Abstract;
using System.Linq;

namespace Iserv.Niis.Report.ReportFilter
{
    internal static class QueryFilter
    {
        internal static IQueryable<T> AddReportFilter<T>(this IQueryable<T> query, ReportFilterData reportFilterData)
            where T : Entity<int>, new()
        {

            //TODO: Not implemented
            query = query.AddSorting();

            query = query.AddPaging(reportFilterData.ItemsCountPerPage, reportFilterData.PageNumber);

            return query;
        }
    }
}
