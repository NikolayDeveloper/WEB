using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Infrastructure;
using Iserv.Niis.Integration.ApiResult.Models;
using Iserv.Niis.Integration.OneC.Model;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Business.Implementations
{
    /// <summary>
    /// Клиент для интеграции с 1C.
    /// </summary>
    public class IntegrationOneCApiClient : BaseIntegrationApiClient, IIntegrationOneCApiClient
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="configuration">Представляет набор свойств конфигурации приложения ключ / значение.</param>
        public IntegrationOneCApiClient(IConfiguration configuration)
        {
            _configuration = configuration;
            BaseAddress = GetBaseAddress();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="baseAddress">Базовый адрес сервиса.</param>
        public IntegrationOneCApiClient(Uri baseAddress) : base(baseAddress)
        {
        }

        #region Локальные переменные
        /// <summary>
        /// Представляет набор свойств конфигурации приложения ключ / значение.
        /// </summary>
        private readonly IConfiguration _configuration;
        #endregion

        #region public методы
        /// <summary>
        /// Получает платежи в диапазоне дат "с" .. "по".
        /// </summary>
        /// <param name="fromDate">Дата "с".</param>
        /// <param name="toDate">Дата "по".</param>
        /// <returns>Результат выполнения API.</returns>
        public async Task<GetDataApiResult<List<PaymentFrom1CDto>>> GetPaymentsByDateRange(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            const string dateFormat = "yyyy-MM-ddTHH:mm:ss";

            var parameters = new Dictionary<string, string>
            {
                {nameof(fromDate), fromDate.ToString(dateFormat)},
                {nameof(toDate), toDate.ToString(dateFormat)}
            };

            var httpStatusCodes = new List<HttpStatusCode>
            {
                HttpStatusCode.OK,
                HttpStatusCode.InternalServerError
            };

            return await GetAsync<GetDataApiResult<List<PaymentFrom1CDto>>>("/api/v1/Payments/GetPaymentsByDateRange", httpStatusCodes, parameters, await GetAccessToken());
        }

        /// <summary>
        /// Получение токена доступа.
        /// </summary>
        /// <param name="tokenDto">Модель для получения токена.</param>
        /// <returns>Токен доступа.</returns>
        public async Task<GetDataApiResult<string>> GetToken(TokenDto tokenDto)
        {
            var httpStatusCodes = new List<HttpStatusCode>
            {
                HttpStatusCode.OK,
                HttpStatusCode.NotFound
            };

            return await PostAsync<GetDataApiResult<string>, TokenDto>("Auth/Token", httpStatusCodes, tokenDto);
        }
        #endregion

        #region private методы
        /// <summary>
        /// Получение токена доступа.
        /// </summary>
        /// <returns>Токен доступа.</returns>
        private async Task<string> GetAccessToken()
        {
            var accessToken = await GetToken(GetToken());
            return accessToken.Data;
        }

        /// <summary>
        /// Получить базовый адрес универсального кода ресурса (URI) интернет-ресурса, используемого при отправке запросов.
        /// </summary>
        /// <returns>Базовый адрес универсального кода ресурса (URI) интернет-ресурса, используемого при отправке запросов.</returns>
        private Uri GetBaseAddress()
        {
            const string key = "IntegrationOneCApi:ServerUrl";

            var configurationSection = _configuration?.GetSection(key);

            if (configurationSection == null)
                throw new ArgumentNullException(nameof(configurationSection));

            return new Uri(configurationSection.Value);
        }
        #endregion

        #region private методы
        /// <summary>
        /// Получить базовый адрес универсального кода ресурса (URI) интернет-ресурса, используемого при отправке запросов.
        /// </summary>
        /// <returns>Базовый адрес универсального кода ресурса (URI) интернет-ресурса, используемого при отправке запросов.</returns>
        private TokenDto GetToken()
        {
            var configurationSection = _configuration?.GetSection("IntegrationOneCApi:Credential");

            if (configurationSection == null)
                throw new ArgumentNullException(nameof(configurationSection));

            return new TokenDto
            {
                AccessKey = configurationSection["AccessKey"],
                SecretKey = configurationSection["SecretKey"]
            };
        }
        #endregion
    }
}
