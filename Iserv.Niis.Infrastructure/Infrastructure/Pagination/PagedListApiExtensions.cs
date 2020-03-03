using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Infrastructure.Pagination
{
    public static class PagedListApiExtensions
    {
        public static IActionResult AsOkObjectResult<T>(this IPagedList<T> pagedList, HttpResponse response)
        {
            return new PagedObjectResult<T>(pagedList, response);
        }
    }
}