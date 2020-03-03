using System.Threading.Tasks;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.ExpertSearch;
using Microsoft.AspNetCore.Http;

namespace Iserv.Niis.Services.Interfaces
{
    /* Внимание, прошу выносить логику, связанную с поиском, из контроллеров в этот сервис, чтобы
     * у нас была хоть какая-то реюзабельность.
     */

    /// <summary>
    /// Сервис, отвечающий за поиск заявок, ОД и договоров.
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Производит поиск по заявкам и ОД изобретений.
        /// </summary>
        /// <param name="request">HTTP-запрос из <see cref="HttpRequest.Query"/> которого берутся данные о фильтрах для поиска.</param>
        /// <returns>Список найденных изобретений.</returns>
        Task<IPagedList<InventionSearchDto>> SearchInventions(HttpRequest request);
    }
}
