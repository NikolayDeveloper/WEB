namespace Iserv.Niis.Infrastructure.Pagination
{
    public class PaginationParams
    {
        public PaginationParams(int limit, int page)
        {
            Limit = limit;
            Page = page;
        }
        public int Limit { get; }
        public int Page { get; }
    }
}
