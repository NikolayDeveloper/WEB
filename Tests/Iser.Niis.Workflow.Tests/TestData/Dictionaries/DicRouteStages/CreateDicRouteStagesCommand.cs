using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Dictionaries;
using NetCoreCQRS.Commands;

namespace Iserv.Niis.Workflow.Tests.TestData.DicRouteStages.DicRouteStages
{
    public class CreateDicRouteStagesCommand : BaseCommand
    {
        public void Execute(List<DicRouteStage> dicRouteStage)
        {
            var dicDouteStageRepository = Uow.GetRepository<DicRouteStage>();
            dicDouteStageRepository?.CreateRangeAsync(dicRouteStage);
            Uow.SaveChangesAsync();
        }
    }
}