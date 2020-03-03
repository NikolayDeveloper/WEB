using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icfem.Request
{
    public class AddIcfemRequestRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int requestId, List<int> icfemIds)
        {
            var requestRepository = Uow.GetRepository<Domain.Entities.Request.Request>();
            var request = await requestRepository.GetByIdAsync(requestId);
            foreach (var id in icfemIds)
            {
                request.Icfems.Add(new DicIcfemRequestRelation{DicIcfemId = id, RequestId = requestId});
            }
            await Uow.SaveChangesAsync();
        }
    }
}
