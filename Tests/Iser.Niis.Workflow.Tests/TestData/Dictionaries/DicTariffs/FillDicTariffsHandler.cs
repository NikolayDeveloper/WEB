using Iserv.Niis.Domain.Entities.Dictionaries;
using NetCoreCQRS;
using NetCoreCQRS.Handlers;
using System.Collections.Generic;

namespace Iserv.Niis.Workflow.Tests.TestData.TestData.DicTariffs
{
    public class FillDicTariffsHandler : BaseHandler
    {
        private readonly IExecutor _executor;

        public FillDicTariffsHandler(IExecutor executor)
        {
            _executor = executor;
        }

        public int Execute()
        {
            var dicRouteStages = GetDicTariffs();
            _executor.GetCommand<CreateDicTariffsComand>().Process(c => c.Execute(dicRouteStages));
            return 1;
        }

        public List<DicTariff> GetDicTariffs()
        {
            return new List<DicTariff>()
            {
                new DicTariff{Code =DicTariff.Codes.TmFormalExaminationOnPurpose},
                new DicTariff{Code = DicTariff.Codes.TmFormalExaminationEmail}

            };
        }
    }
}
