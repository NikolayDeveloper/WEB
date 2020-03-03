using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.DataBridge.Repositories;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class UpdateRoleRouteStagesHandler : BaseHandler
    {
        private readonly IExecutor _executor;

        public UpdateRoleRouteStagesHandler(IExecutor executor)
        {
            _executor = executor;
        }

        public void Execute(int roleId, int[] stagesIds)
        {
            var existingRoleRouteStares = _executor.GetQuery<GetRoleRouteStageByRoleIdQyery>().Process(q => q.Execute(roleId));

            _executor.CommandChain().AddCommand<DeleteRoleRouteStagesCommand>(c => c.Execute(existingRoleRouteStares, stagesIds))
                .AddCommand<CreateRoleRouteStagesCommand>(c => c.Execute(existingRoleRouteStares, stagesIds, roleId))
                .ExecuteAllWithTransaction();
        }
    }
}
