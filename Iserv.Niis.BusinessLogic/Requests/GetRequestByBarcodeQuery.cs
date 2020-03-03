using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
//using NetCoreCQRS.Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    /// <summary>
    /// Запрос для получения заявки по его баркоду.
    /// </summary>
    public class GetRequestByBarcodeQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="barcode">Баркод заявки.</param>
        /// <returns>Заявка.</returns>
        public async Task<Request> ExecuteAsync(int barcode)
        {
            var repository = Uow.GetRepository<Request>();

            var result = repository.AsQueryable()
                .Include(r => r.ProtectionDocType)
                .FirstOrDefaultAsync(r => r.Barcode == barcode);

            return await result;
        }
    }
}