using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Settings
{
    /// <summary>
    /// Выборка системной константы по ее типу
    /// </summary>
    public class GetSettingsByTypeQuery: BaseQuery
    {
        /// <summary>
        /// Выборка системной константы по ее типу
        /// </summary>
        /// <param name="type">Тип константы</param>
        /// <returns></returns>
        public SystemSettings Execute(SettingType type)
        {
            var repo = Uow.GetRepository<SystemSettings>().AsQueryable();

            return repo.SingleOrDefault(r => r.SettingType == type);
        }
    }
}
