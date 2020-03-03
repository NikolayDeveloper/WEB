using System.Threading.Tasks;

namespace Iserv.Niis.Services.Interfaces
{
    public interface IImportPaymentsHelper
    {
        Task ImportFromDb(string number, int requestId);
    }
}