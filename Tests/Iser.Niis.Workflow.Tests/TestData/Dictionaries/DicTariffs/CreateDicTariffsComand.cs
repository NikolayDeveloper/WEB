using Iserv.Niis.Domain.Entities.Dictionaries;
using NetCoreCQRS.Commands;
using System.Collections.Generic;

namespace Iserv.Niis.Workflow.Tests.TestData.TestData.DicTariffs
{
    public class CreateDicTariffsComand : BaseCommand
    {
        public void Execute(List<DicTariff> dicTariffs)
        {
            var dicDouteStageRepository = Uow.GetRepository<DicTariff>();
            dicDouteStageRepository?.CreateRangeAsync(dicTariffs);
            Uow.SaveChangesAsync();
        }
    }
}