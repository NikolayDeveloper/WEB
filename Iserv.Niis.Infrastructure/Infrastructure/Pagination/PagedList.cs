using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Infrastructure.Pagination
{
    /// <summary>
    /// Класс, который представляет страницу с данными.
    /// </summary>
    public class PagedList<T> : IPagedList<T>
    {
        #region Константы

        /// <summary>
        /// Максимальное количество элементов на странице по умолчанию.
        /// </summary>
        public const int DefaultLimit = 15;

        /// <summary>
        /// Номер страницы по умолчанию.
        /// </summary>
        public const int DefaultPage = 1;

        /// <summary>
        /// Минимальное значение максимального количества элементов на странице.
        /// </summary>
        public const int MinLimit = 1;

        /// <summary>
        /// Минимальное значение номера страницы.
        /// </summary>
        public const int MinPage = 1;

        #endregion

        #region Свойства
        /// <summary>
        /// Информация о странице.
        /// </summary>
        public Meta Meta { get; }

        /// <summary>
        /// Данные страницы.
        /// </summary>
        public IEnumerable<T> Items { get; set; }
        #endregion

        #region Конструкторы
        /// <summary>
        /// Приватный контруктор, для инициализации объекта <see cref="PagedList{T}"/>
        /// </summary>
        /// <param name="items"> Данные страницы.</param>
        /// <param name="meta"> Информация о странице.</param>
        private PagedList(IEnumerable<T> items, Meta meta)
        {
            Meta = meta;
            Items = items;
        }
        #endregion

        #region Открытые статические методы
        /// <summary>
        /// Метод для создания объекта <see cref="PagedList{T}"/>.
        /// </summary>
        /// <param name="query"> Запрос в бд.</param>
        /// <param name="limit"> Максимальное количество элементов на странице.</param>
        /// <param name="page"> Номер страницы.</param>
        /// <returns>Объект <see cref="PagedList{T}"/></returns>
        public static PagedList<T> Paginate(IQueryable<T> query, int limit, int page)
        {
            ValidatePageParameters(ref limit, ref page);

            var items = GetPage(query, limit, page).ToList();

            return new PagedList<T>(items, CreatePageMeta(query, items, page));
        }

        /// <summary>
        /// Метод для создания объекта <see cref="PagedList{T}"/>.
        /// </summary>
        /// <param name="query"> Запрос в бд.</param>
        /// <param name="limit"> Максимальное количество элементов на странице.</param>
        /// <param name="page"> Номер страницы.</param>
        /// <returns>Объект <see cref="PagedList{T}"/></returns>
        public static async Task<PagedList<T>> PaginateAsync(IQueryable<T> query, int limit, int page)
        {
            ValidatePageParameters(ref limit, ref page);

            var items = await GetPage(query, limit, page).ToListAsync();

            return new PagedList<T>(items, await CreatePageMetaAsync(query, items, page));
        }
        #endregion

        #region Закрытые статические методы
        /// <summary>
        /// Проверяет значение максимального количества элементов на странице и номер страницы на валидность.
        /// В случае невалидности данных, им задаются значения по умолчанию.
        /// </summary>
        /// <param name="limit"> Максиимальное количество элементов на странице.</param>
        /// <param name="page"> Номер страницы.</param>
        private static void ValidatePageParameters(ref int limit, ref int page)
        {
            limit = limit < MinLimit ? DefaultLimit : limit;
            page = page < MinPage ? DefaultPage : page;
        }

        /// <summary>
        /// Создает данные о странице.
        /// </summary>
        /// <param name="query"> Запрос в бд.</param>
        /// <param name="items"> Элементы на странице.</param>
        /// <param name="currentPage"> Номер текущей страницы.</param>
        /// <returns> Информация о странице.</returns>
        private static Meta CreatePageMeta(IQueryable<T> query, IEnumerable<T> items, int currentPage)
        {
            return new Meta(query.Count(), items.Count(), currentPage);
        }

        /// <summary>
        /// Создает данные о странице.
        /// </summary>
        /// <param name="query"> Запрос в бд.</param>
        /// <param name="items"> Элементы на странице.</param>
        /// <param name="currentPage"> Номер текущей страницы.</param>
        /// <returns>Информация о странице.</returns>
        private static async Task<Meta> CreatePageMetaAsync(IQueryable<T> query, IEnumerable<T> items, int currentPage)
        {
            return new Meta(await query.CountAsync(), items.Count(), currentPage);
        }

        /// <summary>
        /// Получает страницу по макимальному допустимому количеству элементов на ней и по ее номеру.
        /// </summary>
        /// <param name="query"> Запрос в бд.</param>
        /// <param name="limit"> Максиимальное количество элементов на странице.</param>
        /// <param name="page"> Номер страницы.</param>
        /// <returns> Страница.</returns>
        private static IQueryable<T> GetPage(IQueryable<T> query, int limit, int page)
        {
            limit = limit < MinLimit ? DefaultLimit : limit;
            page = page < MinPage ? DefaultPage : page;

            return query
                .Skip((page - 1) * limit)
                .Take(limit);
        }
        #endregion
    }
}