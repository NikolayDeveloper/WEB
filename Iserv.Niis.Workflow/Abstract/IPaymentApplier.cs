using System.Threading.Tasks;

namespace Iserv.Niis.Workflow.Abstract
{
    /// <summary>
    ///     Механизм применения бизнес-логики этапов при применении оплаты
    /// </summary>
    public interface IPaymentApplier<T>
    {
        /// <summary>
        ///     Метод применения бизнес-логики этапов при применении оплаты
        /// </summary>
        /// <param name="paymentUseId">Идентификатор примененной оплата</param>
        Task ApplyAsync(int paymentUseId);
    }
}