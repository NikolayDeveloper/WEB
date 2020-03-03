using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices
{
    /// <summary>
    /// Запрос, который возвращает статус платежа по его коду.
    /// </summary>
    public class GetPaymentStatusByCodeQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="code">Код статуса.</param>
        /// <returns>Статус платежа.</returns>
        public DicPaymentStatus Execute(string code)
        {
            return Uow.GetRepository<DicPaymentStatus>()
                .AsQueryable()
                .FirstOrDefault(status => status.Code == code && !status.IsDeleted);
        }
    }
}
