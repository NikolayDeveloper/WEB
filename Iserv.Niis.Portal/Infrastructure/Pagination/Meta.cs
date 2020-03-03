namespace Iserv.Niis.Portal.Infrastructure.Pagination
{
    public class Meta
    {
        public Meta(int totalCount, int itemsCount, int currentPage)
        {
            TotalCount = totalCount;
            ItemsCount = itemsCount;
            CurrentPage = currentPage;
        }
        public int TotalCount { get; }
        public int CurrentPage { get; }
        public int ItemsCount { get; }
    }
}
