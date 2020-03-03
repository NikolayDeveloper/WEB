using System.Threading.Tasks;
using Iserv.Niis.Model.Models;

namespace Iserv.Niis.Business.Abstract
{
    public interface IDocumentsCompare
    {
        Task<DocumentsCompareDto> GetDocumentsInfoForCompare(int requestId);
    }
}