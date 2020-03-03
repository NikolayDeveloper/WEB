using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.Request
{
    public class AddColorTzRequestRelationCommand: BaseCommand
    {
        public async Task ExecuteAsync(int requestId, List<int> colorTzsIds)
        {
            var requestRepository = Uow.GetRepository<Domain.Entities.Request.Request>();
            var request = await requestRepository.GetByIdAsync(requestId);
            foreach (var colorTzId in colorTzsIds)
            {
                request.ColorTzs.Add(
                    new DicColorTZRequestRelation { ColorTzId = colorTzId, RequestId = requestId });
            }
            await Uow.SaveChangesAsync();
        }
    }
}
