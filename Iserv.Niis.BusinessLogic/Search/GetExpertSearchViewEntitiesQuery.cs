using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using LinqKit;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.BusinessLogic.Search.DTOs;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Infrastructure.Extensions.Filter;


namespace Iserv.Niis.BusinessLogic.Search
{
    /// <summary>
    /// Запрос для получения результатов экспертного поиска.
    /// </summary>
    public class GetExpertSearchViewByHttpRequestQuery : BaseQuery
    {
        /// <summary>
        /// Хранит информацию о том, как производить поиск по определенным свойствам. Ключом словаря является название свойства, по которому ведется поиск, а значением - тип поиска (like, equal).
        /// </summary>
        public Dictionary<string, string> Conditions { get; set; }

        /// <summary>
        /// Выполнение запроса.
        /// <para></para>
        /// Логика получения фильтров запроса хранится здесь. Из объекта класса <see cref="HttpRequest"/> через свойство <see cref="HttpRequest.Query"/> достается информация о том, какие фильтры включены.
        /// </summary>
        /// <param name="request">Http запрос, который хранит информацию о экспертном поиске.</param>
        /// <returns>Результат экспертного поиска.</returns>
        public IQueryable<ExpertSearchViewEntity> Execute(HttpRequest request)
        {
            var query = GetQuery();
            var conditionDto = CreateExpertSearchFilterConditionDto(request.Query);

            query = Filter(query, conditionDto);

            return query;
        }


        private IQueryable<ExpertSearchViewEntity> GetQuery()
        {
            var repository = Uow.GetRepository<ExpertSearchViewEntity>();

            return repository.AsQueryable()
                .Include(view => view.Request)
                .ThenInclude(request => request.RequestCustomers)
                .ThenInclude(requestCustomer => requestCustomer.Customer)
                .Include(view => view.Request)
                .ThenInclude(request => request.RequestCustomers)
                .ThenInclude(requestCustomer => requestCustomer.CustomerRole)
                .Include(view => view.Request)
                .ThenInclude(request => request.ICGSRequests)
                .ThenInclude(icgsRequest => icgsRequest.Icgs)
                .Include(view => view.Request).ThenInclude(d => d.EarlyRegs).ThenInclude(d => d.RegCountry)
                .Include(view => view.Request)
                .ThenInclude(request => request.Icfems)
                .ThenInclude(icfemsRequest => icfemsRequest.DicIcfem)
                .Include(view => view.ProtectionDoc)
                .ThenInclude(protectionDoc => protectionDoc.Request)
                .ThenInclude(request => request.RequestCustomers)
                .ThenInclude(rc => rc.Customer)
                .Include(view => view.ProtectionDoc)
                .ThenInclude(protectionDoc => protectionDoc.Request)
                .ThenInclude(request => request.RequestCustomers)
                .ThenInclude(rc => rc.CustomerRole)
                .Include(view => view.ProtectionDoc).ThenInclude(d => d.EarlyRegs).ThenInclude(d => d.RegCountry)
                .Include(view => view.ProtectionDoc)
                .ThenInclude(protectionDoc => protectionDoc.Request)
                .ThenInclude(request => request.ICGSRequests)
                .ThenInclude(ir => ir.Icgs)
                .Include(view => view.ProtectionDoc)
                .ThenInclude(protectionDoc => protectionDoc.Request)
                .ThenInclude(request => request.Icfems)
                .ThenInclude(icfemsRequest => icfemsRequest.DicIcfem)
                .Include(view => view.Request)
                .ThenInclude(request => request.IPCRequests)
                .ThenInclude(ipc => ipc.Ipc)
                .AsQueryable();
        }

        /// <summary>
        /// Возвращает объект, который хранит информацию о фильтрах экспертного поиска и инициализирует свойство <see cref="Conditions"/>.
        /// </summary>
        /// <param name="httpQuery">Http запрос, из которого достается информация о фильтрах.</param>
        /// <returns>Объект хранящий информацию о фильтрах экспертного поиска.</returns>
        private ExpertSearchFilterConditionDto CreateExpertSearchFilterConditionDto(IQueryCollection httpQuery)
        {
            Conditions = new Dictionary<string, string>();
            var paginationQueryKeys = new[] { "_page", "_limit" };

            var keyValuePairs = httpQuery
                .Where(q => Operators.SupportedOperators.Any(so => q.Key.Contains(so))
                            && !paginationQueryKeys.Any(p => q.Key.Contains(p)))
                .ToList();

            var expertSearchFilterConditionDto = new ExpertSearchFilterConditionDto();

            foreach (var keyValuePair in keyValuePairs)
            {
                var splitedKey = keyValuePair.Key.Split('_');

                var propertyName = splitedKey[1];
                var value = keyValuePair.Value.First();

                //Используется для задачи диапазона значений
                if (splitedKey.Length == 3 && splitedKey[2].Contains("crange"))
                {
                    var splitedValue = keyValuePair.Value.First().Split(',');

                    if (splitedValue.Length == 2)
                    {
                        if (propertyName.Equals("PriorityRegDate", StringComparison.CurrentCultureIgnoreCase)
                            && string.IsNullOrEmpty(splitedValue[1]))
                        {

                            splitedValue[1] = splitedValue[0];
                            splitedValue[0] = null;
                        }
                        var propertyFrom = expertSearchFilterConditionDto.GetType().GetProperty(propertyName + "From",
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyFrom != null)
                        {
                            SetPropertyValue(splitedValue[0], propertyFrom, expertSearchFilterConditionDto);
                        }

                        var propertyTo = expertSearchFilterConditionDto.GetType().GetProperty(propertyName + "To",
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyTo != null)
                        {
                            SetPropertyValue(splitedValue[1], propertyTo, expertSearchFilterConditionDto);
                        }
                    }
                }
                else if (splitedKey.Length == 3 && splitedKey[2] == "contains")
                {
                    var property = expertSearchFilterConditionDto.GetType().GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    SetPropertyValue(keyValuePair.Value.First(), property, expertSearchFilterConditionDto);
                }
                else if (splitedKey.Length == 3 && splitedKey[0] == "cand")
                {
                    var property = expertSearchFilterConditionDto.GetType().GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        var splitedValue = keyValuePair.Value
                            .First()
                            .Split(',')
                            .Where(r => r != "0")
                            .Select(int.Parse)
                            .ToList();
                        if (splitedValue.Count > 0)
                        {
                            property.SetValue(expertSearchFilterConditionDto, splitedValue);
                        }
                    }
                }
                else
                {
                    var property = expertSearchFilterConditionDto.GetType().GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        SetPropertyValue(value, property, expertSearchFilterConditionDto);
                        if (splitedKey.Length == 3)
                        {
                            Conditions.Add(propertyName, splitedKey[2]);
                        }
                    }
                }
            }

            return expertSearchFilterConditionDto;
        }

        private IQueryable<ExpertSearchViewEntity> Filter(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            Func<IQueryable<ExpertSearchViewEntity>, ExpertSearchFilterConditionDto, IQueryable<ExpertSearchViewEntity>>[] filters =
            {
                FilterByRequest,
                FilterByFame,
                FilterByName,
                FilterByIcgsDescription,
                FilterByRequestNumber,
                FilterByPatentAttorney,
                FilterByOwnerCity,
                FilterByOwnerCountry,
                FilterByOwnerName,
                FilterByOwnerRegion,
                FilterByDeclarantCity,
                FilterByDeclarantCountry,
                FilterByDeclarantName,
                FilterByDeclarantRegion,
                FilterByGosNumber,
                FilterByRequestDate,
                FilterByPublishDate,
                FilterByGosDate,
                FilterByIcfem,
                FilterByIcgs,
                FilterByIcis,
                FilterByStatus,
                FilterByTrademarkKind,
                FilterByTrademarkType,
                FilterByIpc,
                FilterByIpcDescriptions,
                FilterByAuthor,
                FilterByRegisterDate
            };

            foreach (var filter in filters)
            {
                query = filter(query, conditionDto);
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByRegisterDate(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {

            if (conditionDto.RegisterDateFrom.HasValue)
            {
                query = query.Where(view => view.ProtectionDoc.RegDate != null &&
                                            view.ProtectionDoc.RegDate.Value.Date >= conditionDto.RegisterDateFrom);
            }

            if (conditionDto.RegisterDateTo.HasValue)
            {
                query = query.Where(view => view.ProtectionDoc.RegDate != null &&
                                            view.ProtectionDoc.RegDate.Value.Date <= conditionDto.RegisterDateTo);
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByIpc(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.IpcCodes != null && conditionDto.IpcCodes.Any())
            {
                query = query.Where(view =>
                    view.Request.IPCRequests.Any(ipc => conditionDto.IpcCodes.Any(code => ipc.Ipc.Code.Contains(code))));
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByIpcDescriptions(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.IpcDescriptions != null && conditionDto.IpcDescriptions.Any())
            {
                query = query.Where(view =>
                    view.Request.IPCRequests.Any(ipc => conditionDto.IpcDescriptions.Any(description => ipc.Ipc.Description.Contains(description))));
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByAuthor(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (!string.IsNullOrWhiteSpace(conditionDto.Author))
            {
                query = query.Where(q => q.Request.RequestCustomers
                                             .Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Author &&
                                                        rc.Customer.NameRu.Contains(conditionDto.Author)) ||
                                         q.ProtectionDoc.ProtectionDocCustomers
                                             .Any(pdc => pdc.CustomerRole.Code == DicCustomerRoleCodes.Author &&
                                                         pdc.Customer.NameRu.Contains(conditionDto.Author)));
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByTrademarkKind(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.TrademarkKindId.HasValue)
            {
                query = query.Where(q =>
                    q.Request.SpeciesTradeMarkId == conditionDto.TrademarkKindId ||
                    q.ProtectionDoc.Request.SpeciesTradeMarkId == conditionDto.TrademarkKindId);
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByTrademarkType(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.TrademarkTypeId.HasValue)
            {
                query = query.Where(q =>
                    q.ProtectionDoc.Request.TypeTrademarkId == conditionDto.TrademarkTypeId ||
                    q.Request.TypeTrademarkId == conditionDto.TrademarkTypeId);
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByStatus(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.RequestStatusIds?.Count > 0 && conditionDto.ProtectionDocStatusIds?.Count > 0)
            {
                query = query.Where(q => conditionDto.RequestStatusIds.Contains(q.Request.StatusId ?? 0) || conditionDto.ProtectionDocStatusIds.Contains(q.ProtectionDoc.StatusId ?? 0));
            }
            else if (conditionDto.RequestStatusIds?.Count > 0)
            {
                query = query.Where(q => conditionDto.RequestStatusIds.Contains(q.Request.StatusId ?? 0));
            }
            else if (conditionDto.ProtectionDocStatusIds?.Count > 0)
            {
                query = query.Where(q => conditionDto.ProtectionDocStatusIds.Contains(q.ProtectionDoc.StatusId ?? 0));
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByIcis(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.Icis?.Count > 0)
            {
                query = query.Where(q =>
                    q.Request.ICISRequests.Any(i => conditionDto.Icis.Contains(i.IcisId)) ||
                    q.ProtectionDoc.Request.ICISRequests.Any(i => conditionDto.Icis.Contains(i.IcisId)));
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByIcgs(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.IcgsIds?.Count > 0)
            {
                query = query.Where(q =>
                    (conditionDto.IcgsIds.Count == q.ProtectionDoc.IcgsProtectionDocs.Count
                        && q.ProtectionDoc.IcgsProtectionDocs.All(i => conditionDto.IcgsIds.Contains(i.IcgsId)))
                    || 
                    (conditionDto.IcgsIds.Count == q.ProtectionDoc.Request.ICGSRequests.Count
                        && q.ProtectionDoc.Request.ICGSRequests.All(i => conditionDto.IcgsIds.Contains(i.IcgsId)))
                    ||
                    (conditionDto.IcgsIds.Count == q.Request.ICGSRequests.Count
                        && q.Request.ICGSRequests.All(i => conditionDto.IcgsIds.Contains(i.IcgsId)))
                );
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByIcfem(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.IcfemIds?.Count > 0)
            {
                query = query.Where(q =>
                    (conditionDto.IcfemIds.Count == q.ProtectionDoc.Icfems.Count
                        && q.ProtectionDoc.Icfems.All(i => conditionDto.IcfemIds.Contains(i.DicIcfemId)))
                    ||
                    (conditionDto.IcfemIds.Count == q.ProtectionDoc.Request.Icfems.Count
                        && q.ProtectionDoc.Request.Icfems.All(i => conditionDto.IcfemIds.Contains(i.DicIcfemId)))
                    ||
                    (conditionDto.IcfemIds.Count == q.Request.Icfems.Count
                        && q.Request.Icfems.All(i => conditionDto.IcfemIds.Contains(i.DicIcfemId)))
                );
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByGosDate(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.GosDateFrom.HasValue)
            {
                query = query.Where(q => q.ProtectionDoc.GosDate.Value.Date >= conditionDto.GosDateFrom.Value.Date);
            }
            if (conditionDto.GosDateTo.HasValue)
            {
                query = query.Where(q => q.ProtectionDoc.GosDate.Value.Date <= conditionDto.GosDateTo.Value.Date);
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByPublishDate(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {

            if (conditionDto.PublishDateFrom.HasValue)
            {
                query = query.Where(q => q.ProtectionDoc.Bulletins.Any(pdb =>
                                             pdb.Bulletin.PublishDate.Value.Date >= conditionDto.PublishDateFrom.Value.Date) ||
                                         q.Request.PublishDate.Value.Date >= conditionDto.PublishDateFrom.Value.Date);
            }
            if (conditionDto.PublishDateTo.HasValue)
            {
                query = query.Where(q => q.ProtectionDoc.Bulletins.Any(pdb =>
                                             pdb.Bulletin.PublishDate.Value.Date <= conditionDto.PublishDateTo.Value.Date) ||
                                         q.Request.PublishDate.Value.Date <= conditionDto.PublishDateTo.Value.Date);
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByRequestDate(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.RequestDateFrom.HasValue)
            {
                query = query.Where(q =>
                    q.ProtectionDoc.RegDate.Value.Date >= conditionDto.RequestDateFrom.Value.Date ||
                    q.Request.RequestDate.Value.Date >= conditionDto.RequestDateFrom.Value.Date);
            }
            if (conditionDto.RequestDateTo.HasValue)
            {
                query = query.Where(q =>
                    q.ProtectionDoc.RegDate.Value.Date <= conditionDto.RequestDateTo.Value.Date ||
                    q.Request.RequestDate.Value.Date <= conditionDto.RequestDateTo.Value.Date);
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByGosNumber(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (!string.IsNullOrEmpty(conditionDto.GosNumber))
            {
                query = IsWordContains("GosNumber") 
                    ? query.Where(r => r.ProtectionDoc.GosNumber.Contains(conditionDto.GosNumber)) 
                    : query.Where(r => r.ProtectionDoc.GosNumber == conditionDto.GosNumber);
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByDeclarantName(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (!string.IsNullOrEmpty(conditionDto.DeclarantName))
            {
                query = query.Where(q => q.Request.RequestCustomers
                                             .Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                                                        rc.Customer.NameRu.Contains(conditionDto.DeclarantName)) ||
                                         q.ProtectionDoc.ProtectionDocCustomers
                                             .Any(pdc => pdc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                                                         pdc.Customer.NameRu.Contains(conditionDto.DeclarantName)));
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByDeclarantCity(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (!string.IsNullOrEmpty(conditionDto.DeclarantCity))
            {
                query = query.Where(q => q.Request.RequestCustomers
                                             .Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                                                        rc.Customer.City.Contains(conditionDto.DeclarantCity)) ||
                                         q.ProtectionDoc.ProtectionDocCustomers
                                             .Any(pdc => pdc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                                                         pdc.Customer.City.Contains(conditionDto.DeclarantCity)));
                //if (IsWordContains("DeclarantCity"))
                //{
                //    query = query.Where(q => q.Request.RequestCustomers.Any(rc =>
                //                                 rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                //                                 rc.Customer.City.Contains(conditionDto.DeclarantCity)) ||
                //                             q.ProtectionDoc.ProtectionDocCustomers.Any(pdc =>
                //                                 pdc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                //                                 pdc.Customer.City.Contains(conditionDto.DeclarantCity)));
                //}
                //else
                //{
                //    query = query.Where(q => q.Request.RequestCustomers.Any(rc =>
                //                                 rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                //                                 rc.Customer.City == conditionDto.DeclarantCity) ||
                //                             q.ProtectionDoc.ProtectionDocCustomers.Any(pdc =>
                //                                 pdc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                //                                 pdc.Customer.City == conditionDto.DeclarantCity));
                //}
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByDeclarantRegion(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {

            if (!string.IsNullOrEmpty(conditionDto.DeclarantOblast))
            {
                query = query.Where(q => q.Request.RequestCustomers
                                             .Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                                                        rc.Customer.Oblast.Contains(conditionDto.DeclarantOblast)) ||
                                         q.ProtectionDoc.ProtectionDocCustomers
                                             .Any(pdc => pdc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                                                         pdc.Customer.Oblast.Contains(conditionDto.DeclarantOblast)));
                //if (IsWordContains("DeclarantOblast"))
                //{
                //    query = query.Where(q => q.Request.RequestCustomers.Any(rc =>
                //                                 rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                //                                 rc.Customer.Oblast.Contains(conditionDto.DeclarantOblast)) ||
                //                             q.ProtectionDoc.ProtectionDocCustomers.Any(pdc =>
                //                                 pdc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                //                                 pdc.Customer.Oblast.Contains(conditionDto.DeclarantOblast)));
                //}
                //else
                //{
                //    query = query.Where(q => q.Request.RequestCustomers.Any(rc =>
                //                                 rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                //                                 rc.Customer.Oblast == conditionDto.DeclarantOblast) ||
                //                             q.ProtectionDoc.ProtectionDocCustomers.Any(pdc =>
                //                                 pdc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                //                                 pdc.Customer.Oblast == conditionDto.DeclarantOblast));
                //}
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByDeclarantCountry(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {

            if (conditionDto.DeclarantCountryId.HasValue)
            {
                query = query.Where(q => q.Request.RequestCustomers
                                             .Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                                                        rc.Customer.CountryId == conditionDto.DeclarantCountryId) ||
                                         q.ProtectionDoc.ProtectionDocCustomers
                                             .Any(pdc => pdc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                                                         pdc.Customer.CountryId == conditionDto.DeclarantCountryId));
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByOwnerName(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (!string.IsNullOrEmpty(conditionDto.OwnerName))
            {
                query = query.Where(q => q.Request.RequestCustomers
                                             .Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                                                        rc.Customer.NameRu.Contains(conditionDto.OwnerName)) ||
                                         q.ProtectionDoc.ProtectionDocCustomers
                                             .Any(pdc => pdc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                                                         pdc.Customer.NameRu.Contains(conditionDto.OwnerName)));
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByOwnerCity(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (!string.IsNullOrEmpty(conditionDto.OwnerCity))
            {
                query = query.Where(q => q.Request.RequestCustomers
                                             .Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                                                        rc.Customer.City.Contains(conditionDto.OwnerCity)) ||
                                         q.ProtectionDoc.ProtectionDocCustomers
                                             .Any(pdc => pdc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                                                         pdc.Customer.City.Contains(conditionDto.OwnerCity)));
                //if (IsWordContains("OwnerCity"))
                //{
                //    query = query.Where(q => q.ProtectionDoc.ProtectionDocCustomers.Any(pdc =>
                //        pdc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                //        pdc.Customer.City.Contains(conditionDto.OwnerCity)));
                //}
                //else
                //{
                //    query = query.Where(q => q.ProtectionDoc.ProtectionDocCustomers.Any(pdc =>
                //        pdc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                //        pdc.Customer.City == conditionDto.OwnerCity));
                //}
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByOwnerRegion(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {

            if (!string.IsNullOrEmpty(conditionDto.OwnerOblast))
            {
                query = query.Where(q => q.Request.RequestCustomers
                                             .Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                                                        rc.Customer.Oblast.Contains(conditionDto.OwnerOblast)) ||
                                         q.ProtectionDoc.ProtectionDocCustomers
                                             .Any(pdc => pdc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                                                         pdc.Customer.Oblast.Contains(conditionDto.OwnerOblast)));
                //if (IsWordContains("OwnerOblast"))
                //{
                //    query = query.Where(q => q.ProtectionDoc.ProtectionDocCustomers.Any(pdc =>
                //        pdc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                //        pdc.Customer.Oblast.Contains(conditionDto.OwnerOblast)));
                //}
                //else
                //{
                //    query = query.Where(q => q.ProtectionDoc.ProtectionDocCustomers.Any(pdc =>
                //        pdc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                //        pdc.Customer.Oblast == conditionDto.OwnerOblast));
                //}
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByOwnerCountry(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.OwnerCountryId.HasValue)
            {
                query = query.Where(q => q.Request.RequestCustomers
                                             .Any(rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                                                        rc.Customer.CountryId == conditionDto.OwnerCountryId) ||
                                         q.ProtectionDoc.ProtectionDocCustomers
                                             .Any(pdc => pdc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                                                         pdc.Customer.CountryId == conditionDto.OwnerCountryId));
                //query = query.Where(q => q.ProtectionDoc.ProtectionDocCustomers
                //    .Any(pdc => pdc.CustomerRole.Code == DicCustomerRoleCodes.Owner &&
                //                pdc.Customer.CountryId == conditionDto.OwnerCountryId));
            }
            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByPatentAttorney(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (!string.IsNullOrEmpty(conditionDto.PatentAttorneyName))
            {
                if (IsWordContains("PatentAttorneyName"))
                {
                    query = query.Where(q => q.Request.RequestCustomers
                                                 .Any(r => r.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney &&
                                                           (r.Customer.NameRu.Contains(conditionDto.PatentAttorneyName) ||
                                                            r.Customer.NameEn.Contains(conditionDto.PatentAttorneyName) ||
                                                            r.Customer.NameKz.Contains(conditionDto.PatentAttorneyName))) ||
                                             q.ProtectionDoc.Request.RequestCustomers
                                                 .Any(r => r.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney &&
                                                           (r.Customer.NameRu.Contains(conditionDto.PatentAttorneyName) ||
                                                            r.Customer.NameEn.Contains(conditionDto.PatentAttorneyName) ||
                                                            r.Customer.NameKz.Contains(conditionDto.PatentAttorneyName))));
                }
                else
                {
                    query = query.Where(q => q.Request.RequestCustomers
                                                 .Any(r => r.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney &&
                                                           (r.Customer.NameRu == conditionDto.PatentAttorneyName ||
                                                            r.Customer.NameEn == conditionDto.PatentAttorneyName ||
                                                            r.Customer.NameKz == conditionDto.PatentAttorneyName)) ||
                                             q.ProtectionDoc.Request.RequestCustomers
                                                 .Any(r => r.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney &&
                                                           (r.Customer.NameRu == conditionDto.PatentAttorneyName ||
                                                            r.Customer.NameEn == conditionDto.PatentAttorneyName ||
                                                            r.Customer.NameKz == conditionDto.PatentAttorneyName)));
                }
            }

            if (!string.IsNullOrEmpty(conditionDto.PatentAttorneyNumber))
            {
                if (IsWordContains("PatentAttorneyNumber"))
                {
                    query = query.Where(q => q.Request.RequestCustomers.Any(r =>
                            r.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney &&
                            r.Customer.PowerAttorneyFullNum.Contains(conditionDto.PatentAttorneyNumber)) ||
                        q.ProtectionDoc.Request.RequestCustomers.Any(r =>
                            r.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney &&
                            r.Customer.PowerAttorneyFullNum.Contains(conditionDto.PatentAttorneyNumber)));
                }
                else
                {
                    query = query.Where(q => q.Request.RequestCustomers.Any(r =>
                            r.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney &&
                            r.Customer.PowerAttorneyFullNum == conditionDto.PatentAttorneyNumber) ||
                        q.ProtectionDoc.Request.RequestCustomers.Any(r =>
                            r.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney &&
                            r.Customer.PowerAttorneyFullNum == conditionDto.PatentAttorneyNumber));
                }
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByRequest(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            return query.Where(view => view.Request == null || view.Request != null);
        }

        private IQueryable<ExpertSearchViewEntity> FilterByFame(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.IsWellKnown != null && conditionDto.IsWellKnown != false) return query;

            var subTypeRepo = Uow.GetRepository<DicProtectionDocSubType>();
            var wellKnownTrademarkType = subTypeRepo.AsQueryable().First(s => s.Code == DicProtectionDocSubtypeCodes.WellKnownTradeMark);
            query = query.Where(view => !(view.Request.SpeciesTradeMarkId == wellKnownTrademarkType.Id || view.ProtectionDoc.SpeciesTradeMarkId == wellKnownTrademarkType.Id));

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByName(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (string.IsNullOrEmpty(conditionDto.Name)) return query;

            if (IsWordContains("Name"))
            {
                if (!conditionDto.Name.Contains("%") && !conditionDto.Name.Contains("%"))
                {
                    query = query.Where(r =>
                        //Request ############################
                        r.Request.NameRu.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.Request.NameEn.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.Request.NameKz.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.Request.Transliteration.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.Request.Transliteration == conditionDto.Name.ToLower() ||
                        r.Request.Translation.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||

                        r.Request.NameRu.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.Request.NameEn.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.Request.NameKz.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.Request.Transliteration.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.Request.Translation.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||

                        r.Request.NameRu.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.Request.NameEn.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.Request.NameKz.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.Request.Transliteration.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.Request.Translation.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||

                        //ProtectionDocRequest ############################
                        r.ProtectionDoc.Request.NameRu.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Request.NameEn.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Request.NameKz.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Request.Transliteration.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Request.Transliteration == conditionDto.Name.ToLower() ||
                        r.ProtectionDoc.Request.Translation.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||

                        r.ProtectionDoc.Request.NameRu.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Request.NameEn.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Request.NameKz.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Request.Transliteration.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Request.Translation.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||

                        r.ProtectionDoc.Request.NameRu.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.ProtectionDoc.Request.NameEn.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.ProtectionDoc.Request.NameKz.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.ProtectionDoc.Request.Transliteration.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.ProtectionDoc.Request.Translation.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||

                        //ProtectionDoc ############################
                        r.ProtectionDoc.NameRu.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.NameEn.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.NameKz.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Transliteration.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Transliteration.ToLower() == conditionDto.Name.ToLower() ||
                        r.ProtectionDoc.Translation.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||

                        r.ProtectionDoc.NameRu.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.NameEn.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.NameKz.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Transliteration.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                        r.ProtectionDoc.Translation.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||

                        r.ProtectionDoc.NameRu.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.ProtectionDoc.NameEn.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.ProtectionDoc.NameKz.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.ProtectionDoc.Transliteration.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                        r.ProtectionDoc.Translation.ToLower().EndsWith($" {conditionDto.Name.ToLower()}")
                        );
                }
                else
                {
                    query = query.Where(r =>
                        EF.Functions.Like(r.Request.NameRu, 
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.Request.NameEn,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.Request.NameKz,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.Request.Transliteration,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.Request.Translation,
                            $"{conditionDto.Name.Replace('*', '_')}") ||

                        EF.Functions.Like(r.ProtectionDoc.Request.NameRu,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.ProtectionDoc.Request.NameEn,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.ProtectionDoc.Request.NameKz,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.ProtectionDoc.Request.Transliteration,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.ProtectionDoc.Request.Translation,
                            $"{conditionDto.Name.Replace('*', '_')}") ||

                        EF.Functions.Like(r.ProtectionDoc.NameRu,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.ProtectionDoc.NameEn,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.ProtectionDoc.NameKz,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.ProtectionDoc.Transliteration,
                            $"{conditionDto.Name.Replace('*', '_')}") ||
                        EF.Functions.Like(r.ProtectionDoc.Translation,
                            $"{conditionDto.Name.Replace('*', '_')}"));
                }
            }
            else
            {
                query = query.Where(r =>
                    r.Request.NameRu == conditionDto.Name ||
                    r.Request.NameEn == conditionDto.Name ||
                    r.Request.NameKz == conditionDto.Name ||
                    r.Request.Transliteration.ToLower() == conditionDto.Name.ToLower() ||
                    r.Request.Transliteration.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                    r.Request.Transliteration.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                    r.Request.Transliteration.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                    r.Request.Translation == conditionDto.Name ||

                    r.ProtectionDoc.Request.NameRu == conditionDto.Name ||
                    r.ProtectionDoc.Request.NameEn == conditionDto.Name ||
                    r.ProtectionDoc.Request.NameKz == conditionDto.Name ||
                    r.ProtectionDoc.Request.Transliteration.ToLower() == conditionDto.Name.ToLower() ||
                    r.ProtectionDoc.Request.Transliteration.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                    r.ProtectionDoc.Request.Transliteration.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                    r.ProtectionDoc.Request.Transliteration.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                    r.ProtectionDoc.Request.Translation == conditionDto.Name ||

                    r.ProtectionDoc.NameRu == conditionDto.Name ||
                    r.ProtectionDoc.NameEn == conditionDto.Name ||
                    r.ProtectionDoc.NameKz == conditionDto.Name ||
                    r.ProtectionDoc.Transliteration.ToLower() == conditionDto.Name.ToLower() ||
                    r.ProtectionDoc.Transliteration.ToLower().Contains($" {conditionDto.Name.ToLower()} ") ||
                    r.ProtectionDoc.Transliteration.ToLower().StartsWith($"{conditionDto.Name.ToLower()} ") ||
                    r.ProtectionDoc.Transliteration.ToLower().EndsWith($" {conditionDto.Name.ToLower()}") ||
                    r.ProtectionDoc.Translation == conditionDto.Name
                );
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByIcgsDescription(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (conditionDto.IcgsDescriptions?.Count > 0)
            {
                var predicate = PredicateBuilder.New<ExpertSearchViewEntity>(false);

                if (IsWordContains("IcgsDescriptions"))
                {
                    foreach (var description in conditionDto.IcgsDescriptions)
                    {
                        predicate.Or(r =>
                            r.Request.ICGSRequests.Any(icgsRequest => icgsRequest.Description.Contains(description) || icgsRequest.DescriptionKz.Contains(description)) ||
                            r.ProtectionDoc.Request.ICGSRequests.Any(icgsRequest => icgsRequest.Description.Contains(description) || icgsRequest.DescriptionKz.Contains(description)));
                    }
                }
                else
                {
                    foreach (var description in conditionDto.IcgsDescriptions)
                    {
                        predicate.Or(r =>
                            r.Request.ICGSRequests.Any(icgsr => icgsr.Description == description || icgsr.DescriptionKz == description) ||
                            r.ProtectionDoc.Request.ICGSRequests.Any(icgsr => icgsr.Description == description || icgsr.DescriptionKz == description));
                    }
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<ExpertSearchViewEntity> FilterByRequestNumber(IQueryable<ExpertSearchViewEntity> query,
            ExpertSearchFilterConditionDto conditionDto)
        {
            if (!string.IsNullOrEmpty(conditionDto.RequestNumber))
            {
                if (IsWordContains("RequestNumber"))
                {
                    query = query.Where(r =>
                        r.Request.RequestNum.Contains(conditionDto.RequestNumber) ||
                        r.ProtectionDoc.RegNumber.Contains(conditionDto.RequestNumber));
                }
                else
                {
                    query = query.Where(r =>
                        r.Request.RequestNum == conditionDto.RequestNumber ||
                        r.ProtectionDoc.RegNumber == conditionDto.RequestNumber);
                }
            }

            return query;
        }

        /// <summary>
        /// Задает значение свойству объекта по названию свойства.
        /// <para></para>
        /// Несмотря на то, что параметр <paramref name="value"/> имеет строковый тип, в методе проводятся проверки типа свойства и параметр конвертируется в нужный тип.
        /// </summary>
        /// <param name="value">Строковое представление значения.</param>
        /// <param name="property">Свойство, в которое заносится новое значение.</param>
        /// <param name="expertSearchFilterConditionDto">Объект, в свойство которого заносится новое значение.</param>
        private void SetPropertyValue(string value, PropertyInfo property, ExpertSearchFilterConditionDto expertSearchFilterConditionDto)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            object convertedValue = null;

            if (property.PropertyType == typeof(string))
            {
                convertedValue = value;
            }

            else if (property.PropertyType == typeof(DateTimeOffset) || property.PropertyType == typeof(DateTimeOffset?))
            {
                DateTimeOffset.TryParse(value, out DateTimeOffset date);
                convertedValue = date;
            }
            else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                convertedValue = Convert.ChangeType(value, typeof(bool));
            }
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                convertedValue = Convert.ChangeType(value, typeof(int));
            }
            else if (property.PropertyType == typeof(List<int>))
            {
                convertedValue = Convert.ChangeType(value.Split(',').Select(int.Parse).ToList(), typeof(List<int>));
            }
            else if (property.PropertyType == typeof(List<string>))
            {
                convertedValue = Convert.ChangeType(value.Split(';').ToList(), typeof(List<string>));
            }

            property.SetValue(expertSearchFilterConditionDto, convertedValue);
        }

        /// <summary>
        /// Проверяяет как стоит производить поиск, строго ли нет.
        /// <para></para>
        /// Если возвращает <see langword="true"/>, вызывающий метод <see cref="Execute(HttpRequest)"/> производит поиска параметра через like, а если <see langword="false"/>, то проверяет на строгое соответствие.
        /// </summary>
        /// <param name="propertyName">Название свойства фильтра.</param>
        /// <returns>Как производить поиск.</returns>
        private bool IsWordContains(string propertyName)
        {
            var condition = Conditions.FirstOrDefault(r => r.Key.IndexOf(propertyName, StringComparison.CurrentCultureIgnoreCase) == 0);

            if (condition.Value == "like")
            {
                return true;
            }

            return false;
        }
    }
}