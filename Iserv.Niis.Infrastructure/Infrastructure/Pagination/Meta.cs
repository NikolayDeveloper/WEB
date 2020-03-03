namespace Iserv.Niis.Infrastructure.Pagination
{
    /// <summary>
    /// Информация о странице.
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// Создает объект <see cref="Meta"/>
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="itemsCount"></param>
        /// <param name="currentPage"></param>
        public Meta(int totalCount, int itemsCount, int currentPage)
        {
            TotalCount = totalCount;
            ItemsCount = itemsCount;
            CurrentPage = currentPage;
        }

        /// <summary>
        /// Общее количество элементов.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Номер текущей страницы.
        /// </summary>
        public int CurrentPage { get; }

        /// <summary>
        /// Количество элементов на текущей странице.
        /// </summary>
        public int ItemsCount { get; }
    }
}
