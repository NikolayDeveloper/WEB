using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GetProtectionDocsByBarcodeQuery : BaseQuery
    {
        public ProtectionDoc Execute(int barCode)
        {
            var repository = Uow.GetRepository<ProtectionDoc>();
            return repository.AsQueryable()
                            .Include(pd => pd.EarlyRegs)
                            .Include(p => p.Request).ThenInclude(er => er.EarlyRegs)
                            .Include(pd => pd.IcgsProtectionDocs).ThenInclude(i => i.Icgs)
                            .Include(pd => pd.Icfems).ThenInclude(i => i.DicIcfem)
                            .FirstOrDefault(pd => pd.Barcode == barCode);
        }
    }
}
