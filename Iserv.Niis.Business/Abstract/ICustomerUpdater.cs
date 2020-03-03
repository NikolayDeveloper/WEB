using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.Request;

namespace Iserv.Niis.Business.Abstract
{
    /// <summary>
    /// Механизм обновления субъектов
    /// </summary>
    public interface ICustomerUpdater
    {
        /// <summary>
        /// Метод обновления субъекта
        /// </summary>
        /// <param name="customer">Сущность субъекта</param>
        void Update(DicCustomer customer);
        /// <summary>
        /// Метод обновления адреса адресата из заявки
        /// </summary>
        /// <param name="request">Заявка на ОД</param>
        /// <param name="requestDetailDto">Транспортная модель</param>
        void UpdateAddressee(Request request, RequestDetailDto requestDetailDto);
        /// <summary>
        /// Метод возвращает клиента, если нет в базе, берет из сервисов ГБД ЮЛ/ФЛ затем добовляет в базу
        /// </summary>
        /// <param name="xin">БИН или ИИН</param>
        /// <param name="isPatentAttorney"></param>
        /// <returns>Клиент</returns>
        Task<DicCustomer> GetCustomer(string xin, bool? isPatentAttorney);
    }
}