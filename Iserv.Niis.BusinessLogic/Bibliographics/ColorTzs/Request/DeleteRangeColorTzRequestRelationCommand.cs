using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.Request
{
    public class DeleteRangeColorTzRequestRelationCommand : BaseCommand
    {
        public async Task ExecuteAsync(List<DicColorTZRequestRelation> colorTzRequestRelations)
        {
            var colorTzRequestRelationRepo = Uow.GetRepository<DicColorTZRequestRelation>();
            colorTzRequestRelationRepo.DeleteRange(colorTzRequestRelations);
            await Uow.SaveChangesAsync();
        }
    }
}