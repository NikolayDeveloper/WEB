using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.InternalServices.Features.IntegrationGbdJur.Abstractions;
using Iserv.Niis.InternalServices.Features.IntegrationGbdJur.Constants;
using Iserv.Niis.InternalServices.Features.Models;
using Iserv.Niis.InternalServices.Features.Utils;
using Microsoft.Extensions.Options;
using Serilog;

namespace Iserv.Niis.InternalServices.Features.IntegrationGbdJur.Implementations
{
    public class GbdJuridicalService : IGbdJuridicalService
    {
        private readonly ExternalServiceGbdJur _externalServiceGbdJur;
        private readonly NiisWebContext _context;

        public GbdJuridicalService(IOptions<ConfigExternalService> configuration, NiisWebContext context)
        {
            _externalServiceGbdJur =
                new ExternalServiceGbdJur(configuration.Value.GbdJurUrl);
            _context = context;
        }

        /// <summary>
        /// Получить информацию о заказчике.
        /// </summary>
        /// <param name="bin">БИН.</param>
        /// <param name="rnn">РНН.</param>
        /// <returns>Заказчик.</returns>
        public DicCustomer GetCustomerInfo(string bin, string rnn = null)
        {
            try
            {
                //var result = _externalServiceGbdJur.FindByBin(InitializeJurInfoByBin(bin, rnn));
                var result = _externalServiceGbdJur.FindByBin(bin);

                if (result.ErrorCode != null)
                {
                    Log.Warning($"Error IntegrationGbdJur: code - {result.ErrorCode}, description - {result.ErrorDescription}");
                    return null;
                }

                var customerInfo = result.JurInfo;
                if (customerInfo == null)
                {
                    Log.Warning($"По БИН - {bin} информация не найдена");
                    return null;
                }

                var statusCode = (StatusCodeGbdJur)Convert.ToInt32(customerInfo.RegStatusCode);
                switch (statusCode)
                {
                    case StatusCodeGbdJur.Success:
                        return GetCustomer(customerInfo);
                    case StatusCodeGbdJur.Abolished:
                        Log.Warning($"В ГБД ЮЛ по БИН - {bin} указан статус ликвидирован");
                        break;
                    case StatusCodeGbdJur.Reorganized:
                        Log.Warning($"В ГБД ЮЛ по БИН - {bin} указан статус \"реорганизован с прекращением деятельности\"");
                        break;
                    default:
                        Log.Warning($"Код ответа не известен {customerInfo?.RegStatusCode ?? string.Empty}");
                        break;
                }
                return null;
            }
            catch(Exception ex)
            {
                Log.Error(ex, ex.Message);
                return null;
            }
        }

        #region PrivateMethods
        private DicCustomer GetCustomer(JurInfo customerInfo)
        {
            var customerTypeJurId = _context.DicCustomerTypes.First(c => c.Code == DicCustomerTypeCodes.Juridical).Id;
            DicCountry countryKz = null;
            int? countryKzId = null;

            if (string.Equals(customerInfo.JurAddress.Country, "КАЗАХСТАН", StringComparison.OrdinalIgnoreCase))
            {
                countryKz = _context.DicCountries.First(c => c.Code == DicCountryCodes.Kazakhstan);
                countryKzId = countryKz.Id;
            }

            return new DicCustomer
            {
                TypeId = GetCustomerTypeId(customerInfo.OrgFormCode) ?? customerTypeJurId,
                Xin = customerInfo.BIN,
                DateCreate = DateTimeOffset.Now,
                Address = GetFullAddress(customerInfo.JurAddress, LanguageType.Ru),
                AddressEn = GetFullAddress(customerInfo.JurAddress, LanguageType.En),
                AddressKz = GetFullAddress(customerInfo.JurAddress, LanguageType.Kz),
                City = GetCityName(customerInfo.JurAddress, LanguageType.Ru),
                Region = GetRegionName(customerInfo.JurAddress, LanguageType.Ru),
                Oblast = GetDistrictName(customerInfo.JurAddress, LanguageType.Ru),
                Republic = GetCountryName(customerInfo.JurAddress, LanguageType.Ru),
                Street = GetStreetName(customerInfo.JurAddress, LanguageType.Ru),
                Apartment = customerInfo.JurAddress.Apartment,
                ShortAddress = GetFullAddress(customerInfo.JurAddress, LanguageType.Ru),
                CertificateNumber = customerInfo.CertNumber,
                CertificateSeries = customerInfo.CertSeries,
                NameRuLong = customerInfo.JurName?.NameRu,
                NameKzLong = customerInfo.JurName?.NameKz,
                NameEnLong = customerInfo.JurName?.NameEn,
                NameRu = customerInfo.JurName?.ShortNameRu ?? customerInfo.JurName?.NameRu,
                NameKz = customerInfo.JurName?.ShortNameKz ?? customerInfo.JurName?.NameKz,
                NameEn = customerInfo.JurName?.ShortNameEn ?? customerInfo.JurName?.NameEn,
                Rnn = customerInfo.RNN,
                RegDate = customerInfo.RegDate,
                ContactName = GetCompanyContactName(customerInfo, LanguageType.Ru),
                Country = countryKz,
                CountryId = countryKzId
            };
        }

        /// <summary>
        /// Получить полный адрес компании.
        /// </summary>
        /// <param name="jurAddress">Адрес регистрации компании.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Полный адрес компании.</returns>
        private string GetFullAddress(JurAddress jurAddress, LanguageType languageType)
        {
            if (jurAddress == null)
            {
                return null;
            }

            return LinesHelper.ConcatNotEmptyStrings(
                GetCountryName(jurAddress, languageType),
                GetDistrictName(jurAddress, languageType),
                GetCityName(jurAddress, languageType),
                GetRegionName(jurAddress, languageType),
                GetStreetType(jurAddress, languageType),
                GetStreetName(jurAddress, languageType),
                jurAddress.House,
                jurAddress.Apartment
            );
        }

        /// <summary>
        /// Получить название страны.
        /// </summary>
        /// <param name="jurAddress">Адрес регистрации компании.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Название страны.</returns>
        private string GetCountryName(JurAddress jurAddress, LanguageType languageType)
        {
            switch (languageType)
            {
                case LanguageType.Ru:
                    return jurAddress.Country;

                case LanguageType.En:
                    return jurAddress.Country;

                case LanguageType.Kz:
                    return jurAddress.CountryKz;

                default:
                    return jurAddress.Country;
            }
        }

        /// <summary>
        /// Получить название области.
        /// </summary>
        /// <param name="jurAddress">Адрес регистрации компании.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Название области.</returns>
        private string GetDistrictName(JurAddress jurAddress, LanguageType languageType)
        {
            switch (languageType)
            {
                case LanguageType.Ru:
                    return jurAddress.District;

                case LanguageType.En:
                    return jurAddress.District;

                case LanguageType.Kz:
                    return jurAddress.DistrictKz;

                default:
                    return jurAddress.District;
            }
        }

        /// <summary>
        /// Получить название города.
        /// </summary>
        /// <param name="jurAddress">Адрес регистрации компании.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Название города.</returns>
        private string GetCityName(JurAddress jurAddress, LanguageType languageType)
        {
            switch (languageType)
            {
                case LanguageType.Ru:
                    return jurAddress.City;

                case LanguageType.En:
                    return jurAddress.City;

                case LanguageType.Kz:
                    return jurAddress.CityKz;

                default:
                    return jurAddress.City;
            }
        }

        /// <summary>
        /// Получить название региона.
        /// </summary>
        /// <param name="jurAddress">Адрес регистрации компании.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Название региона.</returns>
        private string GetRegionName(JurAddress jurAddress, LanguageType languageType)
        {
            switch (languageType)
            {
                case LanguageType.Ru:
                    return jurAddress.Region;

                case LanguageType.En:
                    return jurAddress.Region;

                case LanguageType.Kz:
                    return jurAddress.RegionKz;

                default:
                    return jurAddress.Region;
            }
        }

        /// <summary>
        /// Получить тип улицы.
        /// </summary>
        /// <param name="jurAddress">Адрес регистрации компании.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Тип улицы.</returns>
        private string GetStreetType(JurAddress jurAddress, LanguageType languageType)
        {
            switch (languageType)
            {
                case LanguageType.Ru:
                    return jurAddress.StreetType;

                case LanguageType.En:
                    return jurAddress.StreetType;

                case LanguageType.Kz:
                    return jurAddress.StreetTypeKz;

                default:
                    return jurAddress.StreetType;
            }
        }

        /// <summary>
        /// Получить название улицы.
        /// </summary>
        /// <param name="jurAddress">Адрес регистрации компании.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Название улицы.</returns>
        private string GetStreetName(JurAddress jurAddress, LanguageType languageType)
        {
            switch (languageType)
            {
                case LanguageType.Ru:
                    return jurAddress.Street;

                case LanguageType.En:
                    return jurAddress.Street;

                case LanguageType.Kz:
                    return jurAddress.StreetKz;

                default:
                    return jurAddress.Street;
            }
        }

        /// <summary>
        /// Получает имя контактного лица компании.
        /// </summary>
        /// <param name="jurInfo">Информация о компании.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Имя контактного лица компании.</returns>
        private string GetCompanyContactName(JurInfo jurInfo, LanguageType languageType)
        {
            return jurInfo.HeadJur != null ? GetHeadJurName(jurInfo.HeadJur.Name, languageType) : jurInfo.HeadFiz.FullName;
        }

        /// <summary>
        /// Получить имя юридического контактного лица компании.
        /// </summary>
        /// <param name="jurName">Юридическое имя.</param>
        /// <param name="languageType">Тип языка.</param>
        /// <returns>Имя юридического контактного лица компании.</returns>
        private string GetHeadJurName(JurName jurName, LanguageType languageType)
        {
            switch (languageType)
            {
                case LanguageType.Ru:
                    return jurName.NameRu;

                case LanguageType.En:
                    return jurName.NameEn;

                case LanguageType.Kz:
                    return jurName.NameKz;

                default:
                    return jurName.NameRu;
            }
        }

        private int? GetCustomerTypeId(string code)
        {
            return _context.DicCustomerTypes
                .Where(x => code.Equals(x.Code))
                .Select(x => x.Id)
                .FirstOrDefault();
        }
    }
    #endregion
}
