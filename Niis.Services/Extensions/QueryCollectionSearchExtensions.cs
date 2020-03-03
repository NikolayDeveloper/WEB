using Microsoft.AspNetCore.Http;

namespace Iserv.Niis.Services.Extensions
{
    /// <summary>
    /// Класс с методами расширения для легкого получения значений из <see cref="IQueryCollection"/>.
    /// </summary>
    internal static class QueryCollectionSearchExtensions
    {
        /// <summary>
        /// Нужно ли искать полезные модели.
        /// </summary>
        /// <param name="queryCollection">Коллекция значений в строке адреса.</param>
        /// <returns>Нужно искать полезные модели.</returns>
        public static bool WillSearchUsefulModels(this IQueryCollection queryCollection)
        {
            return queryCollection.ContainsKey("and_searchUsefulModels_eq");
        }
    }
}
