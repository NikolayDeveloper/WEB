using System.Threading.Tasks;

namespace Iserv.Niis.Services.Interfaces
{
    public interface IImportDocumentsHelper
    {
        Task ImportFromDb(string number, int requestId);
        Task ImportContractFromDb(int oldContractId, int newContractId);
    }
}