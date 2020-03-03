using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Iserv.Niis.Exceptions;

namespace Iserv.Niis.Business.Infrastructure
{
    /// <summary>
    /// Базовый абстрактный класс для интеграции с API.
    /// </summary>
    public abstract class BaseIntegrationApiClient
    {
        #region Конструктор
        /// <summary>
        /// Конструктор.
        /// </summary>
        protected BaseIntegrationApiClient()
        {

        }
        #endregion

        #region Конструктор
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="baseAddress">Базовый адрес универсального кода ресурса (URI) интернет-ресурса, используемого при отправке запросов.</param>
        protected BaseIntegrationApiClient(Uri baseAddress)
        {
            BaseAddress = baseAddress;
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Базовый адрес универсального кода ресурса (URI) интернет-ресурса, используемого при отправке запросов.
        /// </summary>
        protected Uri BaseAddress { get; set; }
        #endregion

        #region protected методы
        /// <summary>
        /// Посылает запрос POST как асинхронную операцию указанному Uri с заданным значением, сериализованным как JSON.
        /// </summary>
        /// <typeparam name="TResult">Тип возвращаемого объекта.</typeparam>
        /// <typeparam name="TValue">Тип отправляемого объекта.</typeparam>
        /// <param name="api">Api для получения данных.</param>
        /// <param name="statusCodes">Содержит значения кодов состояний, определенных для протокола HTTP.</param>
        /// <param name="value">Отправляемый объект.</param>
        /// <param name="accessToken">Токен доступа.</param>
        /// <returns>Возвращает объект представляющий ответ сервера.</returns>
        protected async Task<TResult> PostAsync<TResult, TValue>(string api, IEnumerable<HttpStatusCode> statusCodes, TValue value, string accessToken = null) where TResult : class
        {
            using (var httpClient = GetHttpClient(accessToken))
            {
                var requestUri = GetUri(api);

                var responseMessage = await httpClient.PostAsJsonAsync(requestUri, value);

                if (!statusCodes.Contains(responseMessage.StatusCode))
                    throw new HttpStatusCodeException(responseMessage.StatusCode);

                return await responseMessage.Content.ReadAsAsync<TResult>();
            }
        }

        /// <summary>
        /// Отправка запроса GET согласно указанному универсальному коду ресурса (URI) в качестве асинхронной операции.
        /// </summary>
        /// <param name="api">Api для получения данных.</param>
        /// <returns></returns>
        protected async Task<TResult> GetAsync<TResult>(string api) where TResult : class
        {
            return await GetAsync<TResult>(api, null, null);
        }

        /// <summary>
        /// Отправка запроса GET согласно указанному универсальному коду ресурса (URI) в качестве асинхронной операции.
        /// </summary>
        /// <param name="api">Api для получения данных.</param>
        /// <param name="statusCodes">Содержит значения кодов состояний, определенных для протокола HTTP.</param>
        /// <param name="accessToken">Токен доступа.</param>
        /// <returns></returns>
        protected async Task<TResult> GetAsync<TResult>(string api, IEnumerable<HttpStatusCode> statusCodes, string accessToken) where TResult : class
        {
            return await GetAsync<TResult>(api, statusCodes, null, accessToken);
        }

        /// <summary>
        /// Отправка запроса GET согласно указанному универсальному коду ресурса (URI) в качестве асинхронной операции.
        /// </summary>
        /// <param name="api">Api для получения данных.</param>
        /// <param name="statusCodes">Содержит значения кодов состояний, определенных для протокола HTTP.</param>
        /// <param name="parameters">Параметры передаваемые в метод.</param>
        /// <param name="accessToken">Токен доступа.</param>
        /// <returns></returns>
        protected async Task<TResult> GetAsync<TResult>(string api, IEnumerable<HttpStatusCode> statusCodes, Dictionary<string, string> parameters, string accessToken = null) where TResult : class
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();

            using (var httpClient = GetHttpClient(accessToken))
            {
                var requestUri = new Uri(GetUri(api), ToQueryString(ToNameValueCollection(parameters)));

                var responseMessage = await httpClient.GetAsync(requestUri);

                if (!statusCodes.Contains(responseMessage.StatusCode))
                    throw new HttpStatusCodeException(responseMessage.StatusCode);

                var result = await responseMessage.Content.ReadAsAsync<TResult>();
                return result;
            }
        }
        #endregion

        #region private методы
        /// <summary>
        /// Получает адрес универсального кода ресурса (URI) интернет-ресурса, используемого при отправке запросов.
        /// </summary>
        /// <param name="api">Api для получения данных.</param>
        /// <returns>Возвращает базовый адрес универсального кода ресурса (URI) интернет-ресурса, используемого при отправке запросов.</returns>
        private Uri GetUri(string api)
        {
            var baseUrl = BaseAddress.AbsoluteUri;

            return new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/") ? "" : "/")), api);
        }

        /// <summary>
        /// Получает базовый класс для отправки HTTP-запросов и получения HTTP-ответов от ресурса с заданным URI.
        /// </summary>
        /// <param name="accessToken">Токен доступа.</param>
        /// <param name="timeOut">Время ожидания для выполнения запроса.</param>
        /// <returns>Возвращает базовый класс для отправки HTTP-запросов и получения HTTP-ответов от ресурса с заданным URI.</returns>
        private HttpClient GetHttpClient(string accessToken = "", TimeSpan? timeOut = null)
        {
            var httpClient = new HttpClient { BaseAddress = BaseAddress };

            if (timeOut.HasValue)
            {
                httpClient.Timeout = (TimeSpan)timeOut;
            }

            if (string.IsNullOrWhiteSpace(accessToken))
                return httpClient;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        /// <summary>
        /// Осуществляет преобразование Dictionary в NameValueCollection.
        /// </summary>
        /// <param name="parameters">Словарь значений.</param>
        /// <returns></returns>
        private NameValueCollection ToNameValueCollection(Dictionary<string, string> parameters)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var parameter in parameters)
            {
                nameValueCollection.Add(parameter.Key, parameter.Value);
            }

            return nameValueCollection;
        }

        /// <summary>
        /// Формирует параметры для GET запроса.
        /// </summary>
        /// <param name="nameValueCollection">Словарь значений.</param>
        /// <returns></returns>
        private string ToQueryString(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null)
                return string.Empty;
            
            var stringBuilder = new StringBuilder();

            foreach (string key in nameValueCollection.Keys)
            {
                if (string.IsNullOrEmpty(key))
                    continue;

                var values = nameValueCollection.GetValues(key);

                if (values == null)
                    continue;

                foreach (string value in values)
                {
                    stringBuilder.Append(stringBuilder.Length == 0 ? "?" : "&");
                    stringBuilder.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value));
                }
            }

            return stringBuilder.ToString();
        }
        #endregion
    }
}
