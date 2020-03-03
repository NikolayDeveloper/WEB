using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
///using NetCoreCQRS.Queries;


namespace Iserv.Niis.BusinessLogic.Documents
{
    public class GetDocumentByBarcodeQuery : BaseQuery
    {
        public async Task<Document> ExecuteAsync(int barcode)
        {
            var repository = Uow.GetRepository<Document>();
            var result = repository.AsQueryable()
                .Include(r => r.Type)
                .Include(r => r.Requests)
                .Include(r => r.ProtectionDocs)
                .Include(r => r.Contracts)
                .FirstOrDefaultAsync(r => r.Barcode == barcode);

            return await result;
        }
    }
}
