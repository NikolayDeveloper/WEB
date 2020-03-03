using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Infrastructure.Pagination
{
    public class PagedObjectResult<T> : ObjectResult
    {
        public PagedObjectResult(IPagedList<T> pagedList, HttpResponse response) : base(pagedList.Items)
        {
            response.Headers.Add("x-items-count", pagedList.Meta.ItemsCount.ToString());
            response.Headers.Add("x-total-count", pagedList.Meta.TotalCount.ToString());
            response.Headers.Add("x-current-page", pagedList.Meta.CurrentPage.ToString());

        }
    }
}
