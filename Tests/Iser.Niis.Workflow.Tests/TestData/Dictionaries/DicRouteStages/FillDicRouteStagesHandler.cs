using System.Collections.Generic;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using NetCoreCQRS;
using NetCoreCQRS.Handlers;

namespace Iserv.Niis.Workflow.Tests.TestData.DicRouteStages.DicRouteStages
{
    //пример заполнения тестовой БД
    public class FillDicRouteStagesHandler : BaseHandler
    {
        private readonly IExecutor _executor;

        public FillDicRouteStagesHandler(IExecutor executor)
        {
            _executor = executor;
        }

        public int Execute()
        {
            var dicRouteStages = GetDicRouteStages();
            _executor.GetCommand<CreateDicRouteStagesCommand>().Process(c => c.Execute(dicRouteStages));
            return 1;
        }
        
        private static List<DicRouteStage> GetDicRouteStages()
        {
            var date = NiisAmbientContext.Current.DateTimeProvider.Now;

            //todo: Заполнить подобным способом все справочники нужные нам для тестирования процессов
            var routeStages = new List<DicRouteStage>
            {
                new DicRouteStage { Code = "IN1.1", DateCreate = date, IsFirst = true, IsLast = false, IsMultiUser = false, IsReturnable = true, NameRu = "Создание входящего документа", IsSystem = false, IsAuto = false, IsMain = true }
            };

            routeStages.AddRange(GetTradeMarkStages());

            return routeStages;
        }

        /// <summary>
        /// Товарные знаки
        /// </summary>
        /// <returns></returns>
        private static List<DicRouteStage> GetTradeMarkStages()
        {
            var date = NiisAmbientContext.Current.DateTimeProvider.Now;

            return new List<DicRouteStage>()
            {
                new DicRouteStage { Code = RouteStageCodes.TZ_02_2, DateCreate = date, IsFirst = false, IsLast = false, IsMultiUser = false, IsReturnable = true, NameRu = "Ввод оплаты", IsSystem = false, IsAuto = false, IsMain = true }
            };
        }
    }


}
