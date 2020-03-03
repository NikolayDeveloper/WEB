using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.System
{
    public class GetSystemSettingsByTypeQuery: BaseQuery
    {
        public string Execute(SettingType type)
        {
            var repo = Uow.GetRepository<SystemSettings>();

            return repo.AsQueryable().SingleOrDefault(r => r.SettingType == type)?.Value;
        }
    }
}
