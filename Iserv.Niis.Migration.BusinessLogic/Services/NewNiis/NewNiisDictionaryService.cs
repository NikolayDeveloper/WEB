using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisDictionaryService : BaseService
    {
        private readonly NiisWebContextMigration _context;
        private readonly IDicTypeResolver _dicTypeResolver;

        public NewNiisDictionaryService(
            NiisWebContextMigration context,
            IDicTypeResolver dicTypeResolver)
        {
            _context = context;
            _dicTypeResolver = dicTypeResolver;
        }

        #region Common methods

        public bool IsAnyDictionaryByEntityName(string entityName)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _context.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            return dictionaries.Any();
        }

        public void CreateRangeDictionaries(IEnumerable<object> dictionaries)
        {
            _context.AddRange(dictionaries);
            _context.SaveChanges();
        }

        public int GetDictionaryIdByCode(string entityName, string code)
        {
            var dicType = _dicTypeResolver.Resolve(entityName);
            var dictionaries = _context.Set(dicType) as IQueryable<IDictionaryEntity<int>>;
            return dictionaries.Single(d => d.Code == code).Id;
        }

        #endregion

        public List<DicProtectionDocType> GetDicProtectionDocTypes()
        {
            return _context.DicProtectionDocTypes
                .AsNoTracking()
                .ToList();
        }

        public List<DicRoute> GetDicRoutes()
        {
            return _context.DicRoutes
                .AsNoTracking()
                .ToList();
        }

        public int GetDicCustomersCount()
        {
            return _context.DicCustomers
                .OrderBy(c => c.Id)
                .Where(c => c.Id != 0)
                .Count();
        }

        public int? GetLastDicCustomerId()
        {
            return _context.DicCustomers
                .AsNoTracking()
                 .OrderByDescending(d => d.Id)
                 .FirstOrDefault()?.Id;
        }

        public void AddRangeIntegrationConServiceStatus(List<IntegrationConServiceStatus> statuses)
        {
            if (_context.IntegrationConServiceStatuses.Count() > 0)
            {
                _context.IntegrationConServiceStatuses.AddRange(statuses);
                _context.SaveChanges();
            }
        }

        public void CreateRangeCustomerAttorneyInfos(List<CustomerAttorneyInfo> infos)
        {
            _context.CustomerAttorneyInfos.AddRange(infos);
            _context.SaveChanges();
        }

        public bool IsAnyDocumentTemplateFiles()
        {
            return _context.DocumentTemplateFiles
                .AsNoTracking()
                .Any();
        }

        public void CreateRangeProtectionDocAttornies(List<ProtectionDocAttorney> protectionDocAttornies)
        {
            _context.ProtectionDocAttorneys.AddRange(protectionDocAttornies);
            _context.SaveChanges();
        }

        public bool IsAnyProtectionDocAttornies()
        {
            return _context.ProtectionDocAttorneys
                .AsNoTracking()
                .Any();
        }

        public List<DicPaymentStatus> GetDicPaymentStatuses()
        {
            return _context.DicPaymentStatuses
                .AsNoTracking()
                .ToList();
        }

        public List<DicDocumentStatus> GetDicDocumentStatus()
        {
            return _context.DicDocumentStatuses
                .AsNoTracking()
                .ToList();
        }

        public bool IsAnyRouteStateOrders()
        {
            return _context.RouteStageOrders
                .AsNoTracking()
                .Any();
        }

        public void CreateRangeRouteStageOrders(List<RouteStageOrder> routeStageOrders)
        {
            _context.RouteStageOrders.AddRange(routeStageOrders);
            _context.SaveChanges();
        }

        public bool IsAnyAvailabilityCorrespondences()
        {
            return _context.AvailabilityCorrespondences
                .AsNoTracking()
                .Any();
        }

        public void CreateRangeAvailabilityCorrespondences(List<AvailabilityCorrespondence> availabilityCorrespondences)
        {
            _context.AvailabilityCorrespondences.AddRange(availabilityCorrespondences);
            _context.SaveChanges();
        }

        public void UpdateRangeDicRouteStages(List<DicRouteStage> newDicRouteStages)
        {
            var dicRouteStages = _context.DicRouteStages.ToList();
            dicRouteStages.ForEach(dicRouteStage =>
            {
                var newDicRouteStage = newDicRouteStages.FirstOrDefault(d => d.Id == dicRouteStage.Id);
                if (newDicRouteStage != null)
                {
                    dicRouteStage.DateUpdate = DateTimeOffset.Now;
                    dicRouteStage.Code = newDicRouteStage.Code;
                    dicRouteStage.NameRu = newDicRouteStage.NameRu;
                    dicRouteStage.NameKz = newDicRouteStage.NameKz;
                    dicRouteStage.NameEn = newDicRouteStage.NameEn;
                    dicRouteStage.RouteId = newDicRouteStage.RouteId;
                    dicRouteStage.ExpirationType = newDicRouteStage.ExpirationType;
                    dicRouteStage.ExpirationValue = newDicRouteStage.ExpirationValue;
                }

            });
            _context.DicRouteStages.UpdateRange(dicRouteStages);
            _context.SaveChanges();
        }
    }
}
