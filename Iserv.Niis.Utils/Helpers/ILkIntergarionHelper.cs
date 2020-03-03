using Iserv.Niis.Domain.Intergrations;

namespace Iserv.Niis.Utils.Helpers
{
    /// <summary>
    /// Сервис отправки запросов в ЛК
    /// </summary>
    public interface ILkIntergarionHelper
    {
        /// <summary>
        /// Отправка запроса в ЛК
        /// </summary>
        /// <typeparam name="T">Тип обхекта для сериализации</typeparam>
        /// <param name="body">Объект с параметрами для запроса</param>
        /// <param name="action">Тип запроса</param>
        /// <param name="senCode">Код Экшина</param>
        /// <returns>Статус запроса</returns>
        ServerStatus CallWebService<T>(T body, string action, string senCode = "sen");
    }
}