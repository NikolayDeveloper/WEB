using System.Linq;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.ContractCustomers
{
    public class IsContractCustomersHasAllCustomerRoleWithCodesHandler : BaseHandler
    {
        public bool Execute(int contractId, string[] customerRoleCodes)
        {
            var existCustomerRoleCodes = Executor.GetQuery<GetContractCustomersByContractIdQuery>()
                .Process(q => q.Execute(contractId))
                .Select(c => c.CustomerRole.Code)
                .ToList();

            return customerRoleCodes.All(c => existCustomerRoleCodes.Contains(c));
        }
    }
}