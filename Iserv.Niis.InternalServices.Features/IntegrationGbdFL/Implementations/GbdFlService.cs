using System;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.InternalServices.Features.IntegrationGbdFL.Abstractions;
using Iserv.Niis.InternalServices.Features.Models;
using Iserv.Niis.InternalServices.Features.Utils;
using Microsoft.Extensions.Options;
using Serilog;

namespace Iserv.Niis.InternalServices.Features.IntegrationGbdFL.Implementations
{
    public class GbdFlService : IGbdFlService
    {
        private readonly GbdFLProxyWebService _flService;
        private readonly NiisWebContext _context;

        public GbdFlService(IOptions<ConfigExternalService> configuration, NiisWebContext context)
        {
            _flService = new GbdFLProxyWebService(configuration.Value.GbdFlUrl);
            _context = context;
        }
        public GbdFlService(string url)
        {
            _flService = new GbdFLProxyWebService(url);
        }

        public DicCustomer GetCustomer(string iin)
        {
            Return result = null;
            try
            {
                result = _flService.FindPersonByIin(iin);
                if(result?.messageResult.code != "00000")
                {
                    Log.Warning($"Код ошибки: {result?.messageResult.code ?? string.Empty}, описание ошибки: {result?.messageResult.nameRu ?? string.Empty}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, result?.messageResult.nameRu ?? string.Empty);
                return null;
            }
            if (result.persons.Any() == false)
            {
                Log.Warning($"По ИИН - {iin} информация не найдена");
                return null;
            }
            if(result.persons.Length > 1)
            {
                Log.Warning($"По ИИН - {iin} найдено несколько человек");
                result.persons.First();
            }

            return ConvertToDicCustomer(result.persons.First());
        }

        #region PrivateMethods

        private DicCustomer ConvertToDicCustomer(Person person)
        {
            var customerTypePhysicalId = _context.DicCustomerTypes.First(c => c.Code == DicCustomerTypeCodes.Physical).Id;
            DicCountry countryKz = null;
            int? countryKzId = null;

            if (person.citizenship.code == "398")
            {
                countryKz = _context.DicCountries.First(c => c.Code == DicCountryCodes.Kazakhstan);
                countryKzId = countryKz.Id;
            }

            return new DicCustomer
            {
                DateCreate = DateTimeOffset.Now,
                DateUpdate = DateTimeOffset.Now,
                Xin = person.iin,
                NameRu = $"{person.fio.surname ?? string.Empty} {person.fio.firstname ?? string.Empty} {person.fio.secondname ?? string.Empty}",
                Address = GetFullAddress(person.regAddress, LanguageType.Ru),
                AddressEn = GetFullAddress(person.regAddress, LanguageType.En),
                AddressKz = GetFullAddress(person.regAddress, LanguageType.Kz),
                City = string.IsNullOrEmpty(person.regAddress.city)
                    ? person.regAddress.districts.nameRu
                    : person.regAddress.city,
                Region = GetRegionName(person.regAddress, LanguageType.Ru),
                Oblast = GetDistrictName(person.regAddress, LanguageType.Ru),
                Republic = GetCountryName(person.regAddress, LanguageType.Ru),
                Street = person.regAddress.street,
                ShortAddress = GetFullAddress(person.regAddress, LanguageType.Ru),
                TypeId = customerTypePhysicalId,
                Apartment = person.regAddress.flat,
                Country = countryKz,
                CountryId = countryKzId
            };
        }

        /// <summary>
        /// Получить полный адрес человека.
        /// </summary>
        /// <param name="regAddress">Адрес регистрации человека.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Полный адрес человека.</returns>
        private string GetFullAddress(RegAddress regAddress, LanguageType languageType)
        {
            return LinesHelper.ConcatNotEmptyStrings(
                GetCountryName(regAddress, languageType),
                GetDistrictName(regAddress, languageType),
                regAddress.city,
                GetRegionName(regAddress, languageType),
                regAddress.street,
                regAddress.building,
                regAddress.corpus,
                regAddress.flat
            );
        }

        /// <summary>
        /// Получить название страны.
        /// </summary>
        /// <param name="regAddress">Адрес регистрации человека.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Название страны.</returns>
        private string GetCountryName(RegAddress regAddress, LanguageType languageType)
        {
            if (regAddress.country == null)
                return null;

            switch (languageType)
            {
                case LanguageType.Ru:
                    return regAddress.country.nameRu;

                case LanguageType.En:
                    return regAddress.country.nameRu;

                case LanguageType.Kz:
                    return regAddress.country.nameKz;

                default:
                    return regAddress.country.nameRu;
            }
        }

        /// <summary>
        /// Получить название области.
        /// </summary>
        /// <param name="regAddress">Адрес регистрации человека.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Название области.</returns>
        private string GetDistrictName(RegAddress regAddress, LanguageType languageType)
        {
            if (!string.IsNullOrEmpty(regAddress.districtName))
                return regAddress.districtName;

            switch (languageType)
            {
                case LanguageType.Ru:
                    return regAddress.districts.nameRu;

                case LanguageType.En:
                    return regAddress.districts.nameRu;

                case LanguageType.Kz:
                    return regAddress.districts.nameKz;

                default:
                    return regAddress.districts.nameRu;
            }
        }

        /// <summary>
        /// Получить название региона.
        /// </summary>
        /// <param name="regAddress">Адрес регистрации человека.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Название региона.</returns>
        private string GetRegionName(RegAddress regAddress, LanguageType languageType)
        {
            if (!string.IsNullOrEmpty(regAddress.regionName))
                return regAddress.regionName;

            switch (languageType)
            {
                case LanguageType.Ru:
                    return regAddress.region.nameRu;

                case LanguageType.En:
                    return regAddress.region.nameRu;

                case LanguageType.Kz:
                    return regAddress.region.nameKz;

                default:
                    return regAddress.region.nameRu;
            }
        }
        #endregion
    }
}