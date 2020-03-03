using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Iserv.Niis.Infrastructure.Pagination
{
    /// <summary>
    /// Класс с методами расширения для облегчения работы с пагинацией.
    /// </summary>
    public static class PagedListExtensions
    {
        /// <summary>
        /// Создает пагинированный список.
        /// </summary>
        /// <typeparam name="T">Тип элементов на странице.</typeparam>
        /// <param name="superSet"></param>
        /// <param name="limit">Максимальное количество элементов на странице.</param>
        /// <param name="page">Номер страницы.</param>
        /// <returns>Пагинированный список.</returns>
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> superSet, int limit, int page)
        {
            return PagedList<T>.Paginate(superSet, limit, page);
        }

        /// <summary>
        /// Создает пагинированный список.
        /// </summary>
        /// <typeparam name="T">Тип элементов на странице.</typeparam>
        /// <param name="superSet"></param>
        /// <param name="parameters">Параметры пагинации.</param>
        /// <returns>Пагинированный список.</returns>
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> superSet, PaginationParams parameters)
        {
            return PagedList<T>.Paginate(superSet, parameters.Limit, parameters.Page);
        }

        /// <summary>
        /// Создает пагинированный список.
        /// </summary>
        /// <typeparam name="T">Тип элементов на странице.</typeparam>
        /// <param name="superSet"></param>
        /// <param name="parameters">Параметры пагинации.</param>
        /// <returns>Пагинированный список.</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superSet,
            PaginationParams parameters)
        {
            return await PagedList<T>.PaginateAsync(superSet, parameters.Limit, parameters.Page);
        }

        /// <summary>
        /// Получает параметры пагинации из url-а.
        /// </summary>
        /// <param name="request">HTTP-запрос.</param>
        /// <returns>Параметры пагинации.</returns>
        public static PaginationParams GetPaginationParams(this HttpRequest request)
        {
            var limit = 0;
            var page = 0;

            if (request.Query.TryGetValue("_limit", out var values))
            {
                int.TryParse(values.First(), out limit);
            }

            if (request.Query.TryGetValue("_page", out values))
            {
                int.TryParse(values.First(), out page);
            }

            return new PaginationParams(limit, page);
        }
    }
}