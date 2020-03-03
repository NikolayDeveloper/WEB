using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Iserv.Niis.Portal.Infrastructure.Pagination
{
    public static class PagedListExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> superSet, int limit, int page)
        {
            return new PagedList<T>(superSet, limit, page);
        }

        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> superSet, PaginationParams parameters)
        {
            return new PagedList<T>(superSet, parameters.Limit, parameters.Page);
        }

        //TODO: ПЕРЕНЕСЕНО В ПРОЕКТ Iserv.Niis.Api
        public static PaginationParams GetPaginationParams(this HttpRequest request)
        {
            int limit = 0;
            int page = 0;
            StringValues values;

            if (request.Query.TryGetValue("_limit", out values))
            {
                int.TryParse(values.First(), out limit);
            }
            
            if (request.Query.TryGetValue("_page", out values))
            {
                int.TryParse(values.First(), out page);
            }
            
            limit = limit < 1
                ? PagedList<object>.DefaultLimit
                : limit;
            page = page < 1
                ? 1
                : page;
            return new PaginationParams(limit, page);
        }

    }
}