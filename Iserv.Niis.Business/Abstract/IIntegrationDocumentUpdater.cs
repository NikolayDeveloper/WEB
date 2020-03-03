using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Business.Abstract
{
    public interface IIntegrationDocumentUpdater
    {
        Task Add(RequestWorkflow requestWorkflow);
    }
}