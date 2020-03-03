using System.Threading.Tasks;
using Iserv.Niis.FileConverter;

namespace Iserv.Niis.Services.Interfaces
{
    /// <summary>
    /// Сервис, отвечающий за работу с платежами.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Генерирует выписку из банка.
        /// </summary>
        /// <param name="paymentUseId">Идентификатор связанного платежа.</param>
        /// <returns></returns>
        Task<GeneratedDocument> GetStatementFromBank(int paymentUseId);
    }
}
