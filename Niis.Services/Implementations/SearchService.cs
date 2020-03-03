using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes;
using Iserv.Niis.BusinessLogic.Search;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.ExpertSearch;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Services.Extensions;
using Microsoft.AspNetCore.Http;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Services.Implementations
{
    /* Внимание, прошу выносить логику, связанную с поиском, из контроллеров в этот сервис, чтобы
     * у нас была хоть какая-то реюзабельность.
     */

    /// <summary>
    /// Сервис, отвечающий за поиск заявок, ОД и договоров.
    /// </summary>
    public class SearchService : ISearchService
    {
        private readonly IExecutor _executor;

        public SearchService(IExecutor executor)
        {
            _executor = executor;
        }

        /// <summary>
        /// Производит поиск по заявкам и ОД изобретений.
        /// </summary>
        /// <param name="request">HTTP-запрос из <see cref="HttpRequest.Query"/> которого берутся данные о фильтрах для поиска.</param>
        /// <returns>Список найденных изобретений.</returns>
        public async Task<IPagedList<InventionSearchDto>> SearchInventions(HttpRequest request)
        {
            List<string> codesForSearch = new List<string>(InventionTypeCodes);

            if (request.Query.WillSearchUsefulModels())
            {
                codesForSearch.AddRange(UsefulModelTypeCodes);
            }

            IEnumerable<int> industrialPropertyIds = await GetIndustrialPropertyTypeIdsByCode(codesForSearch);

            IQueryable<ExpertSearchViewEntity> expertSearchResults = ExpertSearch(request, industrialPropertyIds);

            return await expertSearchResults
                .ProjectTo<InventionSearchDto>()
                .ToPagedListAsync(request.GetPaginationParams());
        }

        /// <summary>
        /// Производит экспертный поиск.
        /// </summary>
        /// <param name="request">HTTP-запрос из <see cref="HttpRequest.Query"/> которого берутся данные о фильтрах для поиска.</param>
        /// <param name="industrialPropertyTypeIds">Идентификаторы типов объектов промышленной собственности, которые должны быть включены в результаты поиска.</param>
        /// <returns>Результаты экспертного поиска.</returns>
        private IQueryable<ExpertSearchViewEntity> ExpertSearch(
            HttpRequest request,
            IEnumerable<int> industrialPropertyTypeIds)
        {
            return _executor
                .GetQuery<GetExpertSearchViewByHttpRequestQuery>()
                .Process(query => query.Execute(request))
                .Where(expertSearchResult => industrialPropertyTypeIds.Contains(expertSearchResult.ProtectionDocTypeId));
        }

        /// <summary>
        /// Получает идентификаторы объектов промышленной собственности по их коду.
        /// </summary>
        /// <param name="codes">Коды объектов промышленной собственности.</param>
        /// <returns>Идентификаторы промышленной собственности.</returns>
        private async Task<IEnumerable<int>> GetIndustrialPropertyTypeIdsByCode(IEnumerable<string> codes)
        {
            return (await _executor
                .GetQuery<GetProtectionDocTypeByCodesQuery>()
                .Process(query => query.ExecuteAsync(codes)))
                .Select(type => type.Id);
        }

      
        // Коды, которые используются для поиска.
        #region Codes

        /// <summary>
        /// Коды заявки и Патента (ОД) на изобретение (ИЗ).
        /// </summary>
        private static readonly string[] InventionTypeCodes =
        {
            DicProtectionDocTypeCodes.RequestTypeInventionCode,
            DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode
        };

        /// <summary>
        /// Коды заявки и Патента (ОД) на полезную модель (ПМ).
        /// </summary>
        private static readonly string[] UsefulModelTypeCodes =
        {
            DicProtectionDocTypeCodes.RequestTypeUsefulModelCode,
            DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode
        };

        #endregion
    }
}
