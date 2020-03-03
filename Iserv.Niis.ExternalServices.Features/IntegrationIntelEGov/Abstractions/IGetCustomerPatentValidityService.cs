using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    /// <summary>
    /// Сервис получения дпатента по номеру и типу
    /// </summary>
    public interface IGetCustomerPatentValidityService
    {
        /// <summary>
        /// Метод получения патента
        /// </summary>
        /// <param name="gosNumber">Гос. Номер</param>
        /// <param name="patentType">Тип патента</param>
        /// <param name="responce">Данные для ответа</param>
        /// <returns></returns>
        CustomerPatentValidityResponce Get(string gosNumber, ReferenceInfo patentType, CustomerPatentValidityResponce responce);
    }
}