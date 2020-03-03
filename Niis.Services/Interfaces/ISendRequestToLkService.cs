using System.Threading.Tasks;
using Iserv.Niis.Domain.Intergrations;

namespace Iserv.Niis.Services.Interfaces
{
    /// <summary>
    /// Сервис отправки заявки в ЛК
    /// </summary>
    public interface ISendRequestToLkService
    {
        Task<ServerStatus> Send(int requestId);
    }
}