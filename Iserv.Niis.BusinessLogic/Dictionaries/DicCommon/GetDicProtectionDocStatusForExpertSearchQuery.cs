using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCommon
{
    /// <summary>
    /// Запрос для получения "Статус охранного документа".
    /// </summary>
    public class GetDicProtectionDocStatusForExpertSearchQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <returns></returns>
        public async Task<List<dynamic>> ExecuteAsync()
        {
            var routesList = new List<string>
            {
                RouteCodes.ProcessingDocumentsTradeMark,
                RouteCodes.ProcessingDocumentsAppellationOfOrigin
            };

            var repository = Uow.GetRepository<DicProtectionDocStatusRoute>();

            var query = repository.AsQueryable()
                .Include(drs => drs.DicProtectionDocStatus)
                .Include(dr => dr.DicRoute)
                .Where(dpdsr => !dpdsr.DicRoute.IsDeleted &&
                                !dpdsr.DicProtectionDocStatus.IsDeleted &&
                                routesList.Contains(dpdsr.DicRoute.Code))
                .Select(dpdsr =>
                    new
                    {
                        dpdsr.DicProtectionDocStatus.Id,
                        RouteCode = dpdsr.DicRoute.Code,
                        dpdsr.DicProtectionDocStatus.NameRu,
                        dpdsr.DicProtectionDocStatus.NameEn,
                        dpdsr.DicProtectionDocStatus.NameKz
                    });

            var dicProtectionDocStatusRoute = await query.Cast<dynamic>().ToListAsync();

            var dicProtectionDocStatusGroups = dicProtectionDocStatusRoute
                .GroupBy(dpdsr => new {dpdsr.Id, dpdsr.NameRu, dpdsr.NameEn, dpdsr.NameKz})
                .Select(group => new
                {
                    group.Key.Id,
                    Name = new
                    {
                        Ru = group.Key.NameRu,
                        En = group.Key.NameEn,
                        Kz = group.Key.NameKz
                    },
                    RouteCodeFiltering = new
                    {
                        IsTradeMark = group.Any(g => g.RouteCode == RouteCodes.ProcessingDocumentsTradeMark),
                        IsAppellationOfOrigin = group.Any(g => g.RouteCode == RouteCodes.ProcessingDocumentsAppellationOfOrigin)
                    }
                });

            return dicProtectionDocStatusGroups.Cast<dynamic>().ToList();
        }
    }
}
