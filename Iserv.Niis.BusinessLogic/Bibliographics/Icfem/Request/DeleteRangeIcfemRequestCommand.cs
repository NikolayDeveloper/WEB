using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icfem.Request
{
    public class DeleteRangeIcfemRequestCommand : BaseCommand
    {
        public async Task ExecuteAsync(List<DicIcfemRequestRelation> icfemRequests)
        {
            var icfemRequestRepo = Uow.GetRepository<DicIcfemRequestRelation>();
            icfemRequestRepo.DeleteRange(icfemRequests);
            await Uow.SaveChangesAsync();
        }
    }
}