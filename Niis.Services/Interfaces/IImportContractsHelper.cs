using System.Threading.Tasks;

namespace Iserv.Niis.Services.Interfaces
{
    public interface IImportContractsHelper
    {
        Task ImportFromDb(string number, int requestId);
    }
}