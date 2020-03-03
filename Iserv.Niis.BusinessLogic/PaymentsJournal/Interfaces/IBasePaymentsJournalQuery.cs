using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Microsoft.AspNetCore.Http;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal.Interfaces
{
    /// <summary>
    /// Базовый интерфейс для запросов.
    /// </summary>
    /// <typeparam name="TResult">Тип результирующего объекта.</typeparam>
    public interface IBasePaymentsJournalQuery<TResult>
    {
        /// <summary>
        /// Выполнение метода.
        /// </summary>
        /// <param name="searchParameters">Параметры поиска.</param>
        /// <param name="httpRequest">Запрос.</param>
        /// <returns>Результат.</returns>
        TResult Execute(DocumentsSearchParametersDto searchParameters, HttpRequest httpRequest);
    }
}