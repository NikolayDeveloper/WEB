using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.AccountingData;

namespace Iserv.Niis.InternalServices.Features.IntegrationGbdJur.Abstractions
{
    /// <summary>
    /// Интерфейс сервиса по работе с ГБДЮЛ.
    /// </summary>
    public interface IGbdJuridicalService
    {
        /// <summary>
        /// Получить информацию о заказчике.
        /// </summary>
        /// <param name="bin">БИН.</param>
        /// <param name="rnn">РНН.</param>
        /// <returns>Заказчик.</returns>
        DicCustomer GetCustomerInfo(string bin, string rnn = null);
    }
}