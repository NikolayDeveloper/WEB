using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCommon
{
    /// <summary>
    /// Запрос для получения "Статус заявки".
    /// </summary>
    public class GetDicRequestStatusForExpertSearchQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <returns></returns>
        public async Task<List<dynamic>> ExecuteAsync()
        {
            var routesList = new List<string>
            {
                RouteCodes.TradeMark,
                RouteCodes.IndustrialDesigns,
                RouteCodes.AppellationOfOrigin
            };

            var repository = Uow.GetRepository<DicRequestStatusRoute>();

            var query = repository.AsQueryable()
                .Include(drs => drs.DicRequestStatus)
                .Include(dr => dr.DicRoute)
                .Where(drsr => !drsr.DicRoute.IsDeleted &&
                               !drsr.DicRequestStatus.IsDeleted &&
                               routesList.Contains(drsr.DicRoute.Code))
                .Select(drsr =>
                    new
                    {
                        drsr.DicRequestStatus.Id,
                        RouteCode = drsr.DicRoute.Code,
                        drsr.DicRequestStatus.NameRu,
                        drsr.DicRequestStatus.NameEn,
                        drsr.DicRequestStatus.NameKz
                    });

            var dicRequestStatusRoute = await query.Cast<dynamic>().ToListAsync();

            var dicRequestStatusRouteGroups = dicRequestStatusRoute
                .GroupBy(dpdsr => new { dpdsr.Id, dpdsr.NameRu, dpdsr.NameEn, dpdsr.NameKz })
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
                        IsTradeMark = group.Any(g => g.RouteCode == RouteCodes.TradeMark),
                        IsIndustrialDesigns = group.Any(g => g.RouteCode == RouteCodes.IndustrialDesigns),
                        IsAppellationOfOrigin = group.Any(g => g.RouteCode == RouteCodes.AppellationOfOrigin)
                    }
                });

            return dicRequestStatusRouteGroups.Cast<dynamic>().ToList();
        }
    }
}
